using UnityEngine;

public class TotemSystem : MonoBehaviour
{
    [Header("Unit Spawn")]
    public GameObject unitPrefab;
    public float spawnInterval = 5f;
    public int maxAliveUnits = 5;
    public int spawnRadius = 3;

    Vector3Int gridPos;
    float timer;
    int aliveUnits;

    void Awake()
    {
        WorldGen.Instance.TryGetGroundY(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z),
            out int groundY
        );

        gridPos = new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            groundY,
            Mathf.RoundToInt(transform.position.z)
        );
    }

    void Update()
    {
        if (aliveUnits >= maxAliveUnits) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawnUnit();
        }
    }

    void TrySpawnUnit()
    {
        if (unitPrefab == null) return;

        if (!WorldGen.Instance.TryGetGroundY(gridPos.x, gridPos.z, out int groundY))
            return;

        gridPos.y = groundY + 1;

        var spots = WorldGen.Instance.GetWalkableRing(gridPos, spawnRadius, gridPos.y);
        if (spots.Count == 0) spots.Add(gridPos);

        Vector3Int p = spots[Random.Range(0, spots.Count)];
        Vector3 spawnWorldPos = p + new Vector3(0.5f, 0f, 0.5f);

        GameObject unitGO = Instantiate(unitPrefab, spawnWorldPos, Quaternion.identity);

        UnitGeneric unit = unitGO.GetComponent<UnitGeneric>();
        if (unit == null)
            unit = unitGO.AddComponent<UnitGeneric>();

        unit.ownerTotem = this;

        aliveUnits++;
    }

    public void NotifyUnitDeath()
    {
        aliveUnits = Mathf.Max(0, aliveUnits - 1);
    }
}
