using System;
using System.Collections.Generic;

namespace SwiftAPI.API.ServerVariables
{
    public static class ServerVariableManager
    {
        public readonly static Dictionary<string, ServerVariable> Vars = new Dictionary<string, ServerVariable>();

        public static bool TryGetVar(string id, out ServerVariable variable)
        {
            if (Vars.ContainsKey(id))
            {
                variable = Vars[id];

                return true;
            }
            else
            {
                variable = null;

                return false;
            }
        }

        public static void SetVar(string id, string value)
        {
            if (TryGetVar(id, out ServerVariable var))
                var.Value = value;
            else
                Vars.Add(id, new ServerVariable(id, value));
        }
    }
}
