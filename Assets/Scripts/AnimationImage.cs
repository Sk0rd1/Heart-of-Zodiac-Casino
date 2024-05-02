using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimationImage : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> spritesSecondStage = new List<Sprite>();

    [SerializeField]
    private float timeDuration = 1.5f;

    private Image spriteRenderer;

    public void SecondStageAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateSecondStage());
    }

    private void OnEnable()
    {
        float oneFrameTime = timeDuration / sprites.Count;
        spriteRenderer = GetComponent<Image>();
        StartCoroutine(Animate(oneFrameTime));
    }

    private IEnumerator Animate(float oneFrameTime)
    {
        while (true)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                spriteRenderer.sprite = sprites[i];
                yield return new WaitForSeconds(oneFrameTime);
            }
        }
    }

    private IEnumerator AnimateSecondStage()
    {
        float oneFrameTime = timeDuration / sprites.Count;
        Image spriteRenderer = GetComponent<Image>();
        for (int i = 0; i < spritesSecondStage.Count; i++)
        {
            spriteRenderer.sprite = spritesSecondStage[i];
            yield return new WaitForSeconds(oneFrameTime);
        }

        this.gameObject.SetActive(false);
    }
}
