using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public float moveSpeed;           // Movement speed
    public float wanderRadius;        // How far the slime can wander from its current position
    public float arriveDistance = 0.1f;    // How close to the target before picking a new one

    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeTargetRoutine());
    }

    void Update()
    {
        MoveToTarget();
    }

    IEnumerator ChangeTargetRoutine()
    {
        while (true)
        {
            PickNewTarget();
            yield return new WaitForSeconds(2f);
        }
    }

    void PickNewTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        targetPosition = (Vector2)transform.position + randomCircle;
    }

    void MoveToTarget()
    {
        Vector2 currentPosition = rb.position;
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

        // Flip sprite based on movement direction
        float direction = newPosition.x - currentPosition.x;
        if (Mathf.Abs(direction) > 0.01f && spriteRenderer != null)
        {
            spriteRenderer.flipX = direction < 0;
        }

        rb.MovePosition(newPosition);
    }
}
