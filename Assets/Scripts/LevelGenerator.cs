using RobinGoodfellow.CircleGenerator;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public static class LevelGenerator
{
    private static Material[] materialObstacle = new Material[10];
    private static GameObject obstaclePrefab;
    private static bool isSpritesInitializated = false;
    private static List<GameObject> obstacles = new List<GameObject>();

    private static int[][] obstacleProbability = new int[][]
    {
        new int[] { 60, 25, 15, 0, 0, 0, 0, 0, 0, 0},
        new int[] { 35, 35, 20, 10, 0, 0, 0, 0, 0, 0},
        new int[] { 15, 20, 30, 25, 10, 0, 0, 0, 0, 0},
        new int[] { 5, 5, 20, 35, 20, 15, 0, 0, 0, 0},
        new int[] { 0, 0, 10, 20, 40, 20, 10, 0, 0, 0},
        new int[] { 0, 0, 0, 10, 20, 30, 30, 10, 0, 0},
        new int[] { 0, 0, 0, 0, 10, 10, 35, 20, 20, 5},
        new int[] { 0, 0, 0, 0, 0, 10, 20, 20, 25, 25},
        new int[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 55},
    };

    private static void InitializeSprites()
    {
        for (int i = 0; i < materialObstacle.Length; i++)
        {
            materialObstacle[i] = Resources.Load<Material>("CircleElements/Materials/lvl" + (i + 1).ToString());
        }

        obstaclePrefab = Resources.Load("CircleElements/Obstacle") as GameObject;

        isSpritesInitializated = true;
    }

    public static void CreateLevel(int keyLvl)
    {
        if (!isSpritesInitializated)
            InitializeSprites();

        ClearLevel();

        CreateObstacles(keyLvl);
    }

    public static void ClearLevel()
    {
        foreach(GameObject go in obstacles)
        {
            Object.Destroy(go);
        }
    }

    public static void CreateObstacles(int keyLvl)
    {
        int numOfObstacle = 4 + 2 * keyLvl;

        bool[][] intervals = new bool[4][];
        for (int i = 0; i < 4; i++)
            intervals[i] = new bool[360];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 360; j++)
            {
                intervals[i][j] = false;
            }
        }

        for (int i = 0; i < numOfObstacle; i++)
        {
            int rowNum = Random.Range(0, 4);
            int angleNum = Random.Range(0, 5);

            int obstacleProb = Random.Range(0, 100);

            int obstacleLvl = 0;

            for (int j = 0; j < obstacleProbability[keyLvl].Length; j++)
            {
                obstacleProb -= obstacleProbability[keyLvl][j];
                if(obstacleProb <= 0)
                {
                    obstacleLvl = j;
                    break;
                }
            }

            int angle;

            if (angleNum == 0)
                angle = 120;
            else if (angleNum == 1)
                angle = 90;
            else if (angleNum == 2)
                angle = 60;
            else if (angleNum == 3)
                angle = 45;
            else
                angle = 30;

            GameObject obstaclePrefab = Resources.Load<GameObject>("CircleElements/Circles/" + (rowNum + 1).ToString() + "(" + angle.ToString() + ")");
            GameObject go = GameObject.Instantiate(obstaclePrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            ObstacleRotation or = go.GetComponent<ObstacleRotation>();
            go.tag = "obstacle";

            go.GetComponent<ObstacleRotation>().ObstacleLvl = obstacleLvl + 1;
            go.GetComponent<MeshRenderer>().material = materialObstacle[obstacleLvl];

            PostProcessVolume volume = go.GetComponent<PostProcessVolume>();
            PostProcessProfile profile = volume.profile;
            Bloom bloom = profile.GetSetting<Bloom>();
            Color[] colors = new Color[] { new Color(255, 255, 255, 255), new Color(190, 190, 190, 255),
                new Color(146, 209, 79, 255), new Color(0, 175, 80, 255), new Color(1, 176, 241, 255), new Color(1, 32 ,96, 255),
                new Color(112, 48, 160, 255), new Color(255, 102, 0, 255), new Color(254, 0, 0, 255), new Color(32, 27, 27, 255)};
            ColorParameter colorParameter = new ColorParameter();
            colorParameter.value = colors[0];
            bloom.color = colorParameter;
            
            bool isEmpty = true;

            for (int count = 0; count < 30; count++)
            {
                int startPosition = Random.Range(0, 360);
                isEmpty = true;
                int endPosition = startPosition + angle * 2 - 1;

                for (int j = startPosition; j < endPosition; j++)
                {
                    if (j > 359)
                    {
                        if (intervals[rowNum][j - 360])
                        {
                            isEmpty = false;
                            break;
                        }
                    }
                    else
                    {
                        if (intervals[rowNum][j])
                        {
                            isEmpty = false;
                            break;
                        }
                    }
                }

                if (isEmpty)
                {
                    for (int j = startPosition; j < endPosition; j++)
                    {
                        if (j > 359)
                        {
                            intervals[rowNum][j - 360] = true;
                        }
                        else
                        {
                            intervals[rowNum][j] = true;
                        }
                    }
                    or.PositionRotation = 90 - startPosition;
                    if(rowNum % 2 == 0)
                    {
                        or.SpeedRotation = 30 + rowNum * 6f;
                    }
                    else
                    {
                        or.SpeedRotation = - 30 - rowNum * 6f;
                    }
                    break;
                }

                if(count == 9 || isEmpty)
                {
                    Object.Destroy(go);
                }
            }

            obstacles.Add(go);

        }

    }

    public static void MinusLvl()
    {
        foreach(GameObject go in obstacles)
        {
            try
            {
                int obLvl = go.GetComponent<ObstacleRotation>().ObstacleLvl;
                if (obLvl > 1)
                {
                    go.GetComponent<MeshRenderer>().material = materialObstacle[obLvl - 2];
                    go.GetComponent<ObstacleRotation>().ObstacleLvl = obLvl - 1;
                }
            }
            catch { }
        }
    }

}
