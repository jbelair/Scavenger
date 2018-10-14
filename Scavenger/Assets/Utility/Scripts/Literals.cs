using System.Collections.Generic;
using UnityEngine;

public static class Literals
{
    public static Dictionary<string, Dictionary<string, string>> literals;
    public static Dictionary<string, Dictionary<string, string>> iLiterals;// = new Dictionary<string, Dictionary<string, string>>();

    public static string defaultLanguage = "";

    [System.Serializable]
    public struct Language
    {
        public string name;
        public string displayName;
        public string path;
        public bool defaultLanguage;
    }

    public class LanguageLibrary
    {
        public string name;
        public JSONLibrary[] libraries;
        public JSONLibrary[] iLibraries;

        [System.Serializable]
        public class JSONLibrary
        {
            public LibraryKeyValuePair[] literals;

            public LibraryKeyValuePair[] Inverse()
            {
                LibraryKeyValuePair[] i = new LibraryKeyValuePair[literals.Length];

                for(int j = 0; j < i.Length; j++)
                {
                    i[j].key = literals[j].value;
                    i[j].value = literals[j].key;
                }

                return i;
            }
        }

        [System.Serializable]
        public struct LibraryKeyValuePair
        {
            public string key;
            public string value;
        }

        public LanguageLibrary(string name)
        {
            this.name = name;
            TextAsset[] literalsFile = Resources.LoadAll<TextAsset>("Language/" + name);
            libraries = new JSONLibrary[literalsFile.Length];
            iLibraries = new JSONLibrary[literalsFile.Length];
            int i = 0;
            foreach (TextAsset literal in literalsFile)
            {
                libraries[i] = JsonUtility.FromJson<JSONLibrary>(literal.ToString());
                iLibraries[i] = new JSONLibrary() { literals = libraries[i].Inverse() };
                i++;
            }
        }

        public Dictionary<string, string> Compress()
        {
            Dictionary<string, string> compressedLibrary = new Dictionary<string, string>();
            foreach(JSONLibrary lib in libraries)
            {
                if (lib.literals == null)
                    continue;

                foreach(LibraryKeyValuePair entry in lib.literals)
                {
                    compressedLibrary.Add(entry.key, entry.value);
                }
            }
            return compressedLibrary;
        }

        public Dictionary<string, string> CompressInverse()
        {
            Dictionary<string, string> compressedLibrary = new Dictionary<string, string>();
            foreach (JSONLibrary lib in iLibraries)
            {
                if (lib.literals == null)
                    continue;

                foreach (LibraryKeyValuePair entry in lib.literals)
                {
                    if (!compressedLibrary.ContainsKey(entry.key))
                        compressedLibrary.Add(entry.key, entry.value);
                }
            }
            return compressedLibrary;
        }
    }
}
