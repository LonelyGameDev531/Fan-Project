using System.Collections.Generic;
using UnityEngine;

public static class TotemVoxelPlacer
{
    public static void Place(GameObject totemGO)
    {
        for (int i = totemGO.transform.childCount - 1; i >= 0; i--)
            Object.Destroy(totemGO.transform.GetChild(i).gameObject);

        GameObject voxelGO = new GameObject("VoxelMesh");
        voxelGO.transform.SetParent(totemGO.transform, false);

        var mf = voxelGO.AddComponent<MeshFilter>();
        var mr = voxelGO.AddComponent<MeshRenderer>();
        var mc = voxelGO.AddComponent<MeshCollider>();

        mr.sharedMaterial = Resources.Load<Material>("Prefab/Blocks/BrickMaterial");
        Mesh voxelMesh = BuildTotemMeshLocal();
        mf.sharedMesh = voxelMesh;
        mc.sharedMesh = voxelMesh;

        voxelGO.transform.localPosition = Vector3.zero;
        voxelGO.transform.localRotation = Quaternion.identity;
        voxelGO.transform.localScale = Vector3.one;

        GameObject modelPrefab = Resources.Load<GameObject>("Prefab/Structures/Fish_totem");

        GameObject visualGO = Object.Instantiate(modelPrefab, totemGO.transform);
        visualGO.name = "ModelVisual";

        visualGO.transform.localPosition = new Vector3(0f, 2.5f, 0f); ;
        visualGO.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        visualGO.transform.localScale = Vector3.one;
    }

    private static Mesh BuildTotemMeshLocal()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        List<Vector3> verts = new();
        List<int> tris = new();
        List<Vector2> uvs = new();

        int sizeX = 2;
        int sizeY = 3;
        int sizeZ = 2;

        Vector3 pivot = new Vector3(sizeX * 0.5f, 0, sizeZ * 0.5f);

        Vector3[] cubeVerts =
        {
            new(0,0,0), new(1,0,0), new(1,1,0), new(0,1,0),
            new(0,0,1), new(1,0,1), new(1,1,1), new(0,1,1)
        };

        int[][] faces =
        {
            new[]{0,1,2,3},
            new[]{5,4,7,6},
            new[]{4,0,3,7},
            new[]{1,5,6,2},
            new[]{7,6,2,3},
            new[]{4,5,1,0}
        };

        Vector3Int[] dirs =
        {
            new(0,0,-1),
            new(0,0,1),
            new(-1,0,0),
            new(1,0,0),
            new(0,1,0),
            new(0,-1,0)
        };

        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
                for (int z = 0; z < sizeZ; z++)
                {
                    Vector3Int p = new(x, y, z);

                    for (int f = 0; f < 6; f++)
                    {
                        Vector3Int n = p + dirs[f];

                        if (n.x >= 0 && n.x < sizeX &&
                            n.y >= 0 && n.y < sizeY &&
                            n.z >= 0 && n.z < sizeZ)
                            continue;

                        int v = verts.Count;

                        foreach (int i in faces[f])
                        {
                            verts.Add(cubeVerts[i] + p - pivot);
                            uvs.Add(new Vector2(
                                (i == 1 || i == 2 || i == 5 || i == 6) ? 1 : 0,
                                (i >= 2 && i <= 3 || i >= 6) ? 1 : 0
                            ));
                        }

                        tris.Add(v + 0);
                        tris.Add(v + 1);
                        tris.Add(v + 2);
                        tris.Add(v + 0);
                        tris.Add(v + 2);
                        tris.Add(v + 3);
                    }
                }

        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
