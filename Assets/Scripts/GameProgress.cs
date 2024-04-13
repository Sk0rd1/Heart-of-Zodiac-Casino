using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Profiling;

public static class GameProgress
{
    private static SaveData saveData= new SaveData(); 
    public static SaveData LoadGame()
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        if (!File.Exists(Application.persistentDataPath + "/save.bin"))
        {
            //тут повинно бути 0
            SaveData data = new SaveData();

            BinaryFormatter f = new BinaryFormatter();

            using (FileStream stream = File.Create(Application.persistentDataPath + "/save.bin"))
            {
                f.Serialize(stream, data);
            }
        }

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = File.OpenRead(Application.persistentDataPath + "/save.bin"))
        {
            saveData = (SaveData)formatter.Deserialize(stream);
        }

        return saveData;
    }

    public static SaveData GetSave()
    {
        return saveData;
    }

    public static void SaveGame(SaveData saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = File.Create(Application.persistentDataPath + "/save.bin"))
        {
            formatter.Serialize(stream, saveData);
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public int money;
        public int freeSpins;

        public short upKeyLvl;
        public short upKeySpeed;
        public short upKeyReload;
        public short upRoundDuration;

        public int numMinusLvl;
        public int numAddTime;

        public bool isMusicPlay;

        public SaveData(int money = 0, int freeSpins = 0, short upKeyLvl = 0, short upKeySpeed = 0, short upKeyReload = 0, short upRoundDuration = 0, int numMinusLvl = 0, int numAddTime = 0, bool isMusicPlay = false)
        {
            this.money = money;
            this.upKeyLvl = upKeyLvl;
            this.upKeySpeed = upKeySpeed;
            this.upKeyReload = upKeyReload;
            this.upRoundDuration = upRoundDuration;
            this.numMinusLvl = numMinusLvl;
            this.numAddTime = numAddTime;
            this.isMusicPlay = isMusicPlay;
            this.freeSpins = freeSpins;
        }
    }
}
