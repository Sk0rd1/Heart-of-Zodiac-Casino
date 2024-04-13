using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCell : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;

    public bool isFakeCell = true;
    public float PositionY { get; set; } = -1; 

    private void Start()
    {
        if (isFakeCell)
            MoveFakeCell();
        else
            MoveNormalCell();
    }

    private IEnumerator MoveNormalCell()
    {
        while (transform.position.y >= PositionY)
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }

        transform.position = new Vector3(transform.position.x, PositionY, transform.position.z);
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
