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

namespace GenSharp
{
    public static class InvokeErrorRole
    {
        /// <summary>
        /// This invocation takes place in case an error happens while executing post, send, terminate or info
        /// callback and provides some inforamtion to take neccessary actions if possible.
        /// The return value from this invocation will be used for send callback if the error happend while
        /// a send method was executing.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="exception"></param>
        public static void InvokeError(this Request request, Exception exception)
        {
            object reply = null;
            var server = request.Server;
            var errorMethod = server.Callbacks.GetMethod(AttributeType.Error);
            if (errorMethod != null)
                reply = errorMethod.Invoke(request.Server.Behavior,
                                  new object[] { exception, request.Type, request.MethodName, request.Messages });

            if (request.Type == RequestType.Send)
                request.Reply.Complete(reply ?? exception);
        }
    }
}
