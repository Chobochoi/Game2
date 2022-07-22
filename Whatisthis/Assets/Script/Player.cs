using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Move
{
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    private Animator animator;
    private int health = 100;
    public int wallDamage = 1;
    public int food;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound1;
    public float restartDelay = 2.0f;
    
    SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    void Update()
    {
        if (!GameManager.instance.playersTurn) return;
        
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0)
            vertical = 0;
        if (horizontal != 0 || vertical != 0)
        {
            if (horizontal == 1)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontal == -1)
            {
                spriteRenderer.flipX=true;
            }
            AttemptMove<Wall>(horizontal, vertical);
        }        
    }
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        health--;
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        if (Moving(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }
        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }      

    private void CheckIfGameOver()
    {
        if (health <= 0)
        {
            GameManager.instance.GameOver();

            SoundManager.instance.PlaySingle(gameOverSound1);

            SoundManager.instance.musicSource.Stop();
        }
    }

    protected override void OnCantMove<T>(T Component)
    {
        Wall hitWall = Component as Wall;

        hitWall.DamageWall(wallDamage);

        animator.SetTrigger("PlayerAttack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            Invoke("Restart", restartDelay);
            enabled = false;
        }
        
        else if (collision.tag == "Food")
        {
            health += pointsPerFood;

            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Soda")
        {
            health += pointsPerSoda;

            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);

            collision.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    
    public void LoseFood(int loss)
    {
        animator.SetTrigger("PlayerHit");
        food -= loss;
    }
}
