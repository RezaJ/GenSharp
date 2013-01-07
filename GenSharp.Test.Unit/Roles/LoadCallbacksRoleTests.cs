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

using NUnit.Framework;

namespace GenSharp.Test.Unit
{

    /// <summary>
    /// Tests that loaded callback dictionary matches the provided behavior class
    /// </summary>
    [TestFixture]
    public class LoadCallbacksRoleTests
    {
        [TestCase]
        public void LoadCallbacksTest()
        {
            var target = new CallbackDictionary();
            target.LoadCallbacks(typeof(SampleBehavior));

            Assert.AreEqual(6, target.Count);
            Assert.AreEqual(1, target[AttributeType.Init]["InitializeTheServer"].Count);
            Assert.AreEqual(1, target[AttributeType.Terminate]["TerminateTheServer"].Count);
            Assert.AreEqual(1, target[AttributeType.Post]["Add"].Count);
            Assert.AreEqual(1, target[AttributeType.Post]["Multiply"].Count);
            Assert.AreEqual(1, target[AttributeType.Post]["Divide"].Count);
            Assert.AreEqual(typeof(int), target[AttributeType.Send]["Add"][0].ReturnType);
            Assert.AreEqual(1, target[AttributeType.Send]["MultiplyAndReturn"].Count);
            Assert.AreEqual(1, target[AttributeType.Send]["Equals"].Count);
            Assert.AreEqual(4, target[AttributeType.Error]["ErrorHandler"][0].GetParameters().Length);
            Assert.AreEqual(3, target[AttributeType.Info]["ErrorHandler"][0].GetParameters().Length);
        }
    }
}
