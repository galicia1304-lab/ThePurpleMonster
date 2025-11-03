using TMPro;
using UnityEngine;

public class HeartUI : MonoBehaviour
{
    public TextMeshProUGUI heartText;

    void Update()
    {
        heartText.text = "Coins: " + GameManager.Instance.heartCount;
    }
}

