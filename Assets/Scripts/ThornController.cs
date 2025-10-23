using UnityEngine;

public class ThornController : MonoBehaviour
{
    public Transform pointA; // Start point
    public Transform pointB; // End point
    public float speed = 3f;

    void Update()
    {
        // Move towards point B
        transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

        // When reaching point B, teleport back to point A
        if (Vector3.Distance(transform.position, pointB.position) < 0.01f)
        {
            transform.position = pointA.position;
        }
    }
}