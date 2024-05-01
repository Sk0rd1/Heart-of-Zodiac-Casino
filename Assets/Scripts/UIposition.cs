using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIposition : MonoBehaviour
{
    [SerializeField]
    private float defaultYPosition = -275f;
    //default Y position for 1242x2208;

    private void Start()
    {
        //if (Screen.height > 2208)
        //{
            float screenAspect = defaultYPosition * 2208 / Screen.height;
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector3 position = rectTransform.localPosition;
            rectTransform.localPosition = new Vector3(position.x, screenAspect, position.z);
        //}
    }
}
