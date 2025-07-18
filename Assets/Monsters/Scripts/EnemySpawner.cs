using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Prefab of the enemy to spawn")]
    public GameObject enemyPrefab;

    [Tooltip("Time in seconds between spawns")]
    public float spawnInterval = 5f;

    [Tooltip("If true, the first spawn happens immediately")]
    public bool spawnImmediately = true;

    private Coroutine _spawnRoutine;

    void OnEnable()
    {
        _spawnRoutine = StartCoroutine(SpawnLoop());
    }

    void OnDisable()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }

    private IEnumerator SpawnLoop()
    {
        if (spawnImmediately)
            SpawnEnemy();

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }
    }


    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        // 1) pick your spawn‑cube’s world location
        Vector3 wantPos = transform.position;

        // 2) sample the NavMesh up to 5 units away
        if (NavMesh.SamplePosition(wantPos, out NavMeshHit hit, 50f, NavMesh.AllAreas))
        {
            // hit.position is now guaranteed to lie on the NavMesh
            GameObject go = Instantiate(enemyPrefab, hit.position, transform.rotation, transform);
            // no further action needed—agent starts already on the mesh
        }
        else
        {
            Debug.LogWarning("Spawner: No NavMesh within 5 units!", this);
        }
    }
}
