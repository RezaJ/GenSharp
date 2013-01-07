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
    public class InvokePostRoleTests
    {
        /// <summary>
        /// Test for invoke post role
        /// </summary>
        [TestCase]
        public void InvokePostTest()
        {
            var target = new SampleBehavior();
            var server = new GenSharp(target);
            server.Callbacks.LoadCallbacks(target.GetType());

            var request = new Request
                {
                    MethodName = "Add",
                    Type = RequestType.Post,
                    Messages = new object[]{100},
                    Server = server,
                };

            Task.Factory.StartNew(() => { request.InvokePost(); });

            Assert.AreEqual(100, target.CurrentValue);
        }
    }
}
