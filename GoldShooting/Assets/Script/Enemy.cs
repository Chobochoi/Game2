using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScore;
    public float speed;
    public int health;
    public Sprite[] sprites;
    public float maxShotDelay;
    public float curShotDelay;
    public float power;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemBoom;
    public GameObject itemPower;
    public GameObject player;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        
    }

    void Update()
    {
       
        Fire();
        Reload();
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        if (enemyName == "A")
        {
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "B")
        {
            GameObject bulletR = Instantiate(bulletObjB, transform.position + Vector3.right * 0.3f, transform.rotation);
            GameObject bulletL = Instantiate(bulletObjB, transform.position + Vector3.left * 0.3f, transform.rotation);
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - transform.position;
            Vector3 dirVecL = player.transform.position - transform.position;
            rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);
        }
            curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

   public void OnHit(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            int ran = Random.Range(0, 10);
            if (ran < 5)
            {
                Debug.Log("Not Item");
            }
            else if ( ran < 8)
            {
                Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            }
            else if ( ran < 9)
            {
                Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
            }
            else if ( ran < 10)
            {
                Instantiate(itemPower, transform.position, itemPower.transform.rotation);
            }

            Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
        }
    }
}
