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

using System.Threading.Tasks;
using NUnit.Framework;

namespace GenSharp.Test.Unit
{

    [TestFixture]
    public class InvokeTerminateRoleTests
    {
        /// <summary>
        /// Test for invoke terminate role
        /// </summary>
        [TestCase]
        public void InvokeTerminateTest()
        {
            var target = new SampleBehavior();
            var terminationEvent = new Future();
            var server = new GenSharp(target);
            server.Callbacks.LoadCallbacks(target.GetType());

            var request = new Request
                {
                    Type = RequestType.Terminate,
                    Server = server,
                    Reply = terminationEvent
                };

            Task.Factory.StartNew(() => { request.InvokeTerminate(); });

            Assert.IsTrue(target.TerminateGotCalled.WaitUntilCompleted<bool>(100));
            Assert.AreEqual(GenSharpStates.Terminating, server.State);

            Assert.IsTrue(terminationEvent.WaitUntilCompleted<bool>(1000));
            Assert.AreEqual(GenSharpStates.Stopped, server.State);

            Assert.AreEqual(0, target.CurrentValue);
        }
    }
}
