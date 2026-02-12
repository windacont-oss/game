using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 60;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
