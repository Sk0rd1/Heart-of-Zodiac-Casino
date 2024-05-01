using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField]
    private SoundManager soundManager;

    private bool isPlay = false;
    private bool isReload = false;
    private bool isTimerStarted = false;
    private int countPickUpCoin = 0;
    public bool IsTouch { private get; set; } = false;

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

        if (isTimerStarted)
        {
            soundManager.GameOver();
            GameEngine.LoseGame();
        }
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
        bool isStartMoveSound = false;
        countPickUpCoin = 0;
        while (isPlay)
        {
            if (IsTouch && !isReload)
            {
                if(!isStartMoveSound)
                {
                    isStartMoveSound = true;
                    soundManager.KeyMove();
                }
                transform.position = new Vector3(0, transform.position.y + keySpeed * Time.deltaTime, -2);
            }
            else
            {
                isStartMoveSound = false;
                soundManager.KeyMoveStop();
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
            soundManager.KeyKnock();
            Reload();
        }

        if (collider.gameObject.CompareTag("SlotMachine"))
        {
            StopGame();
            soundManager.GameWin();
            GameEngine.WinGame(int.Parse(timerText.text), countPickUpCoin);
        }

        if (collider.gameObject.CompareTag("GameCoin"))
        {
            soundManager.PickUpCoin();
            countPickUpCoin += 1;
            Destroy(collider.gameObject);
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
