using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class JsonTable : MonoBehaviour
{
    [Header("JSON")]
    public string jsonFile = "JsonChallenge.json";
    public string titleKey = "Title";
    public string columnsKey = "ColumnHeaders";
    public string dataKey = "Data";
    public int fileWaitTicks = 30;
    [Header("Prefab")]
    public TMP_Text cellPrefab;
    public static TMP_Text cellPrefabInstance;
    public float cellHeight = 50f;
    [Header("Containers")]
    public RectTransform groupParent;
    public Dictionary<string, Column> columns = new Dictionary<string, Column>();

    int _tick = 0;
    DateTime _lastMod;

    public DateTime ModTime => File.GetLastWriteTime(FileManager.ToLocalPath(jsonFile));

    public static TMP_Text CreateInstance(RectTransform container) => Instantiate(cellPrefabInstance, container);
    public static float CellHeight { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        cellPrefabInstance = cellPrefab;
        CellHeight = cellHeight;
        RefreshTable();
        _lastMod = ModTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (++_tick <= fileWaitTicks)
            return;
        DateTime modTime = ModTime;
        if (modTime != _lastMod)
        {
            RefreshTable();
        }
        _lastMod = modTime;
        _tick = 0;
    }

    public void RefreshTable()
    {
        string strSchema = FileManager.ReadAllText(jsonFile);
        JsonSchema schema = JsonSchema.Parse(strSchema);
        JObject json = JObject.Parse(strSchema);

        if (!json.IsValid(schema))
            return;

        IEnumerable<string> headers = json[columnsKey].Value<JArray>().Values<string>();
        foreach (string strColumn in headers) // toda la magia del sur.
        {
            if (!columns.ContainsKey(strColumn))
                CreateColumn(strColumn);
        }

        JArray data = json[dataKey].Value<JArray>();
        int rowCount = 0;

        foreach (Column column in columns.Values)
            column.Disable();

        foreach (JObject element in data)
        {
            rowCount++;

            foreach (JToken token in element.Children())
            {
                Column column;
                if (token.GetParentKey(out string key) && columns.ContainsKey(key))
                    column = columns[key];
                else
                    column = CreateColumn(key);

                column.Enable();
                column.SetCell(rowCount, token.First.ToString());
            }
        }
    }

    public Column CreateColumn(string title = "")
    {
        Column column;
        if (columns.ContainsKey(title))
            return columns[title];

        columns.Add(title, column = new Column(groupParent, title));
        return column;
    }
}
