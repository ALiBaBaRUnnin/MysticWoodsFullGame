using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float range;

    [SerializeField]
    private float maxDistance;

    private Vector2 wayPoint;

    private void Start()
    {
        SetNewDestination();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDestination();
        }
    }

    private void SetNewDestination()
    {
        wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }
}
