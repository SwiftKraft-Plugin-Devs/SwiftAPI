using System.IO;
using Utf8Json;

namespace SwiftAPI.Utility.Json
{
    public static class JsonUtilities
    {
        public static void SaveAsJSON<T>(this T obj) where T : ISavable => obj.SaveAsFile(Path.Combine(obj.GetFilePath(), obj.GetName() + ".json"));

        public static void SaveAsFile<T>(this T obj, string path) where T : IJsonSerializable => File.WriteAllBytes(path, JSONSerialize(obj));

        public static byte[] JSONSerialize<T>(this T obj) where T : IJsonSerializable => JsonSerializer.Serialize(obj);

        public static T JSONConvert<T>(string path) where T : IJsonSerializable => JsonSerializer.Deserialize<T>(File.ReadAllText(path));

        public static bool TryJSONConvert<T>(string path, out T output) where T : IJsonSerializable
        {
            output = JSONConvert<T>(path);
            return output != null;
        }
    }
}
