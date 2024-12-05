using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private float idleTime = 0.2f;

    [SerializeField]
    private float moveTime = 4f;

    [SerializeField]
    private float chanceToChangeDirection = 0.5f;

    [SerializeField]
    private float moveRadius = 5f;

    [SerializeField]
    private float detectionRange = 3f;

    [SerializeField]
    private string playerTag = "Player";

    [SerializeField]
    private float stoppingDistance = 0.2f;

    [SerializeField]
    private Transform enemyAttackPoint;

    [SerializeField]
    private float attackRange = 0.5f;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private int attackDamage;

    [SerializeField]
    private float initialAttackDelay = 0.5f;

    [SerializeField]
    private int skeletonHealth = 10;

    [SerializeField]
    private bool isAttacking = false;

    [SerializeField]
    private float attackCooldown = 2f;

    [SerializeField]
    private float knockbackForce = 5f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 startPosition;
    private bool isIdling = false;
    public bool playerDetected = false;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    private Animator animator;
    [SerializeField]
    private Slider enemyHealthBar;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        playerTransform = GameObject.FindWithTag(playerTag).transform;
        enemyHealthBar.maxValue = skeletonHealth;
        MoveRandomDirection();
    }

    private void Update()
    {
        if (playerDetected && playerTransform != null)
        {
            playerTransform = GameObject.FindWithTag(playerTag).transform;
        }
    }


    public IEnumerator Attack()
    {
        if (!isAttacking)
        {
            SoundManager.instance.Play("SkeletonHit");
            animator.SetBool("Walk", false);
            isAttacking = true;

            yield return new WaitForSeconds(initialAttackDelay);

            animator.Play("Attack");

            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(enemyAttackPoint.position, attackRange, playerLayer);

            foreach (Collider2D player in hitObjects)
            {
                Debug.Log(player.gameObject.name);
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Vector2 hitDirection = (PlayerController.instance.transform.position - transform.position).normalized;
                    if (PlayerController.instance.GetComponent<PlayerHealth>()._health > 0)
                    {
                        PlayerController.instance.GetComponent<PlayerHealth>().TakeDamage(attackDamage, hitDirection);
                    }

                    //playerHealth.TakeDamage(attackDamage);
                }
                else
                {
                    Debug.LogWarning("PlayerHealth component not found on " + player.gameObject.name);
                }
            }

            yield return new WaitForSeconds(attackCooldown);
            isAttacking = false;
        }
    }


    private void FixedUpdate()
    {
        if (!isIdling)
        {

            DetectPlayer();

            if (playerDetected)
            {
                ChasePlayer();
            }
            else
            {
                rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
                animator.SetBool("Walk", true);

                spriteRenderer.flipX = moveDirection.x < 0;
            }

            ClampToCircle();
        }
    }


    private void ChasePlayer()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > stoppingDistance)
        {
            rb.MovePosition(rb.position + directionToPlayer * moveSpeed * Time.deltaTime);
            animator.SetBool("Walk", true);
        }
        else
        {
            StartCoroutine(Attack());
        }

        spriteRenderer.flipX = directionToPlayer.x < 0;
    }



    private void DetectPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector3(transform.position.x, transform.position.y - 0.8f, 0), detectionRange);
        bool detected = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(playerTag))
            {
                detected = true;
                break;
            }
        }

        playerDetected = detected;
        if (!playerDetected)
        {
            isIdling = false;
        }
    }

    private void ClampToCircle()
    {
        float distanceFromStart = Vector2.Distance(startPosition, rb.position);
        if (distanceFromStart > moveRadius)
        {
            Vector2 directionToStart = (startPosition - rb.position).normalized;
            moveDirection = directionToStart;
        }
    }

    private void MoveRandomDirection()
    {
        float randomAngle = UnityEngine.Random.Range(0f, 360f);
        moveDirection = Quaternion.Euler(0f, 0f, randomAngle) * Vector2.right;

        isIdling = false;
        Invoke(nameof(Idle), moveTime);
    }

    private void Idle()
    {
        isIdling = true;
        animator.SetBool("Walk", false);

        Invoke(nameof(MoveRandomDirection), idleTime);
    }

    public void TakeDamage(int damageValue, Vector3 attackerPosition, float _attackDuration)
    {
        if (skeletonHealth > 0)
        {
            skeletonHealth -= damageValue;
            enemyHealthBar.value = skeletonHealth;
        }

        if (skeletonHealth <= 0)
        {
            Debug.Log("SKELETON DIED");/////////////////////////////////SKELETON DEATH HANDLED HERE
            Destroy(this.gameObject);
        }

        Vector2 knockbackDirection = (transform.position - attackerPosition).normalized;
        StartCoroutine(KnockbackCoroutine(knockbackDirection, _attackDuration));

    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackDirection, float _attackDuration)
    {
        float elapsedTime = 0f;
        Vector2 initialPosition = transform.position;
        Vector2 targetPosition = initialPosition + knockbackDirection * knockbackForce;

        while (elapsedTime < _attackDuration)
        {
            transform.position = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / _attackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isIdling && UnityEngine.Random.value < chanceToChangeDirection)
        {
            MoveRandomDirection();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPosition, moveRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - 0.8f, 0), detectionRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(enemyAttackPoint.position, attackRange);

    }
}