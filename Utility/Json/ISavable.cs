namespace SwiftAPI.Utility.Json
{
    public interface ISavable : IJsonSerializable
    {
        public string GetFilePath();
        public string GetName();
    }
}
