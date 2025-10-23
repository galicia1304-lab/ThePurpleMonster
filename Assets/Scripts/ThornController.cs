using UnityEngine;

public class ThornController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    void Update()
    {
        // Move towards pointB
        transform.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

        // Reset to pointA when reaching pointB
        if (Vector2.Distance(transform.position, pointB.position) < 0.01f)
            transform.position = pointA.position;
    }

}

