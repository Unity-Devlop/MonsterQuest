using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityToolkit
{
    //TODO Json系统
    public static class JsonUtil
    {
        private static string streamingAssetsPath => Application.streamingAssetsPath;
        // private static JsonSerializerSettings _serializerSettings;
        public static TObj LoadJsonFromStreamingAssets<TObj>(string path)
        {
            string json = ReadFile($"{streamingAssetsPath}/{path}");
            // return JsonUtility.FromJson<TObj>(json);
            return JsonConvert.DeserializeObject<TObj>(json);
        }

        public static void SaveJsonToStreamingAssets<TObj>(string path, TObj obj)
        {
            // string json = JsonUtility.ToJson(obj);
            // format json
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            SaveFile($"{streamingAssetsPath}/{path}", json);
        }


        public static TObj LoadJson<TObj>(string path)
        {
            string json = ReadFile(path);
            return JsonUtility.FromJson<TObj>(json);
        }

        public static void SaveJson<TObj>(string path, TObj obj)
        {
            string json = JsonUtility.ToJson(obj);
            SaveFile(path, json);
        }

        public static void SaveFile(string path, string content)
        {
            if (File.Exists(path))
            {
                FileUtil.ReplaceContent(content, path);
            }
            else
            {
                FileUtil.Create(path);
                FileUtil.ReplaceContent(content, path);
            }
        }

        public static string ReadFile(string path)
        {
            return FileUtil.TryReadAllText(path, out var content) ? content : "";
        }
    }
}