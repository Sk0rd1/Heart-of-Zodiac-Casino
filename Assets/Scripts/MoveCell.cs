using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCell : MonoBehaviour
{
    private float moveSpeed = 15f;

    private float positionY = -1;

    public void SetYPosition(float positionY, bool isLastElement = false)
    {
        this.positionY = positionY;

        StartCoroutine(MoveNormalCell(isLastElement));
    }

    public void MoveToBot()
    {
        StartCoroutine(MoveFakeCell());
    }

    private IEnumerator MoveNormalCell(bool isLastElement)
    {
        while (transform.position.y >= positionY)
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }
        
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
        if(isLastElement)
        {
            GameObject.Find("SlotsObjects").GetComponent<SlotsGenerator>().TotalResult();
        }
    }

    private IEnumerator MoveFakeCell()
    {
        while (transform.position.y > -1.1)
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }

        Destroy(this.gameObject);
    }
}
