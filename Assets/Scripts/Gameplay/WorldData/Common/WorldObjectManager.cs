using UnityEngine;
using System.Collections.Generic;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager Instance;

    public GameObject totemPrefab;

    Dictionary<Vector3Int, WorldObject> objects = new();

    void Awake() => Instance = this;

    public void SpawnTotem(Vector3Int pos)
    {
        if (objects.ContainsKey(pos)) return;

        if (!WorldGen.Instance.TryGetGroundY(pos.x, pos.z, out int groundY))
            return;

        Vector3Int spawnPos = new(
            pos.x,
            groundY + 1,
            pos.z
        );

        GameObject go = Instantiate(totemPrefab, spawnPos, Quaternion.identity);

        go.transform.SetParent(null);
        go.SetActive(true);

        go.transform.localScale = Vector3.one;
        go.layer = LayerMask.NameToLayer("Structures");

        TotemVoxelPlacer.Place(go);

        objects[spawnPos] = new WorldObject
        {
            position = spawnPos,
            visual = go
        };
    }
}
