using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int hitTime = 3;
    public Sprite dmgSprite;
    public SpriteRenderer spriteRenderer;

    public AudioClip hitSound1;
    public AudioClip hitSound2;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        SoundManager.instance.RandomizeSfx(hitSound1, hitSound2);
        spriteRenderer.sprite = dmgSprite;

        hitTime -= loss;

        if (hitTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }    
}
