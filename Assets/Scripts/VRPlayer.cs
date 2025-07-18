using UnityEngine;
using System.Collections; 

public class VRPlayer : MonoBehaviour
{
    public HealthData healthData;
    private bool isDead = false;
    public Transform respawnPoint;
    public float respawnDelay = 2f;
    void Start()
    {
        isDead = false;

        if (healthData != null)
        {
            healthData.ResetHealth();
            Debug.Log($"[VRPlayer] Health reset to {healthData.currentHealth}");
        }
        else
        {
            Debug.LogError("[VRPlayer] No HealthData ScriptableObject assigned!");
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead || healthData == null || healthData.IsDead()) return;

        healthData.TakeDamage(amount);
        Debug.Log($"Player took {amount} damage. Current health: {healthData.currentHealth}");

        if (healthData.IsDead())
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player has died.");

        StartCoroutine(Respawn());
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (healthData != null)
        {
            healthData.ResetHealth();
        }

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation; // optional
        }

        isDead = false;

        Debug.Log("Player respawned.");
    }

    public float GetCurrentHealth()
    {
        return healthData != null ? healthData.currentHealth : 0f;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
