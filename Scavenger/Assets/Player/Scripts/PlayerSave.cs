using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerSave
{
    public static List<PlayerSave> saves;

    public static PlayerSave Active()
    {
        if (saves.Count == 0)
        {
            saves.Add(new PlayerSave("save"));

            string unlockedShips = "";
            foreach(ShipDefinition ships in Ships.definitions.Values)
            {
                if (ships.oneIn == 1)
                    unlockedShips += ships.name + " ";
            }

            saves[0].Add("unlocked ships", "StringList", unlockedShips);
        }

        return saves[0];
    }

    public static void Save()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        foreach (PlayerSave save in saves)
        {
            if (save.name != "")
            {
                string dataPath = Path.Combine(folderPath, save.name + ".json");

                using (StreamWriter writer = File.CreateText(dataPath))
                {
                    writer.Write(JsonUtility.ToJson(save.GetData()));
                }
            }
        }
    }

    public static List<PlayerSave> QuerySaves()
    {
        List<PlayerSave> saves = new List<PlayerSave>();
        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
        if (Directory.Exists(folderPath))
        {
            string[] paths = Directory.GetFiles(folderPath, "*.json");
            foreach (string path in paths)
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    saves.Add(new PlayerSave(JsonUtility.FromJson<SerializableData>(reader.ReadToEnd())));
                }
            }
        }

        return saves;
    }

    static PlayerSave()
    {
        saves = QuerySaves();
    }

    [Serializable]
    public class Data
    {
        public string name;
        public string type;
        public string value;
    }

    [Serializable]
    public class SerializableData
    {
        public string name;
        public bool isLatest;
        public List<Data> data = new List<Data>();
    }

    public Dictionary<string, Data> data = new Dictionary<string, Data>();
    public string name;

    public PlayerSave(string name)
    {
        this.name = name;
    }

    public PlayerSave(SerializableData serialized)
    {
        name = serialized.name;
        foreach (Data d in serialized.data)
        {
            data.Add(d.name, d);
        }
    }

    public SerializableData GetData()
    {
        SerializableData d = new SerializableData();
        d.name = name;
        foreach (KeyValuePair<string, Data> pair in data)
        {
            d.data.Add(pair.Value);
        }
        return d;
    }

    public void Add(Data d)
    {
        if (data.ContainsKey(d.name))
            data[d.name] = d;
        else
            data.Add(d.name, d);
    }

    public void Add(string name, string value)
    {
        if (data.ContainsKey(name))
            data[name].value = value;
        else
            data.Add(name, new Data() { name = name, type = StringHelper.PredictType(value), value = value });
    }

    public void Add(string name, string type, string value)
    {
        if (data.ContainsKey(name))
        {
            data[name].type = type;
            data[name].value = value;
        }
        else
            data.Add(name, new Data() { name = name, type = type, value = value });
    }

    public Data Get(string name)
    {
        if (!data.ContainsKey(name))
            data.Add(name, new Data() { name = name, type = StringHelper.PredictType(""), value = "" });

        return data[name];
    }
}
