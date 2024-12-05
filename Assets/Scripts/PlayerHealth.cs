using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int _health;
    [SerializeField] public int _maxHealth;
    public Slider playerHealthSlider;

    [Header("Respawn Settings")]
    [SerializeField] private int maxRespawns = 0; // Maximum allowed respawns
    private int currentRespawnCount = 0;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    [Header("Damage Highlight")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float highlightDuration = 0.2f;
    private Color originalColor;

    [SerializeField] private Animator _animator;

    private bool isInvincible = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = _spriteRenderer.color;
    }

    private void Start()
    {
        _health = _maxHealth;
        playerHealthSlider.maxValue = _maxHealth;
        playerHealthSlider.value = _health;
    }

    public void TakeDamage(int damageValue, Vector2 hitDirection)
    {
        if (isInvincible || _health <= 0) return;

        StartCoroutine(EnableInvincibility());
        _health -= damageValue;
        playerHealthSlider.value = _health;

        if (_health > 0)
        {
            StartCoroutine(ApplyKnockback(hitDirection));
            StartCoroutine(HighlightOnHit());
        }
        else
        {
            HandlePlayerDeath();
        }
    }

    private void HandlePlayerDeath()
    {
        PlayerController.instance.isDead = true;
        Debug.Log("PLAYER DIED: GAME OVER");
        _animator.SetBool("isDead", true);
        Invoke(nameof(UponPlayerDeath), 0.5f);
    }

    private void UponPlayerDeath()
    {
        if (currentRespawnCount < maxRespawns)
        {
            currentRespawnCount++;
            Debug.Log($"Respawned {currentRespawnCount}/{maxRespawns}");
            StartCoroutine(RespawnAfterDelay(1.5f));
        }
        else
        {
            Debug.Log("No respawns left. GAME OVER!");
            // Trigger game over UI or actions
            EndGame();
        }
    }

    private IEnumerator ApplyKnockback(Vector2 hitDirection)
    {
        if (PlayerController.instance.isKnockedBack) yield break;

        PlayerController.instance.isKnockedBack = true;

        // Apply knockback force
        _rigidbody.AddForce(hitDirection * knockbackForce, ForceMode2D.Impulse);
        Debug.Log($"Knockback applied: Direction = {hitDirection}, Force = {knockbackForce}");

        // Wait for the knockback duration
        yield return new WaitForSeconds(knockbackDuration);

        // Reset velocity and knockback state
        _rigidbody.velocity = Vector2.zero;
        PlayerController.instance.isKnockedBack = false;
    }

    private IEnumerator HighlightOnHit()
    {
        _spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(highlightDuration);
        _spriteRenderer.color = originalColor;
    }

    private IEnumerator EnableInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1.0f); // Adjust duration as needed
        isInvincible = false;
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _health = _maxHealth;
        playerHealthSlider.value = _health;

        PlayerController.instance.Respawn();
    }


    public void Heal(int healAmount)
    {
        _health = Mathf.Min(_health + healAmount, _maxHealth);
        playerHealthSlider.value = _health;
    }

    public void UpdateHealthSliderValue()
    {
        playerHealthSlider.value = _health;
    }


    public void EndGame()
    {
        // Handle game over logic here (e.g., show Game Over screen, stop gameplay)
        Debug.Log("Game Over triggered.");
        Destroy(gameObject);
        UIManager.Instance.gameOverPanel_Ref.SetActive(true);
        Time.timeScale = 0; // Stop the game
    }
}
