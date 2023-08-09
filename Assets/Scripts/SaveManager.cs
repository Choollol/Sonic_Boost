using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string path;

    private SaveData data = new SaveData();
    private void Awake()
    {
        path = Application.persistentDataPath + "/save.json";
    }
    void Start()
    {
        Load();

        InvokeRepeating("Save", 60, 60);
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    private void Save()
    {
        data.level = GameManager.level;
        data.sfxVolume = VolumeManager.sfxVolume;
        data.bgmVolume = VolumeManager.bgmVolume;
        data.isGameCompleted = GameManager.isGameCompleted;

        WriteData();
    }
    private void Load()
    {
        ReadData();

        if (data.level == 0)
        {
            data.level = 1;
        }
        GameManager.level = data.level;
        GameManager.isGameCompleted = data.isGameCompleted;
        VolumeManager.Instance.SetVolumes(data.sfxVolume, data.bgmVolume);

        GameManager.Instance.LoadLevel();
    }
    private void WriteData()
    {
        string jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(path, jsonData);
    }

    private void ReadData()
    {
        if (File.Exists(path))
        {
            string contents = File.ReadAllText(path);

            data = JsonUtility.FromJson<SaveData>(contents);
        }
        else
        {
            VolumeManager.Instance.SetVolumes(0.5f, 0.5f);
            Save();
            ReadData();
        }
    }
}
