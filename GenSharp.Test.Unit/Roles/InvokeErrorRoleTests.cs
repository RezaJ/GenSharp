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
    public class InvokeErrorRoleTests
    {
        /// <summary>
        /// Test for invoke error role
        /// </summary>
        [TestCase]
        public void InvokeErrorTest()
        {
            var target = new SampleBehavior();
            var reply = new Future();
            var server = new GenSharp(target);
            server.Callbacks.LoadCallbacks(target.GetType());

            var request = new Request
                {
                    MethodName = "MyBadMethod",
                    Type = RequestType.Send,
                    Messages = new object[]{100},
                    Server = server,
                    Reply = reply
                };

            Task.Factory.StartNew(() => { request.InvokeError(new Exception("Sample Error")); });

            var errorData = target.ErrorHandlerGotCalled.WaitUntilCompleted<ErrorData>(200);

            Assert.AreEqual("Sample Error", errorData.ExceptionMessage);
            Assert.AreEqual("MyBadMethod", errorData.MethodName);
            Assert.AreEqual(100, errorData.Value);

            Assert.AreEqual(666666, reply.WaitUntilCompleted<int>(100)); // returned response from error handler
            Assert.AreEqual(0, target.CurrentValue); // value should have not changed
        }
    }
}
