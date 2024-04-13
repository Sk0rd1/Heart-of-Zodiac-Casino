using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsGenerator : MonoBehaviour
{
    private Sprite[] cellSprites = new Sprite[8];
    private int[][] cV = new int[3][];
    private List<GameObject> cellGO = new List<GameObject>();
    private GameObject cellImagePrefab;

    [SerializeField]
    private GameObject[] linesImage = new GameObject[9];
    private GameObject[][] frames = new GameObject[3][];
    private List<GameObject>[] currentFrames = new List<GameObject>[9];
    private List<int> winLines = new List<int>();

    private GameObject[][] fakeCells = new GameObject[5][];
    //private int countFakeCellsRow = 0;

    private int totalResult = 0;
    private int countLine = 0;
    private bool isReadyToGenerate = true;

    private void GenerateFakeSlots()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < fakeCells[i].Length; j++)
            {
                GameObject go = GameObject.Instantiate(cellImagePrefab);

                int ranValue = Random.Range(0, 8);

                go.GetComponent<SpriteRenderer>().sprite = cellSprites[ranValue];

                go.transform.position = new Vector3(i - 2, j + 3, 0);
                //fakeCells[i][j] = go;
            }
        }
    }

    public void GenerateSlots()
    {
        if (!isReadyToGenerate)
            return;

        isReadyToGenerate = false;
        foreach (GameObject go in cellGO)
            Object.Destroy(go);

        foreach (GameObject go in linesImage)
            go.SetActive(false);

        cellGO.Clear();

        for(int i = 0; i < 5; i++)
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
        }

        TotalResult();
    }

    private void OnEnable()
    {
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

    private void TotalResult()
    {
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
            totalResult += 0;
        }

        winLines.Add(currentResult);
        countLine++;

        if (countLine == 9)
        {
            countLine = 0;
            StartCoroutine(ShowWinLines());
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

                yield return new WaitForSeconds(2f);

                linesImage[i].SetActive(false);
                foreach (GameObject go in currentFrames[i])
                    go.SetActive(false);
            }
        }
        winLines.Clear();

        foreach (List<GameObject> go in currentFrames)
            go.Clear();

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

        int totalResult = 0;

        if (uniqueValue < 5)
        {
            if (numFroot + numClover == 5)
            {
                totalResult = 400;
            }
            else if (numFroot + numClover == 4)
            {
                totalResult = 250;
            }
            else if (numFroot + numClover == 3)
            {
                totalResult = 150;
            }
        }
        else if (uniqueValue == 5)
        {
            if (numFroot + numClover == 5)
            {
                totalResult = 600;
            }
            else if (numFroot + numClover == 4)
            {
                totalResult = 450;
            }
            else if (numFroot + numClover == 3)
            {
                totalResult = 250;
            }
        }
        else if (uniqueValue == 6)
        {
            if (numFroot + numClover == 5)
            {
                totalResult = 800;
            }
            else if (numFroot + numClover == 4)
            {
                totalResult = 600;
            }
            else if (numFroot + numClover == 3)
            {
                totalResult = 400;
            }
        }

        return totalResult;
    }
}
