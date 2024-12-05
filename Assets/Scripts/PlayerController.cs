using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEditor;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float _speed;

    public float origninalSpeed;

    public bool isDead = false;

    [SerializeField]
    private float _attackDuration = 0.42f;

    [SerializeField]
    private float _attackCooldown = 0.8f;

    [SerializeField]
    private float _attackRange = 1.5f;

    [SerializeField]
    private Vector2 _attackOffset;

    [SerializeField]
    public int _enemyDamage = 1;

    [SerializeField]
    public float knockbackForce = 5f;

    [SerializeField]
    private float damage=1;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Vector2 _movementInput;
    private SpriteRenderer _spriteRenderer;
    private bool attacking = false;

    [Header("Enviroment")]
    [SerializeField] private FenceVariables _fence;
    public bool isKnockedBack = false;

    [Header("Sword Slash")]
    [SerializeField]
    private GameObject swordSlashPrefab; // Sword slash projectile prefab
    [SerializeField]
    private float swordSlashDelay = 0.2f;
    [SerializeField]
    private float swordSlashSpeed = 5f;
    private Vector2 lastFacingDirection = Vector2.right; // Default to facing right

    [Header("Respawn Settings")]
    [SerializeField] private Transform[] spawnPoints; // Assign these in the inspector

    [SerializeField] private PlayerHealth playerHealth;


    public void Respawn()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        // Choose a random spawn point
        //int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform RespawnPoint = GameObject.Find("Respawn Point").transform;

        // Move player to the selected spawn point
        if (RespawnPoint != null)
        {
            transform.position = RespawnPoint.position;
        }
        else {
            playerHealth.EndGame();
        }

        // Reset necessary player stats (example: health, speed, etc.)
        ResetPlayerStats();
    }

    private void ResetPlayerStats()
    {
        // Example: Reset speed and other stats
        Time.timeScale = 1f;
        isDead = false;
        playerHealth._health = playerHealth._maxHealth;
        playerHealth.UpdateHealthSliderValue();
        _speed = origninalSpeed;
        attacking = false;
        _animator.SetBool("isDead", false);
        _animator.SetBool("Run", false);
        // Reset any other necessary variables
    }

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _speed = origninalSpeed;
    }
    private void Update()
    {
        _attackCooldown -= Time.deltaTime;

        if (_speed <= 0)
        {
            _animator.SetBool("Run", false);
        }

        if (Input.GetMouseButtonDown(0) && !isDead)
        {
            if (!attacking)
            {
                StartCoroutine(Attack());
            }
        }
        else if (Input.GetKeyDown(KeyCode.X) && !isDead)
        {
            if (!attacking)
            {
                StartCoroutine(SwordSlashAttack());
            }
        }

        if (Input.GetMouseButtonDown(1) && !isDead)
        {
            TryInteract();
        }
    }
    private void TryInteract()
    {
        Slot s = FindObjectOfType<Inventory>().GetSelected();
        ItemButton iB = s.transform.GetComponentInChildren<ItemButton>();
        if (iB != null)
        {
            if (iB.gameObject.name.Contains("Fence"))
            {
                GridLayout gridLayout = FindObjectOfType<GridLayout>();
                Vector3Int cellPosition =
                    gridLayout.WorldToCell(transform.position + (Vector3)_attackOffset);
                //if (!_fence.Tilemap.HasTile(cellPosition))
                //{
                //    _fence.Tilemap.SetTile(cellPosition, _fence.Fence);
                //    iB.RemoveOne();
                //}
            }
        }
    }


    private void FixedUpdate()
    {
        if (isDead)
        {
            _rigidbody.velocity = Vector2.zero; // Stop the player from moving
            return;
        }

        if (!isKnockedBack)
        {
            // Apply player movement only if not knocked back
            _rigidbody.velocity = _movementInput * _speed;
        }
        else
        {
            // Keep existing velocity during knockback
            Debug.Log("Player is being knocked back.");
        }

        FlipSprite();

        if (_movementInput.x != 0 && _movementInput.y != 0)
        {
            _animator.SetBool("Horizontal", true);
            _animator.SetBool("Vertical", false);
        }
        else
        {
            _animator.SetBool("Horizontal", Mathf.Abs(_movementInput.x) > 0);
            _animator.SetBool("Vertical", Mathf.Abs(_movementInput.y) > 0);
        }

        if (_movementInput.y > 0)
            _animator.SetFloat("yPosition", 1f);
        else if (_movementInput.y < 0)
            _animator.SetFloat("yPosition", 0f);

        bool isMoving = Mathf.Abs(_movementInput.x) > 0 || Mathf.Abs(_movementInput.y) > 0;
        _animator.SetBool("Run", isMoving);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 gizmoPosition = (Vector2)transform.position + _attackOffset * _attackRange;

        Gizmos.DrawWireSphere(gizmoPosition, _attackRange);
    }
    private void FlipSprite()
    {
        if (_movementInput.x > 0)
            _spriteRenderer.flipX = false;
        else if (_movementInput.x < 0)
            _spriteRenderer.flipX = true;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            _movementInput = Vector2.zero; // Ignore input if the player is dead
            return;
        }

        _movementInput = context.ReadValue<Vector2>();

        if (_movementInput != Vector2.zero)
        {
            lastFacingDirection = _movementInput.normalized; // Update facing direction
        }

        if (_movementInput.x != 0 && _movementInput.y != 0)
        {
            _animator.SetBool("Horizontal", true);
            _animator.SetBool("Vertical", false);
        }
        else
        {
            _animator.SetBool("Horizontal", Mathf.Abs(_movementInput.x) > 0);
            _animator.SetBool("Vertical", Mathf.Abs(_movementInput.y) > 0);
        }

        if (_movementInput.y > 0)
        {
            _attackOffset = new Vector2(0f, 0.2f);
            _animator.SetFloat("yPosition", 1f);
        }
        else if (_movementInput.y < 0)
        {
            _attackOffset = new Vector2(0f, -2.2f);
            _animator.SetFloat("yPosition", 0f);
        }

        if (_movementInput.x > 0)
        {
            _attackOffset = new Vector2(1.2f, -0.8f);
            _spriteRenderer.flipX = false;
        }
        else if (_movementInput.x < 0)
        {
            _attackOffset = new Vector2(-1.2f, -0.8f);
            _spriteRenderer.flipX = true;
        }
    }
    public IEnumerator Attack()
    {
        if (_attackCooldown <= 0)
        {
            attacking = true;
            _attackCooldown = 0.8f;
            _speed = 0f;
            _animator.SetTrigger("Attack");
            SoundManager.instance.Play("PlayerHit");
            _animator.SetBool("Run", false);
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position + (Vector3)_attackOffset * _attackRange, _attackRange);

            foreach (Collider2D hitObject in hitObjects)
            {
                if (hitObject.TryGetComponent<TreeScript>(out var tree))
                {
                    tree.TakeDamage((int)damage);
                }

                SkeletonEnemy skeletonEnemy = hitObject.GetComponent<SkeletonEnemy>();
                if (skeletonEnemy != null)
                {
                    skeletonEnemy.TakeDamage(_enemyDamage, transform.position, _attackDuration);
                }

                Vector3Int cellPosition = FindObjectOfType<GridLayout>().WorldToCell(transform.position + (Vector3)_attackOffset);
                //if (_fence.Tilemap.HasTile(cellPosition))
                //{
                //    _fence.Tilemap.SetTile(cellPosition, null);
                //    Instantiate(_fence.Pickup, cellPosition + new Vector3(0.5f, 0.5f), Quaternion.identity);
                //}
            }

            yield return new WaitForSeconds(_attackDuration);
            _speed = origninalSpeed;

            attacking = false;
        }
    }

    public IEnumerator SwordSlashAttack()
    {
        if (_attackCooldown <= 0)
        {
            attacking = true;
            _attackCooldown = 0.8f;
            _speed = 0f;

            _animator.SetTrigger("Attack"); // Play attack animation
            yield return new WaitForSeconds(swordSlashDelay);

            // Determine facing direction based on lastFacingDirection
            Vector3 slashDirection = new Vector3(lastFacingDirection.x, lastFacingDirection.y, 0);
            Vector3 spawnOffset = slashDirection.normalized; // Adjust spawn based on direction
            float rotationZ = 0f;

            if (lastFacingDirection.y > 0) // Facing Up
            {
                rotationZ = 90f; // Rotate 90 degrees for upward direction
            }
            else if (lastFacingDirection.y < 0) // Facing Down
            {
                rotationZ = -90f; // Rotate -90 degrees for downward direction
            }
            else if (lastFacingDirection.x > 0) // Facing Right
            {
                rotationZ = 0f; // No rotation needed for right direction
            }
            else if (lastFacingDirection.x < 0) // Facing Left
            {
                rotationZ = 180f; // Rotate 180 degrees for left direction
            }

            // Spawn the sword slash
            GameObject slash = Instantiate(swordSlashPrefab, transform.position + new Vector3(spawnOffset.x, spawnOffset.y - 0.5f, spawnOffset.z), Quaternion.Euler(0, 0, rotationZ));
            Rigidbody2D slashRb = slash.GetComponent<Rigidbody2D>();

            if (slashRb != null)
            {
                slashRb.velocity = slashDirection * swordSlashSpeed;
            }

            yield return new WaitForSeconds(_attackDuration);
            _speed = origninalSpeed;
            attacking = false;
        }
    }





    [System.Serializable]
    public class FenceVariables
    {
        public Tilemap Tilemap;
        public GameObject Pickup;
        public RuleTile Fence;
    }
}
