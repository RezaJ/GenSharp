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

namespace GenSharp
{
    public class Future :
		IAsyncResult
	{
		readonly ManualResetEvent resetEvent;
		readonly object state;
		volatile bool isCompleted;
        object result;


        public Future() : this(null)
        {
        }

        public Future(object state)
		{
		    isCompleted = false;
            this.state = state;
			resetEvent = new ManualResetEvent(false);
		}

		public bool IsCompleted
		{
			get { return isCompleted; }
		}

		public WaitHandle AsyncWaitHandle
		{
            get { return resetEvent; }
		}

		public object AsyncState
		{
			get { return state; }
		}

		public bool CompletedSynchronously
		{
			get { return false; }
		}

        public void Complete(object value)
        {
            if (isCompleted)
            {
                throw new InvalidOperationException("A Future cannot be completed twice");
            }

            result = value;

            isCompleted = true;

            resetEvent.Set();
        }

        public T WaitUntilCompleted<T>(int timeout)
		{
            resetEvent.WaitOne(timeout);
            return (T)result;
		}

        public T WaitUntilCompleted<T>(TimeSpan timeout)
		{
            resetEvent.WaitOne(timeout);
            return (T)result;
		}

        ~Future()
		{
            resetEvent.Close();
		}
	}
}