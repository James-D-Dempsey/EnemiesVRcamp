using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "GameData/HealthData")]
public class HealthData : ScriptableObject
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }
}
