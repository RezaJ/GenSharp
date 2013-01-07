//   Copyright 2013 RezaJ
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace GenSharp
{
    public class GenSharp
    {
        readonly bool longRunningServer;
        volatile GenSharpStates state = GenSharpStates.Stopped;
        public GenSharpStates State
        {
            get { return state; }
            set { state = value; }
        }
 
        public CallbackDictionary Callbacks { get; set; }
        internal BlockingCollection<Request> Mailbox { get; set; }
        internal object Behavior { get; private set; }

        public GenSharp(object behavior):this(behavior, true, 0) {}

        public GenSharp(object behavior, bool longRunningServer, int queueSize)
        {
            this.longRunningServer = longRunningServer;
            Mailbox = (queueSize < 1)
                          ? new BlockingCollection<Request>()
                          : new BlockingCollection<Request>(queueSize);
            Callbacks = new CallbackDictionary();
            Behavior = behavior;
        }

        /// <summary>
        /// Starts the server and returns the task that server loop is running on.
        /// </summary>
        /// <returns></returns>
        public Task Start()
        {
            return Task.Factory.StartNew(() =>
                {
                    new InitializeGenSharp(this).DoIt();
                    new ProcessMailbox(this).DoIt();
                }, longRunningServer ? TaskCreationOptions.LongRunning : TaskCreationOptions.None);   
        }

        /// <summary>
        /// Enqueues a post (aka. cast) request to server, the control returns directly right after. 
        /// </summary>
        /// <param name="postMethodName">The post callback method name in behavior class.</param>
        /// <param name="messages">Messages (parameters) to the callback method</param>
        /// <returns>Returns true if the request is queued successfully for execution.</returns>
        public bool Post(string postMethodName, params object[] messages)
        {
            return Mailbox.TryAdd(new Request
                {
                    MethodName = postMethodName,
                    Type = RequestType.Post,
                    Messages = messages,
                    Server = this
                }, Timeout.Infinite);
        }

        /// <summary>
        /// Enqueues a send request to server. Control returns directly right after to the calling method.
        /// </summary>
        /// <param name="sendMethodName">The send callback method name.</param>
        /// <param name="messages">Messages (parameters) to the callback method</param>
        /// <returns>Returns a future reply (IAsyncResult) that will be set by the server after the
        /// request is executed and the reply is available.</returns>
        public Future Send(string sendMethodName, params object[] messages)
        {
            var reply = new Future();
            Mailbox.TryAdd(new Request
            {
                MethodName = sendMethodName,
                Type = RequestType.Send,
                Messages = messages,
                Reply = reply,
                Server = this
            });
            return reply;
        }

        /// <summary>
        /// Enqueues a termination request in the server. After this call no new request can be placed but
        /// the existing requests will still get processed in the same order as they were placed. Control
        /// returns directly right after to the calling method.
        /// </summary>
        /// <returns>Return a future reply (IAsyncResult) that will be set after running the
        /// terminating callback and when the server is stopped.</returns>
        public Future Stop()
        {
            var reply = new Future();
            Mailbox.TryAdd(new Request
            {
               Type = RequestType.Terminate,
               Reply = reply,
               Server = this
            }, Timeout.Infinite);
            Mailbox.CompleteAdding();
            return reply;
        }

        /// <summary>
        /// Enqueues a send request but the the control does NOT return to the calling method until after the
        /// request is processed by the server.
        /// </summary>
        /// <typeparam name="T">The expected return type from the callback method.</typeparam>
        /// <param name="sendMethodName">The send callback method name.</param>
        /// <param name="messages">Messages (parameters) to the callback method</param>
        /// <returns>Reply from callback method</returns>
        public T Call<T>(string sendMethodName, params object[] messages)
        {
            var reply = Send(sendMethodName, messages);
            return reply.WaitUntilCompleted<T>(Timeout.Infinite);
        }

        /// <summary>
        /// Enqueues a send request but the the control does NOT return to the calling method until after the
        /// request is processed by the server. If by the given timeout the request is not processed the 
        /// control returns to the calling method regardless but the request does NOT get cancelled.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendMethodName">The send callback method name.</param>
        /// <param name="timeout">Timeout in miliseconds</param>
        /// <param name="messages">Messages (parameters) to the callback method</param>
        /// <returns>Reply from callback method</returns>
        public T Call<T>(string sendMethodName, int timeout, params object[] messages)
        {
            var reply = Send(sendMethodName, messages);
            return reply.WaitUntilCompleted<T>(timeout);
        }

        /// <summary>
        /// Enqueues a send request but the the control does NOT return to the calling method until after the
        /// request is processed by the server. If by the given timeout the request is not processed the 
        /// control returns to the calling method regardless but the request does NOT get cancelled.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendMethodName">The send callback method name.</param>
        /// <param name="timeout">Timeout</param>
        /// <param name="messages">Messages (parameters) to the callback method</param>
        /// <returns>Reply from callback method</returns>
        public T Call<T>(string sendMethodName, TimeSpan timeout, params object[] messages)
        {
            var reply = Send(sendMethodName, messages);
            return reply.WaitUntilCompleted<T>(timeout);
        }
    }
}
