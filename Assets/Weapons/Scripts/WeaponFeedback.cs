using UnityEngine;
using System.Collections.Generic;

public class WeaponFeedback : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public int damageAmount = 10;
    public float hitCooldown = 0.5f; // cooldown in seconds
    public List<GameObject> hitParticles; // assign multiple FX if you want randomness


    private Dictionary<Collider, float> lastHitTimes = new Dictionary<Collider, float>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monster")) return;

        float currentTime = Time.time;

        // Check if this monster was hit recently
        if (lastHitTimes.ContainsKey(other))
        {
            float lastHitTime = lastHitTimes[other];
            if (currentTime - lastHitTime < hitCooldown)
            {
                // Still in cooldown window, skip hit
                return;
            }
        }

        // Register the hit time
        lastHitTimes[other] = currentTime;

        // Spawn floating damage text
        Vector3 directionToMonster = (other.transform.position - transform.position).normalized;
        Vector3 hitPoint = other.ClosestPoint(transform.position + directionToMonster * 0.5f);
        Vector3 spawnPosition = hitPoint + Vector3.up * 0.01f;

        if (floatingTextPrefab != null)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);
            FloatingDamageText fdt = floatingText.GetComponent<FloatingDamageText>();
            if (fdt != null)
            {
                fdt.SetDamage(damageAmount);
            }
        }

        // === Spawn random particle effect ===
        if (hitParticles != null && hitParticles.Count > 0)
        {
            GameObject fx = hitParticles[Random.Range(0, hitParticles.Count)];
            spawnPosition = other.ClosestPoint(transform.position);

            // Calculate hit direction and spawn rotation
            Vector3 hitDirection = (spawnPosition - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(hitDirection);

            // Spawn random particle effect
            if (hitParticles != null && hitParticles.Count > 0)
            {
                fx = hitParticles[Random.Range(0, hitParticles.Count)];
                Instantiate(fx, spawnPosition, rotation);
            }

            Debug.Log("Hit monster for " + damageAmount + " damage");
        }
    }
}