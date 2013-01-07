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
    public static class InvokeInfoRole
    {
        /// <summary>
        /// This invocation takes place when the matching method signature with provided request is not found.
        /// </summary>
        /// <param name="request"></param>
        public static void InvokeInfo(this Request request)
        {
            object reply = null;
            var server = request.Server;

            var infoMethod = server.Callbacks.GetMethod(AttributeType.Info);
            if (infoMethod != null)
                reply = infoMethod.Invoke(request.Server.Behavior,
                                          new object[] { request.Type, request.MethodName, request.Messages });

            if (request.Type == RequestType.Send)
                request.Reply.Complete(reply);
        }
    }
}
