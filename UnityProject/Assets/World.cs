using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    // Scale
    public const int CHUNK_COUNT_X = 256;
    public const int CHUNK_COUNT_Y = 16;
    public const int CHUNK_COUNT_Z = 256;

    // Basic
    GameObject[,,] chunkMap;
    ChunkSaver chunkSaver;
    List<GameObject> chunkPool;
    public const int CHUNK_POOL_SIZE = 1024;

    void ActiveChunk(int x, int y, int z)
    {
        if (chunkMap[x, y, z] == null)
        {
            if (chunkPool.Count > 0)
            {
                GameObject newChunk = chunkPool[chunkPool.Count - 1];
                chunkPool.RemoveAt(chunkPool.Count - 1);
                newChunk.GetComponent<Chunk>().Reset(x, y, z);
                newChunk.GetComponent<Chunk>().Load(chunkSaver);
                newChunk.SetActive(true);
                chunkMap[x, y, z] = newChunk;
            }
            else
            {
                Debug.LogError("No more free chunks in chunkPool!");
            }
        }
    }

    void FreeChunk(int x, int y, int z)
    {
        if (chunkMap[x, y, z] != null)
        {
            GameObject theChunk = chunkMap[x, y, z];
            chunkMap[x, y, z] = null;
            theChunk.GetComponent<Chunk>().Unload(chunkSaver);
            theChunk.GetComponent<Chunk>().Free();
            theChunk.SetActive(false);
            chunkPool.Add(theChunk);
        }
    }

    void Init()
    {
        chunkSaver = new ChunkSaver("Chunk.save");
        chunkMap = new GameObject[CHUNK_COUNT_X, CHUNK_COUNT_Y, CHUNK_COUNT_Z];
        chunkPool = new List<GameObject>();
        for (int i = 0; i < CHUNK_POOL_SIZE; ++i)
        {
            chunkPool.Add(Instantiate(Resources.Load("ChunkObject") as GameObject));
        }
    }

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
	}
}
