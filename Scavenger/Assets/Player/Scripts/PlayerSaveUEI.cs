using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveUEI : MonoBehaviour
{
    public List<PlayerSave.SerializableData> saves = new List<PlayerSave.SerializableData>();
    public RectTransform optionalContinue;

    // Use this for initialization
    void Awake()
    {
        List<PlayerSave> saved = PlayerSave.QuerySaves();
        
        if (optionalContinue)
            optionalContinue.gameObject.SetActive(!(saved.Count == 0 && optionalContinue));
    }

    private void Update()
    {
        saves.Clear();
        foreach (PlayerSave s in PlayerSave.saves)
        {
            saves.Add(s.GetData());
        }
    }

    public void Save()
    {
        PlayerSave.Save();
    }

    public void New()
    {
        PlayerSave.saves[0] = new PlayerSave("save");
    }
}
