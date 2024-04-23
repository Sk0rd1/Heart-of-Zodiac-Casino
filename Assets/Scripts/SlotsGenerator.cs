using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameProgress;

public class SlotsGenerator : MonoBehaviour
{
    private Sprite[] cellSprites = new Sprite[8];
    private int[][] cV = new int[3][];
    private List<GameObject> cellGO = new List<GameObject>();
    private GameObject cellImagePrefab;

    [SerializeField]
    private GameObject[] linesImage = new GameObject[9];
    private int[] totalResults = new int[9];
    private int[] totalFreeSpins = new int[9];
    private GameObject[][] frames = new GameObject[3][];
    private List<GameObject>[] currentFrames = new List<GameObject>[9];
    private List<int> winLines = new List<int>();
    private GameProgress.SaveData saveData;

    private GameObject[][] fakeCells = new GameObject[5][];
    //private int countFakeCellsRow = 0;

    private bool isFreeSpin = false;
    private int totalResult = 0;
    private int countFreeSpins = 0;
    private int countLine = 0;
    public bool isReadyToGenerate { get; private set; } = true;

    [SerializeField]
    private GameObject textTotalWin;
    [SerializeField]
    private TextMeshProUGUI money;
    [SerializeField]
    private TextMeshProUGUI bet;
    [SerializeField]
    private GameObject noCoin;
    [SerializeField]
    private TextMeshProUGUI textFreeSpin;
    [SerializeField]
    private TextMeshProUGUI textPlusFreeSpin;
    private int betValue = 1;
    private float animationTime = 1.5f;

    private void GenerateFakeSlots()
    {
        for (int i = 0; i < 5; i++)
        {
            for(int j = 0; j < fakeCells[i].Length; j++)
            {
                GameObject go = GameObject.Instantiate(cellImagePrefab);

                int ranValue = Random.Range(0, 8);

                go.GetComponent<SpriteRenderer>().sprite = cellSprites[ranValue];
                go.GetComponent<MoveCell>().MoveToBot();

                go.transform.position = new Vector3(i - 2, j + 2, 0);
            }

            int verticalPos = fakeCells[i].Length + 2;

            for (int j = 0; j < 3; j++)
            {
                GameObject go = GameObject.Instantiate(cellImagePrefab);

                int ranValue = Random.Range(0, 8);

                cV[j][i] = ranValue;

                go.GetComponent<SpriteRenderer>().sprite = cellSprites[ranValue];
                MoveCell moveCell = go.GetComponent<MoveCell>();

                if(i == 4 && j == 0)
                    moveCell.SetYPosition(1 - j, true);
                else
                    moveCell.SetYPosition(1 - j);

                go.transform.position = new Vector3(i - 2, 1 - j + verticalPos, 0);

                cellGO.Add(go);
            }
        }
    }

    public void Clear()
    {
        foreach (GameObject go in cellGO)
            Object.DestroyImmediate(go);
        cellGO.Clear();
    }

    public void BetPlus()
    {
        betValue += 1;
        bet.text = (betValue * 100).ToString();
    }

    public void BetMinus()
    {
        if (betValue > 1)
            betValue -= 1;

        bet.text = (betValue * 100).ToString();
    }

    private bool CheckMoney()
    {
        if (saveData.money >= betValue * 100)
        {
            saveData.money -= betValue * 100;
            money.text = saveData.money.ToString();
            return true;
        }
        else
        {
            noCoin.SetActive(true);

            betValue = saveData.money / 100;

            if (betValue > 0)
            {
                bet.text = (betValue * 100).ToString();
            }
            else
            {
                betValue = 1;
                bet.text = (betValue * 100).ToString();
            }

            return false;
        }
    }

    public void FreeSpin()
    {
        if(saveData.freeSpins > 0)
        {
            isFreeSpin = false;
            saveData.freeSpins -= 1;
            textFreeSpin.text = saveData.freeSpins.ToString();
            GenerateSlots();
        }
    }

    public void GenerateFirstSlots()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject go = GameObject.Instantiate(cellImagePrefab);

                int ranValue = Random.Range(0, 8);

                cV[j][i] = ranValue;

                go.GetComponent<SpriteRenderer>().sprite = cellSprites[ranValue];
                MoveCell moveCell = go.GetComponent<MoveCell>();

                go.transform.position = new Vector3(i - 2, 1 - j, 0);

