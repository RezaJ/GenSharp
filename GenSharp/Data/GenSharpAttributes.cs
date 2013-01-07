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

namespace GenSharp
{
    public static class GenSharpAttributes
    {
        public static AttributeType GetTypeCode(GenSharpAttribute attribute)
        {
            if (attribute is InitAttribute) return AttributeType.Init;
            if (attribute is SendAttribute) return AttributeType.Send;
            if (attribute is PostAttribute) return AttributeType.Post;
            if (attribute is InfoAttribute) return AttributeType.Info;
            if (attribute is ErrorAttribute) return AttributeType.Error;
            return AttributeType.Terminate;
        }
    }

    public enum AttributeType : byte
    {
        Init,
        Send,
        Post,
        Info,
        Error,
        Terminate
    }

    public class GenSharpAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class InitAttribute : GenSharpAttribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SendAttribute : GenSharpAttribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PostAttribute : GenSharpAttribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class InfoAttribute : GenSharpAttribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ErrorAttribute : GenSharpAttribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TerminateAttribute : GenSharpAttribute { }

}
