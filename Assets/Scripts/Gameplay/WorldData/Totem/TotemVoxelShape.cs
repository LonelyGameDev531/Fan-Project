using UnityEngine;

public static class TotemVoxelShape
{
    public static readonly Vector3Int[] Blocks =
    {
        new(0,0,0), new(1,0,0), new(2,0,0),
        new(0,0,1), new(1,0,1), new(2,0,1),

        new(0,1,0), new(1,1,0), new(2,1,0),
        new(0,1,1), new(1,1,1), new(2,1,1),
    };

    public const int SizeX = 3;
    public const int SizeY = 2;
    public const int SizeZ = 2;
}
