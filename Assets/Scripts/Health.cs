using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public const int MAXHEALTH = 100;

    public int currentHealth = MAXHEALTH;
    public RectTransform healthBar;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }

        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }
}
