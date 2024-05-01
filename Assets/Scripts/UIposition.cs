using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIposition : MonoBehaviour
{
    [SerializeField]
    private float defaultYPosition = -275f;

    private void Start()
    {
        float screenAspect = defaultYPosition * 2208 / Screen.height;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 position = rectTransform.localPosition;
        rectTransform.localPosition = new Vector3(position.x, screenAspect, position.z);
    }
}
