using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class JsonTokenExtensions
{
    public static bool GetParentKey(this JToken token, out string key)
    {
        key = string.Empty;
        string[] sequence = token.Path.Split('.');
        if (sequence.Length == 0)
            return false;
        key = sequence[sequence.Length - 1];
        return true;
    }
}