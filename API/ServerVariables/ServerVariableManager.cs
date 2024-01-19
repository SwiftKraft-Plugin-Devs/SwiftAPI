using System;
using System.Collections.Generic;

namespace SwiftAPI.API.ServerVariables
{
    public static class ServerVariableManager
    {
        public readonly static Dictionary<string, ServerVariable> Vars = new Dictionary<string, ServerVariable>();

        public static bool TryGetVar<T>(string id, out ServerVariable<T> variable)
        {
            if (Vars.ContainsKey(id) && Vars[id] is ServerVariable<T> v)
            {
                variable = v;

                return true;
            }
            else
            {
                variable = null;

                return false;
            }
        }

        public static void SetVar<T>(string id, T value)
        {
            if (Vars.ContainsKey(id) && Vars[id] is ServerVariable<T> v)
                v.Value = value;
            else if (!Vars.ContainsKey(id))
                Vars.Add(id, new ServerVariable<T>() { Value = value });
            else
                throw new TypeMismatchException(typeof(T));
        }

        public class TypeMismatchException : Exception
        {
            public Type Type;

            public TypeMismatchException(Type type)
            {
                Type = type;
            }
        }
    }
}
