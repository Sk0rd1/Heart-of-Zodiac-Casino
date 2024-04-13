using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class GameEngine
{
    private static GameObject circleObjects;
    private static GameObject slotsObjects;
    private static GameObject key;
    private static GameObject mainMenu;
    private static GameObject upgrade;
    private static GameObject slots;
    private static GameObject gameMenu;
    private static GameObject noCoin;
    private static GameObject winGame;
    private static GameObject loseGame;

    private static TextMeshProUGUI textMinusLvl;
    private static TextMeshProUGUI textAddTime;

    public static void StartGame()
    {
        gameMenu.SetActive(true);
        upgrade.SetActive(false);
        mainMenu.SetActive(false);
        circleObjects.SetActive(true);
        winGame.SetActive(false);
        loseGame.SetActive(false);
        LevelGenerator.CreateLevel(GameProgress.LoadGame().upKeyLvl);
        GameProgress.SaveData saveData = GameProgress.GetSave();

        KeyMovement keyMovement = key.GetComponent<KeyMovement>();
        keyMovement.SetKeyParameters(saveData.upKeyLvl, saveData.upKeySpeed, saveData.upKeyReload, saveData.upRoundDuration);
        keyMovement.StartGame();

        GameObject.Find("HUD/Game/TapToStartText").SetActive(true);
    }

    public static void LoseGame()
    {
        LevelGenerator.ClearLevel();
        loseGame.SetActive(true);
    }

    public static void WinGame(int time)
    {
        LevelGenerator.ClearLevel();
        //circleObjects.SetActive(false);
        //mainMenu.SetActive(true);
        //gameMenu.SetActive(false);
        //upgrade.SetActive(false);

        GameProgress.SaveData saveGame = GameProgress.LoadGame();
        int price = 100 + 50 * saveGame.upKeyLvl + 5 * time;
        saveGame.money += price;
        saveGame.freeSpins++;

        winGame.SetActive(true);
        winGame.GetComponentInChildren<TextMeshProUGUI>().text = "You WIN " + price.ToString() + "\n and 1 Free Spin";
        GameProgress.SaveGame(saveGame);
    }

    public static void MainMenu()
    {
        LevelGenerator.ClearLevel();
        circleObjects.SetActive(false);
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        upgrade.SetActive(false);
        slotsObjects.SetActive(false);
        slots.SetActive(false);
    }

    public static void ResetAll()
    {
        GameProgress.SaveData saveData = new GameProgress.SaveData(15000);
        GameProgress.SaveGame(saveData);
    }

    public static void Upgrade()
    {
        circleObjects.SetActive(false);
        mainMenu.SetActive(false);
        gameMenu.SetActive(false);
        upgrade.SetActive(true);
        noCoin.SetActive(false);
        slotsObjects.SetActive(false);
    }

    public static void InitializeObjects()
    {
        circleObjects = GameObject.Find("CircleObjects");
        slotsObjects = GameObject.Find("SlotsObjects");
        key = GameObject.Find("CircleObjects/Key");
        mainMenu = GameObject.Find("HUD/MainMenu");
        upgrade = GameObject.Find("HUD/Upgrade");
        gameMenu = GameObject.Find("HUD/Game");
        slots = GameObject.Find("HUD/Slots");
        noCoin = GameObject.Find("HUD/Upgrade/NoCoin");
        winGame = GameObject.Find("HUD/Game/Win");
        loseGame = GameObject.Find("HUD/Game/Lose");

        circleObjects.SetActive(false);
        slotsObjects.SetActive(false);
        mainMenu.SetActive(true);
        upgrade.SetActive(false);
        gameMenu.SetActive(false);
        winGame.SetActive(false);
        slots.SetActive(false);
    }

    public static void AddTime()
    {
        key.GetComponent<KeyMovement>().PressAddTime();
    }

    public static void Slots()
    {
        circleObjects.SetActive(false);
        mainMenu.SetActive(false);
        upgrade.SetActive(false);
        gameMenu.SetActive(false);
        winGame.SetActive(false);
        slotsObjects.SetActive(true);
        slots.SetActive(true);

         GameObject.Find("SlotsObjects").GetComponent<SlotsGenerator>().GenerateSlots();
    }


}
