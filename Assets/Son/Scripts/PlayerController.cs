using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastDirection;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private ParticleSystem waterSplash;
    [SerializeField] private Vector2 splashOffsetUp = new Vector2(0, 1);
    [SerializeField] private Vector2 splashOffsetDown = new Vector2(0, -1);
    [SerializeField] private Vector2 splashOffsetLeft = new Vector2(-1, 0);
    [SerializeField] private Vector2 splashOffsetRight = new Vector2(1, 0);
    [SerializeField] private float wateringAnimationDuration = 1f;

    private bool isWateringEffectActive = false;
    private bool isWatering = false;
    private bool isDigging = false;

    void Start()
    {
        waterSplash.Stop();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lastDirection = new Vector2(0, -1);
    }

    void Update()
    {
        if (FindObjectOfType<InventoryController>().IsInventoryOpen()) return;

        if (!isWatering && !isDigging)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement = Vector2.zero;
        }

        float speed = movement.magnitude;
        animator.SetFloat("Speed", speed);

        float directionX = movement.x;
        float directionY = movement.y;

        if (movement.x != 0 && movement.y != 0)
        {
            directionY = 0;
        }

        if (speed > 0)
        {
            lastDirection = new Vector2(directionX, directionY).normalized;
            animator.SetFloat("DirectionX", directionX);
            animator.SetFloat("DirectionY", directionY);

            if (isWateringEffectActive)
            {
                StopWaterSplash();
                animator.SetBool("IsWatering", false);
                isWateringEffectActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && speed == 0 && !isWatering && !isDigging)
        {
            StartCoroutine(HandleDigging());
        }

        if (Input.GetKeyDown(KeyCode.T) && speed == 0 && !isWatering)
        {
            isWatering = true;
            animator.SetBool("IsWatering", true);
            animator.SetBool("IsDigging", false);
            StartCoroutine(PlayWaterSplashAfterAnimation());
        }
    }

    void FixedUpdate()
    {
        if (!isWatering && !isDigging)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private IEnumerator PlayWaterSplashAfterAnimation()
    {
        yield return new WaitForSeconds(wateringAnimationDuration);
        PlayWaterSplash();
        isWateringEffectActive = true;

        float effectDuration = waterSplash.main.startLifetime.constant;
        yield return new WaitForSeconds(effectDuration);

        StopWaterSplash();
        animator.SetBool("IsWatering", false);
        isWateringEffectActive = false;
        isWatering = false;
    }

    void PlayWaterSplash()
    {
        if (waterSplash == null) return;

        waterSplash.Stop();
        Vector2 splashOffset = GetDirectionOffset(lastDirection);
        waterSplash.transform.localPosition = splashOffset;
        waterSplash.Play();
    }

    void StopWaterSplash()
    {
        if (waterSplash != null)
        {
            waterSplash.Stop();
            isWateringEffectActive = false;
        }
    }

    Vector2 GetDirectionOffset(Vector2 direction)
    {
        if (direction.y > 0) return splashOffsetUp;
        if (direction.y < 0) return splashOffsetDown;
        if (direction.x < 0) return splashOffsetLeft;
        if (direction.x > 0) return splashOffsetRight;
        return Vector2.zero;
    }

    public void SetDigDirection(Vector2Int playerCell, Vector2Int targetCell)
    {
        Vector2Int digDirection = GetSnappedDirection(playerCell, targetCell);
        lastDirection = digDirection;
        animator.SetFloat("DirectionX", lastDirection.x);
        animator.SetFloat("DirectionY", lastDirection.y);
        StartCoroutine(HandleDigging());
    }

    private Vector2Int GetSnappedDirection(Vector2Int playerCell, Vector2Int targetCell)
    {
        Vector2 diff = new Vector2(targetCell.x - playerCell.x, targetCell.y - playerCell.y);
        Vector2 snap = diff;

        // Ưu tiên trục Y
        if (Mathf.Abs(diff.y) >= Mathf.Abs(diff.x))
        {
            snap.x = 0;
            snap.y = Mathf.Sign(diff.y);
        }
        else
        {
            snap.y = 0;
            snap.x = Mathf.Sign(diff.x);
        }

        return new Vector2Int((int)snap.x, (int)snap.y);
    }

    private IEnumerator HandleDigging()
    {
        isDigging = true;
        animator.SetBool("IsDigging", true);
        animator.SetBool("IsWatering", false);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsDigging", false);
        isDigging = false;
    }
}
