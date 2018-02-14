using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World: MonoBehaviour
{
    public GameObject player;
    public int worldSizeX, worldSizeY, worldSizeZ;
    public GameObject chunkPrefeb;

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
                    chunks[i, j, k] = Chunk.CreateChunkInstance(this, i, j, k);
                    chunks[i, j, k].Load(chunkSaver);
                }
    }
}