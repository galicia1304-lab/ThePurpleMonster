using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Template Character (disabled in scene)")]
    public GameObject characterTemplate;

    [Header("Spawn Settings")]
    public Vector3 spawnPosition = Vector3.zero;

    public void SpawnCharacter()
    {
        if (characterTemplate == null)
        {
            Debug.LogError("No character template assigned!");
            return;
        }

        // Create a new GameObject for the character
        GameObject newCharacter = new GameObject("RuntimeCharacter");

        // Set position
        newCharacter.transform.position = spawnPosition;

        // Copy essential components
        CopyAnimator(characterTemplate, newCharacter);
        CopyRenderer(characterTemplate, newCharacter);

        // Optional: copy Rigidbody/Collider if you need physics
        if (characterTemplate.TryGetComponent<Rigidbody>(out Rigidbody rbTemplate))
        {
            Rigidbody rb = newCharacter.AddComponent<Rigidbody>();
            rb.mass = rbTemplate.mass;

        }

        if (characterTemplate.TryGetComponent<Collider>(out Collider colTemplate))
        {
            System.Type colType = colTemplate.GetType();
            newCharacter.AddComponent(colType);
        }
    }

    private void CopyAnimator(GameObject template, GameObject target)
    {
        Animator templateAnimator = template.GetComponent<Animator>();
        if (templateAnimator != null)
        {
            Animator animator = target.AddComponent<Animator>();
            animator.runtimeAnimatorController = templateAnimator.runtimeAnimatorController;
            animator.applyRootMotion = templateAnimator.applyRootMotion;
        }
    }

    private void CopyRenderer(GameObject template, GameObject target)
    {
        // For 2D
        if (template.TryGetComponent<SpriteRenderer>(out SpriteRenderer srTemplate))
        {
            SpriteRenderer sr = target.AddComponent<SpriteRenderer>();
            sr.sprite = srTemplate.sprite;
            sr.color = srTemplate.color;
        }

        // For 3D
        if (template.TryGetComponent<MeshRenderer>(out MeshRenderer mrTemplate) &&
            template.TryGetComponent<MeshFilter>(out MeshFilter mfTemplate))
        {
            MeshFilter mf = target.AddComponent<MeshFilter>();
            mf.mesh = mfTemplate.mesh;

            MeshRenderer mr = target.AddComponent<MeshRenderer>();
            mr.materials = mrTemplate.materials;
        }
    }
}


