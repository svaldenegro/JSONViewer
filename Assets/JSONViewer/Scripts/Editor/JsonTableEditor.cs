using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JsonTable))]
public class JsonTableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Test Json"))
        {
            JsonTable jsonTbl = (JsonTable)target;
            JObject json = JObject.Parse(FileManager.ReadAllText(jsonTbl.jsonFile));
            Debug.Log(json[jsonTbl.titleKey]);
            Debug.Log(json[jsonTbl.columnsKey].Value<JArray>().Values<string>());
            IEnumerable<string> columns = json[jsonTbl.columnsKey].Value<JArray>().Values<string>();
            foreach (string column in columns)
                Debug.Log(column);

            JArray data = json[jsonTbl.dataKey].Value<JArray>();
            foreach (JObject element in data)
            {
                foreach (JToken token in element.Children())
                {
                    Debug.Log(token.First);
                }
            }
        }
    }
}
