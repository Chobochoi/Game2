using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Move
{
    private Animator animator;
    private int health = 100;
    public int wallDamage;
    public int foodEnergy;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
    }

    void Update()
    {
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0)
            vertical = 0;
        if (horizontal != 0 || vertical != 0)
        {
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

        }
    }

    protected override void OnCantMove<T>(T Component)
    {
        Wall hitWall = Component as Wall;

        hitWall.DamageWall(wallDamage);

        animator.SetTrigger("State");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            health += foodEnergy;

            collision.gameObject.SetActive(false);
        }
    }
}
