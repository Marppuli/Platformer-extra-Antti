using TMPro;
using UnityEngine;
using GA.Platformer;

public class HealthDisplay : MonoBehaviour
{
    public Health CurrentHealth;  // Reference to the Health script
    public TextMeshProUGUI healthText;  // Reference to the TextMeshProUGUI component

    void Start()
    {
        if (CurrentHealth == null)
        {
            CurrentHealth = GetComponentInParent<Health>();
        }

        if (healthText == null)
        {
            healthText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        if (CurrentHealth != null && healthText != null)
        {
            // Update the health display text
            healthText.text = CurrentHealth.CurrentHealth.ToString();
        }
    }
}