                cellGO.Add(go);
            }
        }
    }

    public void GenerateSlots()
    {
        if (!isReadyToGenerate)
            return;

        if (!CheckMoney())
            return;

        //StopShowLine();
        textTotalWin.gameObject.SetActive(false);

        isReadyToGenerate = false;
        foreach (GameObject go in cellGO)
            Object.Destroy(go);

        foreach (GameObject go in linesImage)
            go.SetActive(false);

        cellGO.Clear();

        GenerateFakeSlots();

        /*for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                GameObject go = GameObject.Instantiate(cellImagePrefab);

                int ranValue = Random.Range(0, 8);

                cV[j][i] = ranValue;

                //string imagePath = "cell_" + ranValue.ToString();

                go.GetComponent<SpriteRenderer>().sprite = cellSprites[ranValue];

                go.transform.position = new Vector3(i - 2, 2 - j, 0);

                cellGO.Add(go);
            }
        }*/

        //TotalResult();
    }

    private void OnEnable()
    {
        saveData = GameProgress.LoadGame();
        money.text = saveData.money.ToString();
        textFreeSpin.text = saveData.freeSpins.ToString();
        textPlusFreeSpin.gameObject.SetActive(false);

        noCoin.SetActive(false);

        for (int i = 0; i < cV.Length; i++)
        {
            cV[i] = new int[5];
        }

        for (int i = 0; i < cellSprites.Length; i++) 
        {
            cellSprites[i] = Resources.Load<Sprite>("Slots/Cells/cell_" + i.ToString());
        }

        cellImagePrefab = Resources.Load<GameObject>("Slots/cellImage");

        for (int i = 0; i < linesImage.Length; i++)
        {
            linesImage[i].SetActive(false);
        }

        for (int i = 0; i < frames.Length; i++)
            frames[i] = new GameObject[5];

        for (int i = 0; i < frames.Length; i++)
        {
            for (int j = 0; j < frames[i].Length; j++) 
            {
                frames[i][j] = GameObject.Find("SlotsObjects/Frames/frame"+ i + j);
                frames[i][j].SetActive(false);
            }
        }

        for (int i = 0; i < currentFrames.Length; i++)
            currentFrames[i] = new List<GameObject>();

        for (int i = 0; i < 5; i++)
            fakeCells[i] = new GameObject[12 + i * 3];
    }

    public void TotalResult()
    {
        totalResult = 0;
        countFreeSpins = 0;

        for (int i = 0; i < totalFreeSpins.Length; i++)
            totalFreeSpins[i] = 0;

        CheckOneLine(CountResult(0, new int[] { cV[0][0], cV[0][1], cV[0][2], cV[0][3], cV[0][4] }, new int[][] { new int[]{0, 0}, new int[] {0, 1}, new int[] {0, 2}, new int[] {0, 3}, new int[] {0, 4}}));
        CheckOneLine(CountResult(1, new int[] { cV[1][0], cV[1][1], cV[1][2], cV[1][3], cV[1][4] }, new int[][] { new int[]{1, 0}, new int[] {1, 1}, new int[] {1, 2}, new int[] {1, 3}, new int[] {1, 4}}));
        CheckOneLine(CountResult(2, new int[] { cV[2][0], cV[2][1], cV[2][2], cV[2][3], cV[2][4] }, new int[][] { new int[]{2, 0}, new int[] {2, 1}, new int[] {2, 2}, new int[] {2, 3}, new int[] {2, 4}}));

        CheckOneLine(CountResult(3, new int[] { cV[0][0], cV[1][1], cV[2][2], cV[1][3], cV[0][4] }, new int[][] { new int[]{0, 0}, new int[] {1, 1}, new int[] {2, 2}, new int[] {1, 3}, new int[] {0, 4}}));
        CheckOneLine(CountResult(4, new int[] { cV[2][0], cV[1][1], cV[0][2], cV[1][3], cV[2][4] }, new int[][] { new int[]{2, 0}, new int[] {1, 1}, new int[] {0, 2}, new int[] {1, 3}, new int[] {2, 4}}));

        CheckOneLine(CountResult(5, new int[] { cV[2][0], cV[2][1], cV[1][2], cV[2][3], cV[2][4] }, new int[][] { new int[]{2, 0}, new int[] {2, 1}, new int[] {1, 2}, new int[] {2, 3}, new int[] {2, 4}}));
        CheckOneLine(CountResult(6, new int[] { cV[0][0], cV[0][1], cV[1][2], cV[0][3], cV[0][4] }, new int[][] { new int[]{0, 0}, new int[] {0, 1}, new int[] {1, 2}, new int[] {0, 3}, new int[] {0, 4}}));

        CheckOneLine(CountResult(7, new int[] { cV[1][0], cV[0][1], cV[0][2], cV[0][3], cV[1][4] }, new int[][] { new int[]{1, 0}, new int[] {0, 1}, new int[] {0, 2}, new int[] {0, 3}, new int[] {1, 4}}));
        CheckOneLine(CountResult(8, new int[] { cV[1][0], cV[2][1], cV[2][2], cV[2][3], cV[1][4] }, new int[][] { new int[]{1, 0}, new int[] {2, 1}, new int[] {2, 2}, new int[] {2, 3}, new int[] {1, 4}}));
    }

    private void CheckOneLine(int currentResult)
    {
        if(currentResult != 0)
        {
            totalResult += currentResult;
        }

        totalResults[countLine] = currentResult;
        winLines.Add(currentResult);
        countLine++;

        if (countLine == 9)
        {
            countLine = 0;
            StartCoroutine(ShowWinLines());
            StartCoroutine(ShowWin());
        }
    }

    /*private void StopShowLine()
    {
        StopCoroutine(ShowWinLines());

        foreach(var lines in linesImage)
            lines.SetActive(false);

        foreach (List<GameObject> frames in currentFrames)
            foreach (GameObject go in frames)
                go.SetActive(false);

        winLines.Clear();

        foreach (List<GameObject> go in currentFrames)
            go.Clear();
    }*/

    private IEnumerator ShowWin()
    {
        if (totalResult > 0)
            textTotalWin.gameObject.SetActive(true);
        TextMeshProUGUI text = textTotalWin.GetComponentInChildren<TextMeshProUGUI>(); 

        int textCount = 0;

        for(int i = 0; i < totalResults.Length; i++)
        {
            if(totalResults[i] != 0)
            {
                int winCount = totalResults[i];
                float currentCount = 0;
                for(float j = 0; j <= animationTime; j += Time.deltaTime)
                {
                    currentCount = j * winCount / animationTime;
                    text.text = ((int)(textCount + currentCount)).ToString();
                    yield return new WaitForEndOfFrame();
                }
                textCount += winCount;
                text.text = textCount.ToString();
            }
        }
    }

    private IEnumerator ShowWinLines()
    {
        for (int i = 0; i < winLines.Count; i++)
        {
            if (winLines[i] != 0)
            {
                linesImage[i].SetActive(true);
                foreach (GameObject go in currentFrames[i])
                    go.SetActive(true);
                if(totalFreeSpins[i] != 0)
                {
                    textPlusFreeSpin.gameObject.SetActive(true);
                    countFreeSpins += totalFreeSpins[i];
                    textPlusFreeSpin.text =  "+" + countFreeSpins.ToString() + " FS!";
                }

                yield return new WaitForSeconds(animationTime);

                linesImage[i].SetActive(false);
                foreach (GameObject go in currentFrames[i])
                    go.SetActive(false);
            }
        }

        winLines.Clear();

        foreach (List<GameObject> go in currentFrames)
            go.Clear();

        saveData.money += totalResult;
        saveData.freeSpins += countFreeSpins;
        money.text = saveData.money.ToString();
        textFreeSpin.text = saveData.freeSpins.ToString();
        GameProgress.SaveGame(saveData);

        textPlusFreeSpin.gameObject.SetActive(false);
        isReadyToGenerate = true;
    }
    private int CountResult(int num, int[] cv, int[][] pos)
    {
        bool isUnique = true;
        int uniqueValue = -1;

        int numClover = 0;
        int numFroot = 0;
        for (int i = 0; i < cv.Length;i++) 
        {
            if(isUnique)
            {
                if (cv[i] == 4)
                {
                    numClover++;
                }
                else
                {
                    numFroot++;
                    uniqueValue = cv[i];
                    isUnique = false;
                    continue;
                }
            }
            else
            {
                if (cv[i] == 4)
                {
                    numClover++;
                }
                else
                {
                    if (cv[i] == uniqueValue)
                    {
                        numFroot++;
                    }
                    else
                    {
                        if(i < 3)
                        {
                            return 0;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if(cv[i] == 4 || cv[i] == uniqueValue)
            {
                currentFrames[num].Add(frames[pos[i][0]][pos[i][1]]);
            }
        }

        int temporaryBetValue = 0;
        if (isFreeSpin)
        {
            temporaryBetValue = 1;
            betValue = 1;
        }

        int totalResult = 0;

        if (uniqueValue < 5)
        {
            if (numFroot + numClover == 5)
            {
                totalResult = 40 * betValue;
            }
            else if (numFroot + numClover == 4)
            {
                totalResult = 25 * betValue;
            }
            else if (numFroot + numClover == 3)
            {
                totalResult = 15 * betValue;
            }
        }
        else if (uniqueValue == 5)
        {
            if (numFroot + numClover == 5)
            {
                totalResult = 100 * betValue;
            }
            else if (numFroot + numClover == 4)
            {
                totalResult = 60 * betValue;
            }
            else if (numFroot + numClover == 3)
            {
                totalResult = 45 * betValue;
            }
        }
        else if (uniqueValue == 6)
        {
            if (numFroot + numClover == 5)
            {
                totalResult = 150 * betValue;
            }
            else if (numFroot + numClover == 4)
            {
                totalResult = 100 * betValue;
            }
            else if (numFroot + numClover == 3)
            {
                totalResult = 50 * betValue;
            }
        }
        else if (uniqueValue == 7)
        {
            if (numFroot + numClover == 5)
            {
                totalResult = 100 * betValue;
                totalFreeSpins[num] = 3; 
            }
            else if (numFroot + numClover == 4)
            {
                totalResult = 100 * betValue;
                totalFreeSpins[num] = 2;
            }
            else if (numFroot + numClover == 3)
            {
                totalResult = 100 * betValue;
                totalFreeSpins[num] = 1;
            }
        }

        if (isFreeSpin)
        {
            isFreeSpin = false;
            betValue = temporaryBetValue;
        }

        return totalResult;
    }
}
