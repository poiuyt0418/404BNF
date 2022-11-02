using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    static string saveFilePath = Application.persistentDataPath + "/save.json";
    public static GameData gameData = new GameData();

    public static void ReadFile()
    {
        if (File.Exists(saveFilePath))
        {
            string savedData = File.ReadAllText(saveFilePath);
            gameData = JsonUtility.FromJson<GameData>(savedData);
        }
    }

    public static void WriteFile()
    {
        string gameDataToJson = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFilePath, gameDataToJson);
    }
}

[System.Serializable]
public class GameData
{
    public List<Stars> starData = new List<Stars>();

    [System.Serializable]
    public class Stars
    {
        public int level;
        public int starCount;
    }

    public int StarsCount(int level)
    {
        foreach (Stars star in starData)
        {
            if (star.level == level)
            {
                return star.starCount;
            }
        }
        return 0;
    }

    public void AddStars(int level, int starAmount)
    {
        starData.Add(new Stars { level = level, starCount = starAmount});
    }
}
