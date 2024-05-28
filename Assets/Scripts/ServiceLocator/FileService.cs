using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class FileService : MonoBehaviour, IService
{
  

    public GameSetting gameSetting;

#if UNITY_EDITOR
    private string _prePath = Application.streamingAssetsPath;
#else
     private string _prePath = Application.persistentDataPath;
#endif

  

    private string PathComBine(string path)
    {
        return Path.Combine(_prePath, path);
    }

    public bool FileExists(string path)
    {
        string realPath = PathComBine(path);
        return File.Exists(realPath);
    }

    public string LoadFileToString(string path)
    {
        string realPath = PathComBine(path);
        Debug.Log($"load from {realPath}");
        if (!File.Exists(realPath))
        {
            using (File.Create(realPath))
            {
            }
        }

        var text = Task.Run(async () => { return await File.ReadAllTextAsync(realPath); }).GetAwaiter().GetResult();
        Debug.Log($" content:\n {text}");
        return text;
    }

    public async void WirteStringToFile(string path, string content)
    {
        string realPath = PathComBine(path);
        Debug.Log($"load from {realPath}");
        if (!File.Exists(realPath))
        {
            using (File.Create(realPath))
            {
            }
        }

        await File.WriteAllTextAsync(realPath, content);
    }
}