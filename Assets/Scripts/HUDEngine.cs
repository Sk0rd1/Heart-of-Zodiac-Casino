using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDEngine : MonoBehaviour
{
    private GameProgress.SaveData saveData = new GameProgress.SaveData();

    [SerializeField]
    private TextMeshProUGUI moneyText;

    [SerializeField]
    private TextMeshProUGUI keyLvlText;
    [SerializeField]
    private List<GameObject> keyLvlSkillPoints;

    [SerializeField]
    private TextMeshProUGUI keySpeedText;
    [SerializeField]
    private List<GameObject> keySpeedSkillPoints;

    [SerializeField]
    private TextMeshProUGUI reloadTimeText;
    [SerializeField]
    private List<GameObject> reloadTimeSkillPoints;

    [SerializeField]
    private TextMeshProUGUI roundDurationText;
    [SerializeField]
    private List<GameObject> roundDurationSkillPoints;

    [SerializeField]
    private TextMeshProUGUI textMinusLvl;

    [SerializeField]
    private TextMeshProUGUI textAddTime;

    [SerializeField]
    private Button bKeyLvl;
    [SerializeField]
    private Button bKeySpeed;
    [SerializeField]
    private Button bKeyReload;
    [SerializeField]
    private Button bRoundDuration;

    [SerializeField]
    private GameObject noCoin;

    [SerializeField]
    private GameObject winGame;
    [SerializeField]
    private TextMeshProUGUI gameMinusLvl;
    [SerializeField]
    private TextMeshProUGUI gameAddTime;

    private void Awake()
    {
        GameEngine.InitializeObjects();
    }

    public void PressStartGame()
    {
        winGame.SetActive(false);

        GameEngine.StartGame();

        saveData = GameProgress.GetSave();

        gameMinusLvl.text = saveData.numMinusLvl.ToString();
        gameAddTime.text = saveData.numAddTime.ToString();
    }

    public void GamePressMinusLvl()
    {
        if(saveData.numMinusLvl > 0)
        {
            saveData.numMinusLvl--;
            GameProgress.SaveGame(saveData);
            gameMinusLvl.text = saveData.numMinusLvl.ToString();
            LevelGenerator.MinusLvl();
        }
    }

    public void GamePressAddTime()
    {
        if (saveData.numAddTime > 0)
        {
            saveData.numAddTime--;
            GameProgress.SaveGame(saveData);
            gameAddTime.text = saveData.numAddTime.ToString();
            GameEngine.AddTime();
        }
    }
    public void Upgrade()
    {
        GameEngine.Upgrade();
        InitializeUpdate();
    }

    public void Slots()
    {
        GameEngine.Slots();
    }

    public void MainMenu()
    {
        try
        {
            if (GameObject.Find("SlotsObjects").active)
            {
                if (GameObject.Find("SlotsObjects").GetComponent<SlotsGenerator>().isReadyToGenerate)
                {
                    GameObject.Find("SlotsObjects").GetComponent<SlotsGenerator>().Clear();
                    GameEngine.MainMenu();
                }
            }
            else
                GameEngine.MainMenu();
        } 
        catch
        {
            GameEngine.MainMenu();
        }
    }

    public void ResetAll()
    {
        GameEngine.ResetAll();
        bKeyLvl.gameObject.SetActive(true);
        bKeySpeed.gameObject.SetActive(true);
        bKeyReload.gameObject.SetActive(true);
        bRoundDuration.gameObject.SetActive(true);
        InitializeUpdate();
    }

    public void InitializeUpdate()
    {
        saveData = GameProgress.LoadGame();

        moneyText.text = saveData.money.ToString();

        for (int i = 0; i < keyLvlSkillPoints.Count; i++) 
        {
            if(i < saveData.upKeyLvl)
            {
                keyLvlSkillPoints[i].SetActive(true);
            }
            else
            {
                keyLvlSkillPoints[i].SetActive(false);
            }
        }
        try
        {
            keyLvlText.text = CostUpgrade.keyLvlCost[saveData.upKeyLvl].ToString();
        }
        catch
        {
            keyLvlText.text = "MAX";
            bKeyLvl.gameObject.SetActive(false);
        }

        for (int i = 0; i < keySpeedSkillPoints.Count; i++)
        {
            if (i < saveData.upKeySpeed)
            {
                keySpeedSkillPoints[i].SetActive(true);
            }
            else
            {
                keySpeedSkillPoints[i].SetActive(false);
            }
        }

        try
        {
            keySpeedText.text = CostUpgrade.keySpeedCost[saveData.upKeySpeed].ToString();
        }
        catch
        {
            keySpeedText.text = "MAX";
            bKeySpeed.gameObject.SetActive(false);
        }

        for (int i = 0; i < reloadTimeSkillPoints.Count; i++)
        {
            if (i < saveData.upKeyReload)
            {
                reloadTimeSkillPoints[i].SetActive(true);
            }
            else
            {
                reloadTimeSkillPoints[i].SetActive(false);
            }
        }

        try
        {
            reloadTimeText.text = CostUpgrade.keyReloadCost[saveData.upKeyReload].ToString();
        }
        catch
        {
            reloadTimeText.text = "MAX";
            bKeyReload.gameObject.SetActive(false);
        }

        for (int i = 0; i < roundDurationSkillPoints.Count; i++)
        {
            if (i < saveData.upRoundDuration)
            {
                roundDurationSkillPoints[i].SetActive(true);
            }
            else
            {
                roundDurationSkillPoints[i].SetActive(false);
            }
        }

        try 
        {
            roundDurationText.text = CostUpgrade.roundDurationCost[saveData.upRoundDuration].ToString();
        }
        catch
        {
            roundDurationText.text = "MAX";
            bRoundDuration.gameObject.SetActive(false);
        }

        textMinusLvl.text = saveData.numMinusLvl.ToString();
        textAddTime.text = saveData.numAddTime.ToString();
    }

    public void UpKeyLvl()
    {
        if (CostUpgrade.keyLvlCost[saveData.upKeyLvl] <= saveData.money)
        {
            saveData.money -= CostUpgrade.keyLvlCost[saveData.upKeyLvl];
            saveData.upKeyLvl++;
            GameProgress.SaveGame(saveData);
            InitializeUpdate();
        }
        else
        {
            noCoin.SetActive(true);
        }
    }

    public void UpKeySpeed()
    {
        if (CostUpgrade.keySpeedCost[saveData.upKeySpeed] <= saveData.money)
        {
            saveData.money -= CostUpgrade.keyLvlCost[saveData.upKeySpeed];
            saveData.upKeySpeed++;
            GameProgress.SaveGame(saveData);
            InitializeUpdate();
        }
        else
        {
            noCoin.SetActive(true);
        }
    }

    public void UpKeyReload()
    {
        if (CostUpgrade.keyReloadCost[saveData.upKeyReload] <= saveData.money)
        {
            saveData.money -= CostUpgrade.keyLvlCost[saveData.upKeyReload];
            saveData.upKeyReload++;
            GameProgress.SaveGame(saveData);
            InitializeUpdate();
        }
        else
        {
            noCoin.SetActive(true);
        }
    }

    public void BuyMinusLvl()
    {
        if (200 <= saveData.money)
        {
            saveData.money -= 200;
            saveData.numMinusLvl++;
            GameProgress.SaveGame(saveData);
            InitializeUpdate();
        }
        else
        {
            noCoin.SetActive(true);
        }
    }

    public void BuyAddTime()
    {
        if (250 <= saveData.money)
        {
            saveData.money -= 250;
            saveData.numAddTime++;
            GameProgress.SaveGame(saveData);
            InitializeUpdate();
        }
        else
        {
            noCoin.SetActive(true);
        }
    }

    public void UpRoundDuration()
    {
        if (CostUpgrade.roundDurationCost[saveData.upRoundDuration] <= saveData.money)
        {
            saveData.money -= CostUpgrade.keyLvlCost[saveData.upRoundDuration];
            saveData.upRoundDuration++;
            GameProgress.SaveGame(saveData);
            InitializeUpdate();
        }
        else
        {
            noCoin.SetActive(true);
        }
    }

    public void SpinSlot()
    {
        GameObject.Find("SlotsObjects").GetComponent<SlotsGenerator>().GenerateSlots();
    }
}
