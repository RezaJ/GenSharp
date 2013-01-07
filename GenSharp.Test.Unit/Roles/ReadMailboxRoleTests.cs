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
using NUnit.Framework;

namespace GenSharp.Test.Unit
{
    /// <summary>
    /// Tests the role to read the server mail box
    /// </summary>
    [TestFixture]
    public class ReadMailboxRoleTests
    {
        [TestCase]
        public void ProcessTest_Post()
        {
            var expectedRequest = new Request {MethodName = "TEST"};
            var target = new BlockingCollection<Request>(1) { expectedRequest };
            
            var retrievedRequest = target.ReadMailbox();

            Assert.AreSame(expectedRequest, retrievedRequest);
            Assert.AreEqual(0, target.Count);
        }
    }
}
