using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSprite : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites = new List<Sprite>();

    [SerializeField]
    private float timeDuration = 1.5f;

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float oneFrameTime = timeDuration / sprites.Count;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        for(int i = 0; i < sprites.Count; i++) 
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(oneFrameTime);
        }
    }
}
