namespace SwiftAPI.API.ServerVariables
{
    public class ServerVariable 
    {
        public string ID;
        public string Value;

        public ServerVariable(string id, string value)
        {
            ID = id;
            Value = value;
        }
    }
}
