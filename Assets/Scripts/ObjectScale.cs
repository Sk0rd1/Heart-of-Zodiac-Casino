using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectScale : MonoBehaviour
{
    [SerializeField]
    private float minScale = 45;
    [SerializeField]
    private float maxScale = 55;
    [SerializeField]
    private float speedScale = 15;

    private bool isIncrease = true;

    private float currentScale;

    private void Start()
    {
        currentScale = minScale;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isIncrease)
        {
            if (currentScale >= maxScale)
            {
                isIncrease = false;
            }

            currentScale += speedScale * Time.deltaTime;
        }
        else
        {
            if (currentScale <= minScale)
            {
                isIncrease = true;
            }

            currentScale -= speedScale * Time.deltaTime;
        }

        this.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }
}
