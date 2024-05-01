using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    void Start()
    {
        float targetHeight = 6.15f;
        float screenAspect = targetHeight / 2f / ((float)Screen.width / (float)Screen.height);
        if (screenAspect < 5.45f)
            Camera.main.orthographicSize = 5.45f;
        else
            Camera.main.orthographicSize = screenAspect;
        
    }
}
