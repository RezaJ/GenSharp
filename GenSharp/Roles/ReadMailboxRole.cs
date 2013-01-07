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

using System.Collections.Concurrent;
using System.Threading;

namespace GenSharp
{
    public static class ReadMailboxRole
    {
        /// <summary>
        /// Processes the next available request from mailbox  and dispatches the request 
        /// to the corresponding invocation method.
        /// In case of an error, "Error" callback method will get called.
        /// If no request is available it waits until a requests gets
        /// enqueued in the server. 
        /// </summary>
        /// <param name="mailbox"></param>
        public static Request ReadMailbox(this BlockingCollection<Request> mailbox)
        {
            Request request;
            return mailbox.TryTake(out request, Timeout.Infinite) ? request : null;
        }
    }
}
