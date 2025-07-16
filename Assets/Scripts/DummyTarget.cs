using UnityEngine;
using UnityEngine.AI;


public class DummyTarget : MonoBehaviour
{
    private Animator m_Animator;

    public float health = 100f;

    private bool isDead = false;

    void Start()
    {

    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead == false)
        {
            Debug.Log($"{gameObject.name} has died.");

            isDead = true;
            Destroy(gameObject, 2f); 
        }
    }
}
