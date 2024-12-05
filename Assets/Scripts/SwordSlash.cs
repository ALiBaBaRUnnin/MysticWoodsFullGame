using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<SkeletonEnemy>().TakeDamage(1, collision.transform.position, 0f);
            Destroy(gameObject);
        }
        else { 
            Destroy(gameObject);
        }
    }
}
