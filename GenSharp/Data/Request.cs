﻿//   Copyright 2013 RezaJ
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

namespace GenSharp
{
    public enum RequestType
    {
        Post,
        Send,
        Terminate
    }

    public class Request
    {
        public RequestType Type { get; set; }
        public object[] Messages { get; set; }
        public string MethodName { get; set; }
        public Future Reply { get; set; }
        public GenSharp Server { get; set; }
    }
}
