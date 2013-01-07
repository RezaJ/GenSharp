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
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace GenSharp.Test.Acceptance
{
    [Binding]
    public class GenericServerSteps
    {
        SampleBehavior behavior;
        GenSharp server;

        [Given(@"I have a sample behavior")]
        public void GivenIHaveASampleBehavior()
        {
            behavior = new SampleBehavior();
        }
        
        [StepDefinition(@"I start my new server")]
        public void WhenIStartMyNewServer()
        {
            server = new GenSharp(behavior, false, 10);
            server.Start();
        }
        
        [When(@"I stop the server")]
        public void WhenIStopTheServer()
        {
            var serverIsStopped = server.Stop();
            Assert.IsTrue(serverIsStopped.WaitUntilCompleted<bool>(3000000));
        }
        
        [Then(@"The server status should be (.*)")]
        public void ThenTheServerStatusShouldBeStarted(string state)
        {
            Assert.AreEqual(Enum.Parse(typeof(GenSharpStates), state), server.State);
        }
        
        [Then(@"Init callback should be invoked")]
        public void ThenInitCallbackShouldHaveBeenInvoked()
        {
            Assert.IsTrue(behavior.InitGotCalled.WaitUntilCompleted<bool>(2000000));
        }

        [Then(@"Terminate callback should not be invoked yet")]
        public void ThenTerminateShouldNotBeInvokedYet()
        {
            Assert.IsFalse(behavior.TerminateGotCalled.IsCompleted);
        }

        [Then(@"Terminate callback should be invoked")]
        public void ThenTerminateShouldBeInvokedYet()
        {
            Assert.IsTrue(behavior.TerminateGotCalled.WaitUntilCompleted<bool>(2000000));
        }

        [When(@"I do a post on method (.*) by value (.*)")]
        public void WhenPostOnMethodAddWithValue(string method, int value)
        {
            server.Post(method, value);
        }

        [When(@"I do a call on method (.*) by value (.*) the reply is (.*)")]
        public void WhenCallOnMethodAddWithValueTheReplyIs(string method, int value, int reply)
        {
            Assert.AreEqual(reply, server.Call<int>(method, 2000000, value));
        }


        [Then(@"I do a call on method (.*) and the reply is (.*)")]
        public void ThenCallOnMethodAndReplyIs(string method, int reply)
        {
            Assert.AreEqual(reply, server.Call<int>(method, 3000000, null));
        }

        [Then(@"The Error handler gets called for method (.*) with message (.*) and error (.*)")]
        public void ThenErrorHandlerGetsCalled(string methodName, int value, string errorMessage)
        {
            var errorData = behavior.ErrorHandlerGotCalled.WaitUntilCompleted<ErrorData>(2000000);
            behavior.ErrorHandlerGotCalled = new Future();
            Assert.AreEqual(methodName, errorData.MethodName);
            Assert.AreEqual(value, errorData.Value);
            Assert.AreEqual(errorMessage, errorData.ExceptionMessage);
        }

        [Then(@"The Info handler gets called for method (.*) with message (.*)")]
        public void ThenInfoHandlerGetsCalled(string methodName, int value)
        {
            var infoData = behavior.InfoHandlerGotCalled.WaitUntilCompleted<ErrorData>(2000000);
            behavior.InfoHandlerGotCalled = new Future();

            Assert.AreEqual(methodName, infoData.MethodName);
            Assert.AreEqual(value, infoData.Value);
        }


        [AfterScenario("terminate_server")]
        public void TerminateServer()
        {
            var serverIsStopped = server.Stop();
            Assert.IsTrue(serverIsStopped.WaitUntilCompleted<bool>(3000000));
        }
    }

    public class SampleBehavior
    {
        public Future InitGotCalled = new Future();
        public Future TerminateGotCalled = new Future();
        public Future ErrorHandlerGotCalled = new Future();
        public Future InfoHandlerGotCalled = new Future();
        private int currentValue;

        public string StringPost = null;
        public bool IntPostGotCalled = false;

        [Init]
        public void InitializeTheServer()
        {
            InitGotCalled.Complete(true);
            currentValue = 0;
        }

        [Terminate]
        public void TerminateTheServer()
        {
            TerminateGotCalled.Complete(true);
        }

        [Post, Send]
        public int Add(int value)
        {
            currentValue += value;
            return currentValue;
        }

        [Post]
        public void Multiply(int value)
        {
            currentValue *= value;
        }

        [Post]
        public void Divide(int value)
        {
            currentValue /= value;
        }

        [Send]
        public int MultiplyAndReturn(int value)
        {
            if (value > 99999999)
                throw new OverflowException("Overflow Exception.");

            currentValue *= value;
            return currentValue;
        }
        
        [Send]
        public int Equals()
        {
            return currentValue;
        }

        [Error]
        public int ErrorHandler(Exception exception, RequestType type, string methodName, object message)
        {
            ErrorHandlerGotCalled.Complete(new ErrorData {
                ExceptionMessage = exception.InnerException.Message,
                MethodName = methodName,
                Value = ((object[]) message)[0]
            });
            return currentValue;
        }

        [Info]
        public int ErrorHandler(RequestType type, string methodName, object message)
        {
            InfoHandlerGotCalled.Complete(new ErrorData
            {
                MethodName = methodName,
                Value = ((object[])message)[0]
            });
            return currentValue;
        }
    }

    class ErrorData
    {
        public object Value;

        public string ExceptionMessage { get; set; }

        public string MethodName { get; set; }
    }
}
