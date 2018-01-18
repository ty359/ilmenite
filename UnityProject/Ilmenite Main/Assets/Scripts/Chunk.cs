using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk : MonoBehaviour
{
    public const int chunkSizeX = 16;
    public const int chunkSizeY = 16;
    public const int chunkSizeZ = 16;

    public Block[,,] blocks;

    public void Load(int x, int y, int z)
    {
        blocks = new Block[chunkSizeX, chunkSizeY, chunkSizeZ];
        for (int i = 0; i < chunkSizeX; i++)
            for (int j = 0; j < chunkSizeY; j++)
                for (int k = 0; k < chunkSizeZ; k++)
                {
                    if (j <= 4 && y == 0)
                    {
                        blocks[i, j, k] = Block.NewDirtBlock();
                    }
                }
    }
    public void Save(int x, int y, int z)
    {
    }
    public void Free()
    {
    }

    bool renderUpToDate = false;

    public void SetBlock(int x, int y, int z, Block block)
    {
        if (blocks == null)
        {
            Debug.LogError("Try to setblock before load chunk");
        } else
        {
            blocks[x, y, z] = block;
            renderUpToDate = false;
        }
    }

    public void MeshUpdate()
    {
        if (renderUpToDate || blocks == null)
            return;
        MeshList meshList = new MeshList();

        for (int i = 0; i < chunkSizeX; i++)
            for (int j = 0; j < chunkSizeY; j++)
                for (int k = 0; k < chunkSizeZ; k++)
                    if (blocks[i, j, k] != null)
                        for (int s = 0; s < 6; ++ s)
                            if (!IsSolid(i - (int)directions[s].x, j - (int)directions[s].y, k - (int)directions[s].z))
                                Block.BuildMeshFace(
                                    meshList,
                                    new Vector3(i + .5f, j + .5f, k + .5f),
                                    Quaternion.FromToRotation(directions[0], directions[s]),
                                    blocks[i, j, k].GetBlockFaceUV(s)
                                );

        meshList.ApplyTo(gameObject.GetComponent<MeshFilter>().mesh);
        gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
        renderUpToDate = true;
    }

    bool IsSolid(int x, int y, int z)
    {
        if (x < 0 || y < 0 || z < 0 || x >= chunkSizeX || y >= chunkSizeY || z >= chunkSizeZ)
            return false;
        if (blocks[x, y, z] == null)
            return false;
        return blocks[x, y, z].IsSolid();
    }
    static Vector3[] directions = {
        new Vector3(0, 0, 1),
        new Vector3(0, 1, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, 0, -1),
        new Vector3(0, -1, 0),
        new Vector3(-1, 0, 0),
    };

    void Update()
    {
        MeshUpdate();
    }
}