using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World: MonoBehaviour
{
    public GameObject player;
    public int worldSizeX, worldSizeY, worldSizeZ;
    public float renderDistance;

    public GameObject chunkPrefeb;

    // 将position下取整的方块设置为blockId的方块
    public void SetBlock(Vector3 position, Block block)
    {
    }
    
    // 按长方体区域取方块
    // 以position下取整为坐标最小点，返回一个Block[cx,cy,cz]
    public Block[,,] GetBlocks(Vector3 position, int cx, int cy, int cz)
    {
        return null;
    }

    Chunk[,,] chunks;
    ChunkSaver chunkSaver;

    void Start()
    {
        BlockProtoTypeAssigner.Assign();
        chunkSaver = ChunkSaver.Create("test.save", worldSizeX, worldSizeY, worldSizeZ);
        chunks = new Chunk[worldSizeX, worldSizeY, worldSizeZ];
        for (int i = 0; i < worldSizeX; ++ i)
            for (int j = 0; j < worldSizeY; ++ j)
                for (int k = 0; k < worldSizeZ; ++ k)
                {
                    chunks[i, j, k] = Instantiate(
                        chunkPrefeb,
                        new Vector3(i * Chunk.chunkSizeX * Block.sideLength, j * Chunk.chunkSizeY * Block.sideLength, k * Chunk.chunkSizeZ * Block.sideLength),
                        Quaternion.identity,
                        gameObject.transform
                    ).GetComponent<Chunk>();
                    chunks[i, j, k].Load(chunkSaver, i, j, k);
                }
    }
}