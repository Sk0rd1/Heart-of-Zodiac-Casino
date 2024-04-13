using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private float keySpeed = 10f;
    [SerializeField]
    private int keyLvl = 6;
    [SerializeField]
    private float timeReload = 2f;
    [SerializeField]
    private float roundDuration = 30f;

    private bool isPlay = false;
    private bool isReload = false;
    private bool isTimerStarted = false;

    public void StartGame()
    {
        isPlay = true;
        isReload = false;
        timerText.text = ((int)roundDuration).ToString();
        transform.position = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    public void StartTimer()
    {
        if (!isTimerStarted)
        {
            isTimerStarted = true;
            StartCoroutine(GameTimer());
        }
    }

    private IEnumerator GameTimer()
    {
        for(float i = 0; i < roundDuration; i += Time.deltaTime)
        {
            if (!isTimerStarted)
                break;
            timerText.text = ((int)(roundDuration - i)).ToString();
            yield return new WaitForEndOfFrame();
        }

        if(isTimerStarted)
            GameEngine.LoseGame();
        StopGame();
    }

    public void StopGame()
    {
        isPlay = false;
        isReload = false;
        isTimerStarted = false;
    }

    public void SetKeyParameters(int upKeyLvl, int upKeySpeed, int upTimeReload, int roundDuration)
    {
        keyLvl = upKeyLvl + 1;

        keySpeed = 1f + 0.2f * upKeySpeed;

        timeReload = 2 - 0.2f * upTimeReload;

        this.roundDuration = 15 + 2.5f * roundDuration;
    }

    private IEnumerator Move()
    {
        while (isPlay)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !isReload)
            {
                transform.position = new Vector3(0, transform.position.y + keySpeed * Time.deltaTime, -2);
            }
            else
            {
                transform.position = new Vector3(0, transform.position.y - keySpeed * Time.deltaTime, -2);

                if (transform.position.y < 0)
                {
                    transform.position = Vector3.zero;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("obstacle"))
        {
            if (collider.gameObject.GetComponent<ObstacleRotation>().ObstacleLvl <= keyLvl)
            {
                Destroy(collider.gameObject);
            }
            Reload();
        }

        if (collider.gameObject.CompareTag("SlotMachine"))
        {
            StopGame();
            GameEngine.WinGame(int.Parse(timerText.text));
        }
    }

    private void Reload()
    {
        isReload = true;
        gameObject.transform.position = Vector3.zero;

        StartCoroutine(ReloadAnimation());
    }

    private IEnumerator ReloadAnimation()
    {
        bool rotationDirection = true;
        float currentTime = 0;
        float speedRotation = 90f;

        transform.rotation = Quaternion.Euler(0, 0, -13);

        for (float i = 0; i < timeReload; i += Time.deltaTime)
        {
            if (!isReload) break;

            float rotation;
            if (rotationDirection)
            {
                rotation = transform.rotation.eulerAngles.z + speedRotation * Time.deltaTime;
            }
            else
            {
                rotation = transform.rotation.eulerAngles.z - speedRotation * Time.deltaTime;
            }

            transform.rotation = Quaternion.Euler(0, 0, rotation);

            currentTime += Time.deltaTime;
            if(currentTime >= 0.3f)
            {
                rotationDirection = !rotationDirection;
                currentTime = 0;
            }

            yield return new WaitForEndOfFrame();
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForEndOfFrame();
        isReload = false;
    }

    public void PressAddTime()
    {
        roundDuration += 10f;
    }
}
