using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    void Start()
    {
        float targetHeight = 6.15f;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        Camera.main.orthographicSize = targetHeight / 2f / screenAspect;
        
    }
}
