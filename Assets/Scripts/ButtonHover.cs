using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = hoverColor; // Lights up
    }

    void OnMouseExit()
    {
        spriteRenderer.color = normalColor; // Returns to normal
    }

    void OnMouseDown()
    {
        Debug.Log("Start Button Clicked!");
        // Example: UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}

