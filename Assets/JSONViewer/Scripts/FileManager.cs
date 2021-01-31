using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileManager
{

    public static string ReadAllText(string file) => File.ReadAllText(ToLocalPath(file));
    public static string ToLocalPath(string path) => Path.Combine(Application.streamingAssetsPath, path);
}
