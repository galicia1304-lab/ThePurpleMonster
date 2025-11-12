using UnityEngine;

public class Fireball : MonoBehaviour
{
    void OnBecameInvisible()
    {
        // Destroy this fireball when it's no longer visible by the camera
        Destroy(gameObject);
    }
}

