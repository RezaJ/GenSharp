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
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace GenSharp
{
    public class CallbackDictionary : Dictionary<AttributeType, Dictionary<String, List<MethodInfo>>>
    {
        public void AddMethod(GenSharpAttribute attribute, MethodInfo method)
        {
            var key = GenSharpAttributes.GetTypeCode(attribute);

            if (!ContainsKey(key))
                this[key] = new Dictionary<string, List<MethodInfo>>();

            var selectedAttributeType = this[key];

            if(!selectedAttributeType.ContainsKey(method.Name))
                selectedAttributeType.Add(method.Name, new List<MethodInfo>(5));

            selectedAttributeType[method.Name].Add(method);
        }

        internal MethodInfo GetMethod(AttributeType type, string methodName, object[] messages)
        {
            Dictionary<string, List<MethodInfo>> selectedAttributeType;
            TryGetValue(type, out selectedAttributeType);
            if (selectedAttributeType == null) return null;

            List<MethodInfo> allOverloadedMethods;
            selectedAttributeType.TryGetValue(methodName, out allOverloadedMethods);
            if (allOverloadedMethods == null) return null;
            if (allOverloadedMethods.Count == 1)
                return (ArgsEqual(allOverloadedMethods[0], messages)) ? allOverloadedMethods[0] : null;
            // this is mostly the case unless the method is overloaded

            foreach (var method in selectedAttributeType[methodName])
                if (ArgsEqual(method, messages)) return method;

            return null;
        }

        internal MethodInfo GetMethod(AttributeType type)
        {
            Dictionary<string, List<MethodInfo>> selectedAttributeType;
            TryGetValue(type, out selectedAttributeType);
            return selectedAttributeType == null ? null : selectedAttributeType.Values.First()[0];
        }

        private static bool ArgsEqual(MethodInfo method, object[] messages)
        {
            if (method.GetParameters().Length == 0 && (messages == null || messages.Length == 0)) return true;
            if (method.GetParameters().Length != messages.Length) return false;

            for (int i = 0; i < messages.Length; i++)
                if (method.GetParameters()[i].ParameterType != messages[i].GetType()) return false;

            return true;
        }


    }
}
