using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    [SerializeField]
    private Vector2 startPosition;
    [SerializeField]
    private Vector2 endPosition;

    private bool isMoveToEnd = true;

    private float distance;
    private float currentDistance = 0;

    private void OnEnable()
    {
        distance = Vector2.Distance(startPosition, endPosition);
    }

    void Update()
    {
        if(isMoveToEnd)
        {
            if(currentDistance >= distance)
            {
                currentDistance = 0;
                isMoveToEnd = false;
            }
            Vector3 oneFrameDistance = new Vector3((endPosition.x - startPosition.x), (endPosition.y - startPosition.y), 0) * Time.deltaTime;
            currentDistance += Vector3.Distance(Vector3.zero, oneFrameDistance);
            transform.position += oneFrameDistance;
        }
        else
        {
            if (currentDistance >= distance)
            {
                currentDistance = 0;
                isMoveToEnd = true;
            }
            Vector3 oneFrameDistance = new Vector3((startPosition.x - endPosition.x), (startPosition.y - endPosition.y), 0) * Time.deltaTime;
            currentDistance += Vector3.Distance(Vector3.zero, oneFrameDistance);
            transform.position += oneFrameDistance;
        }
    }
}
