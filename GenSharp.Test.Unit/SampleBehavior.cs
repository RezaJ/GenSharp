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
using System.Threading;

namespace GenSharp.Test.Unit
{
    public class SampleBehavior
    {
        public Future InitGotCalled = new Future();
        public Future TerminateGotCalled = new Future();
        public Future ErrorHandlerGotCalled = new Future();
        public Future InfoHandlerGotCalled = new Future();
        public int CurrentValue;

        public string StringPost = null;
        public bool IntPostGotCalled = false;

        [Init]
        public void InitializeTheServer()
        {
            InitGotCalled.Complete(true);
            CurrentValue = 0;
        }

        [Terminate]
        public void TerminateTheServer()
        {
            TerminateGotCalled.Complete(true);
            Thread.Sleep(200);
        }

        [Post, Send]
        public int Add(int value)
        {
            CurrentValue += value;
            return CurrentValue;
        }

        [Post]
        public void Multiply(int value)
        {
            CurrentValue *= value;
        }

        [Post]
        public void Divide(int value)
        {
            CurrentValue /= value;
        }

        [Send]
        public int MultiplyAndReturn(int value)
        {
            if (value > 99999999)
                throw new OverflowException("Overflow Exception.");

            CurrentValue *= value;
            return CurrentValue;
        }

        [Send]
        public int Equals()
        {
            return CurrentValue;
        }

        [Error]
        public int ErrorHandler(Exception exception, RequestType type, string methodName, object message)
        {
            ErrorHandlerGotCalled.Complete(new ErrorData
            {
                ExceptionMessage = exception.Message,
                MethodName = methodName,
                Value = ((object[])message)[0]
            });
            return 666666;
        }

        [Info]
        public int ErrorHandler(RequestType type, string methodName, object message)
        {
            InfoHandlerGotCalled.Complete(new ErrorData
            {
                MethodName = methodName,
                Value = ((object[])message)[0]
            });
            return 55555;
        }
    }

    public class ErrorData
    {
        public object Value;

        public string ExceptionMessage { get; set; }

        public string MethodName { get; set; }
    }
}
