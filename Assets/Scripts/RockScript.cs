using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public GameObject[] itemsToDrop;
    public int minItemsToDrop = 1;
    public int maxItemsToDrop = 3;

    public float lineOffset = 0.5f;
    public float spacing = 2.0f;

    private bool takingDamage = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);

            int numItemsToDrop = Random.Range(minItemsToDrop, maxItemsToDrop + 1);

            List<Vector3> spawnPositions = GenerateSpawnPositions(numItemsToDrop);

            for (int i = 0; i < numItemsToDrop; i++)
            {
                if (itemsToDrop.Length > 0)
                {
                    int randomIndex = Random.Range(0, itemsToDrop.Length);
                    GameObject item = itemsToDrop[randomIndex];

                    Instantiate(item, spawnPositions[i], Quaternion.identity);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!takingDamage)
        {
            currentHealth -= damage;

            takingDamage = true;
            StartCoroutine(ResetDamageFlag());
        }
    }

    private IEnumerator ResetDamageFlag()
    {
        yield return new WaitForSeconds(0.5f);
        takingDamage = false;
    }

    private List<Vector3> GenerateSpawnPositions(int count)
    {
        List<Vector3> positions = new List<Vector3>();

        Vector3 bottomPosition = transform.position - Vector3.up * lineOffset;

        for (int i = 0; i < count; i++)
        {
            Vector3 position = bottomPosition + Vector3.right * i * spacing;

            float randomOffset = Random.Range(-spacing * 0.25f, spacing * 0.25f);
            position += Vector3.right * randomOffset;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, spacing * 0.5f);
            bool positionOccupied = colliders.Length > 0;

            while (positionOccupied)
            {
                randomOffset = Random.Range(-spacing * 0.25f, spacing * 0.25f);
                position += Vector3.right * randomOffset;

                colliders = Physics2D.OverlapCircleAll(position, spacing * 0.5f);
                positionOccupied = colliders.Length > 0;
            }

            positions.Add(position);
        }

        return positions;
    }
}
