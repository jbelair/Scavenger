﻿using System.Collections.Generic;
using UnityEngine;

public class LiteralsLoader : MonoBehaviour
{
    public void Awake()
    {
        Literals.literals = new Dictionary<string, Dictionary<string, string>>();
        Literals.iLiterals = new Dictionary<string, Dictionary<string, string>>();

        string firstLanguage = "";

        TextAsset[] languagesAssets = Resources.LoadAll<TextAsset>("Language/Languages");
        Literals.LanguageLibrary[] libraries = new Literals.LanguageLibrary[languagesAssets.Length];
        int i = 0;
        foreach (TextAsset language in languagesAssets)
        {
            Literals.Language lang = JsonUtility.FromJson<Literals.Language>(language.ToString());
            libraries[i] = new Literals.LanguageLibrary(lang.path);
            Literals.literals.Add(lang.name, libraries[i].Compress());
            Literals.iLiterals.Add(lang.name, libraries[i].CompressInverse());

            if (lang.defaultLanguage)
                Literals.defaultLanguage = Environment.language = lang.name;

            if (firstLanguage == "")
                firstLanguage = lang.name;

            i++;
        }

        if (Literals.defaultLanguage == "")
            Literals.defaultLanguage = Environment.language = firstLanguage;
    }
}