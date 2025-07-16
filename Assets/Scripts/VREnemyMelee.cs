using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class VREnemyMelee : MonoBehaviour
{
    [Header("AI Settings")]
    public Transform target;
    public float moveSpeed = 3.5f;           // ← Adjust in Inspector
    public float AttackDistance = 5f;
    public float attackCooldown = 1f;

    [Header("Health Settings")]
    public float maxHealth = 50f;
    [Tooltip("When false, plays Death anim and stops AI.")]
    private bool Alive = true;                // ← Exposed in Inspector

    private float currentHealth;
    private float lastAttackTime = 0f;

    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private VRPlayer playerHealth;
    private float m_Distance;

    [Header("SFX")]
    public AudioClip walkClip;
    public AudioClip attackClip;
    public AudioClip deathClip;

    private AudioSource sfxSource;   // for one‑shots
    private AudioSource loopSource;  // optional, for walking loop

    [Header("Damage Popup")]
    public GameObject damagePopupPrefab;  // assign your Canvas+Text prefab
    private Vector3 popupOffset = new Vector3(0, 2f, 0);

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        playerHealth = target.GetComponent<VRPlayer>();

        // initialize
        currentHealth = maxHealth;
        m_Agent.speed = moveSpeed;
        m_Animator.SetBool("Alive", true);

        var sources = GetComponents<AudioSource>();
        sfxSource = sources[0];
        loopSource = sources.Length > 1 ? sources[1] : sfxSource;
        loopSource.clip = walkClip;
        loopSource.loop = true;
        loopSource.playOnAwake = false;
    }

    void Update()
    {
        
        // if we’re dead, bail out
        if (!Alive) return;

        // allow tweaking speed at runtime
        m_Agent.speed = moveSpeed;

        m_Distance = Vector3.Distance(transform.position, target.position);

        bool isMoving = m_Agent.velocity.magnitude > 0.1f && Alive;

        // 2) Start/stop loopSource
        if (isMoving && !loopSource.isPlaying)
            loopSource.Play();
        else if (!isMoving && loopSource.isPlaying)
            loopSource.Stop();

        if (m_Distance < AttackDistance && Time.time - lastAttackTime >= attackCooldown)
        {
            m_Agent.isStopped = true;
            m_Animator.SetTrigger("Attack");
            sfxSource.PlayOneShot(attackClip);

            if (playerHealth != null && !playerHealth.IsDead())
            {
                playerHealth.TakeDamage(10f);
                lastAttackTime = Time.time;
            }
        }
        else
        {
            m_Agent.isStopped = false;
            m_Agent.destination = target.position;
        }
    }

    /// <summary>
    /// Call this to damage the enemy.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (!Alive) return;

        currentHealth -= amount;

        Vector3 worldPos = transform.position + popupOffset;
        DamagePopup.Create(damagePopupPrefab, worldPos, Mathf.CeilToInt(amount).ToString());

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Alive = false;                   // flips the Inspector bool
        m_Agent.isStopped = true;        // freeze movement
        m_Animator.SetBool("Alive", false); // trigger the Death transition
        sfxSource.PlayOneShot(deathClip);
        Destroy(gameObject, 15f);
    }
}
