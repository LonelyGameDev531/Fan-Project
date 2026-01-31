[System.Serializable]
public struct ChunkBlock
{
    public ChunkBlockType type;
    public byte data;

    public bool IsSolid => type != ChunkBlockType.Air;
}
