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
using System.Linq;

namespace GenSharp
{
    public static class LoadCallbacksRole
    {
        /// <summary>
        /// Loads all the callback methods in the given behavior class marked with attributes
        /// </summary>
        /// <param name="callbackDictionary">The callback container that this method will be injected to</param>
        /// <param name="behaviorType">User-defined behavior</param>
        public static void LoadCallbacks(this CallbackDictionary callbackDictionary, Type behaviorType)
        {
            foreach (var method in behaviorType.GetMethods())
                foreach (var attribute in method.GetCustomAttributes(true).OfType<GenSharpAttribute>())
                    callbackDictionary.AddMethod(attribute, method);
        }
    }
}
