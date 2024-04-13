using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotation : MonoBehaviour
{
    public float SpeedRotation { private get; set; } = 0f;

    public float PositionRotation { private get; set; } = 0f;

    public int ObstacleLvl { get; set; } = 1;

    void Update()
    {
        PositionRotation += SpeedRotation * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, 0, PositionRotation);
    }
}
