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
using System.Threading.Tasks;
using NUnit.Framework;

namespace GenSharp.Test.Unit
{

    [TestFixture]
    public class InvokeInfoRoleTests
    {
        /// <summary>
        /// Test for invoke info role
        /// </summary>
        [TestCase]
        public void InvokeInfoTest()
        {
            var target = new SampleBehavior();
            var reply = new Future();
            var server = new GenSharp(target);
            server.Callbacks.LoadCallbacks(target.GetType());

            var request = new Request
            {
                MethodName = "MultiplyAndReturn",
                Type = RequestType.Send,
                Messages = new object[] { 500, "ExtraParamThatDoesNotMatch" },
                Server = server,
                Reply = reply
            };

            Task.Factory.StartNew(() => { request.InvokeInfo(); });

            var errorData = target.InfoHandlerGotCalled.WaitUntilCompleted<ErrorData>(200);

            Assert.AreEqual("MultiplyAndReturn", errorData.MethodName);
            Assert.AreEqual(500, errorData.Value);

            Assert.AreEqual(55555, reply.WaitUntilCompleted<int>(100)); // returned response from error handler
            Assert.AreEqual(0, target.CurrentValue); // value should have not changed
        }
    }
}
