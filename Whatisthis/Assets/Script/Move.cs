using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : MonoBehaviour
{
    public float moveTime = 0.1f;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rigid;
    private float inverseMoveTime;
    private bool isMoving = false;
    public LayerMask blockingLayer;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
    }

    protected bool Moving(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);

        boxCollider.enabled = true;
        if (hit.transform == null && !isMoving)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rigid.position, end, inverseMoveTime * Time.deltaTime);
            rigid.MovePosition(newPosition);

            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            
            yield return null;
        }
        rigid.MovePosition(end);

        isMoving = false;
    }      

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Moving(xDir, yDir, out hit);
        if (hit.transform == null)
            return;
        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);        
    }

    protected abstract void OnCantMove<T>(T Component) where T : Component;

    
}
