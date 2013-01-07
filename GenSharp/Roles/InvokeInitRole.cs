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

namespace GenSharp
{
    public static class InvokeInitRole
    {
        /// <summary>
        /// Retrieves the init callback method and invokes it.
        /// </summary>
        /// <param name="callbackDictionary"></param>
        /// <param name="behavior"></param>
        public static void InvokeInit(this CallbackDictionary callbackDictionary, object behavior)
        {
            var initMethod = callbackDictionary.GetMethod(AttributeType.Init);
            if (initMethod != null)
                initMethod.Invoke(behavior, null);
        }
    }
}
