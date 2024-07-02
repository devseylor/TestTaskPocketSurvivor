using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Character[] characters;
}

public static class SaveManager
{
    // TODO
    // Файл сохранений находится по адрису
    // C:\Users\[user]\AppData\LocalLow\DefaultCompany\GardenOfDreams_TestSaveData.dat
    
    private const string saveFileName = "SaveData.dat";
    public static SaveData LoadData()
    {
        SaveData saveData;
        if (File.Exists(Application.persistentDataPath + saveFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveFileName, FileMode.Open);
            saveData = (SaveData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Game data loaded!");
        }
        else
        {
            saveData = new SaveData
            {
                characters = new[]
                {
                    new Character
                    {
                        health = 100,
                        maxHealth = 100,
                        name = "Герой",

                        inventory = new[]
                        {
                            new ItemData {id = 4, count = 1,},
                            new ItemData {id = 2, count = 1,},
                            null, 
                            new ItemData {id = 1, count = 6,},
                            null,
                            new ItemData {id = 6, count = 50,},
                            new ItemData {id = 5, count = 1,},
                            new ItemData {id = 3, count = 1,},
                            null, null, null,
                            new ItemData {id = 7, count = 100,},
                            null, null, null, null, null, null,
                            null, null, null, null, null, null,
                            null, null, null, null, null, null,
                        }
                    },
                    new Character
                    {
                        health = 100,
                        maxHealth = 100,
                        name = "Враг",
                        inventory = new[]
                        {
                            new ItemData
                            {
                                id = 1,
                                count = 6,
                            }
                        }
                    }
                }

            };
            Debug.Log("Game data created!");
        }

        return saveData;
    }

    public static void SaveData(SaveData saveData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveFileName);
        bf.Serialize(file, saveData);
        file.Close();
        Debug.Log("Game data saved!");
    }

    public static void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + saveFileName))
        {
            File.Delete(Application.persistentDataPath + saveFileName);
            Debug.Log("Data reset complete!");
        }
        else
        {
            Debug.LogError("No save data to delete.");
        }
    }
}