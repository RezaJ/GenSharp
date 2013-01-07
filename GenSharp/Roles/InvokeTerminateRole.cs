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

namespace GenSharp
{
    public static class InvokeTerminateRole
    {
        /// <summary>
        /// Occurs when stopping the server.
        /// </summary>
        /// <param name="request"></param>
        public static void InvokeTerminate(this Request request)
        {
            var server = request.Server;

            server.State = GenSharpStates.Terminating;

            var terminateMethod = server.Callbacks.GetMethod(AttributeType.Terminate);
            if (terminateMethod != null)
                terminateMethod.Invoke(server.Behavior, null);

            server.Callbacks.Clear();
            server.Mailbox.Dispose();
            server.State = GenSharpStates.Stopped;

            request.Reply.Complete(true);
        }
    }
}
