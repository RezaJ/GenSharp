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
    /// <summary>
    /// Brings up the server
    /// </summary>
    internal class InitializeGenSharp
    {
        readonly GenSharp server;

        public InitializeGenSharp(GenSharp newServer)
        {
            server = newServer;
        }

        public void DoIt()
        {
            server.State = GenSharpStates.Initializing;
            try
            {
                server.Callbacks = new CallbackDictionary();
                server.Callbacks.LoadCallbacks(server.Behavior.GetType());
                server.Callbacks.InvokeInit(server.Behavior);
            }
            catch (Exception)
            {
                server.State = GenSharpStates.Stopped;
                throw;
            }
          
            server.State = GenSharpStates.Started;
        }
    }
}
