using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ChunkSaver
{
    const int version = 31415;

    ChunkSaver() { }

    int headerSize0 = 128;
    string savePath;
    FileStream fileStream;
    int sizeX, sizeY, sizeZ;
    public static ChunkSaver Create(string path, int sizeX, int sizeY, int sizeZ)
    {
        ChunkSaver saver = new ChunkSaver
        {
            savePath = path,
            fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite),
            sizeX = sizeX,
            sizeY = sizeY,
            sizeZ = sizeZ,
        };
        saver.fileStream.Write(System.BitConverter.GetBytes(ChunkSaver.version), 0, 4);
        saver.fileStream.Write(System.BitConverter.GetBytes(sizeX), 0, 4);
        saver.fileStream.Write(System.BitConverter.GetBytes(sizeY), 0, 4);
        saver.fileStream.Write(System.BitConverter.GetBytes(sizeZ), 0, 4);
        saver.fileStream.Seek(saver.headerSize0, SeekOrigin.Begin);
        for (int x = 0; x < sizeX; ++x)
            for (int y = 0; y < sizeY; ++y)
                for (int z = 0; z < sizeZ; ++z)
                {
                    saver.fileStream.Write(System.BitConverter.GetBytes(0), 0, 4);
                    saver.fileStream.Write(System.BitConverter.GetBytes(0), 0, 4);
                }
        Debug.Log("Create new save file " + path);
        return saver;
    }
    public static ChunkSaver Open(string path)
    {
        ChunkSaver saver = new ChunkSaver()
        {
            savePath = path,
            fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite),
        };
        saver.fileStream.Seek(0, SeekOrigin.Begin);
        byte[] buffer = new byte[16];
        saver.fileStream.Read(buffer, 0, 4);
        int v = System.BitConverter.ToInt32(buffer, 0);
        if (v != version)
        {
            Debug.LogError("incorrect save file");
            return null;
        }
        saver.fileStream.Read(buffer, 0, 12);
        saver.sizeX = System.BitConverter.ToInt32(buffer, 0);
        saver.sizeY = System.BitConverter.ToInt32(buffer, 4);
        saver.sizeZ = System.BitConverter.ToInt32(buffer, 8);
        Debug.Log("Open save file " + path);
        return saver;
    }
    public void GetSize(out int x, out int y, out int z)
    {
        x = sizeX;
        y = sizeY;
        z = sizeZ;
    }
    public byte[] GetChunk(int x, int y, int z)
    {
        fileStream.Seek(headerSize0 + 8 * (((x) * sizeY + y) * sizeZ + z), SeekOrigin.Begin);
        byte[] buffer = new byte[8];
        fileStream.Read(buffer, 0, 8);
        int shift = System.BitConverter.ToInt32(buffer, 0);
        int length = System.BitConverter.ToInt32(buffer, 4);
        byte[] ret = new byte[length];
        fileStream.Seek(shift, SeekOrigin.Begin);
        fileStream.Read(ret, 0, length);
        return ret;
    }
    public void SaveChunk(int x, int y, int z, byte[] bytes)
    {
        int shift = (int)fileStream.Seek(0, SeekOrigin.Current);
        int length = bytes.Length;
        fileStream.Seek(headerSize0 + 8 * (((x) * sizeY + y) * sizeZ + z), SeekOrigin.Begin);
        fileStream.Write(System.BitConverter.GetBytes(shift), 0, 4);
        fileStream.Write(System.BitConverter.GetBytes(length), 0, 4);
        fileStream.Seek(0, SeekOrigin.End);
        fileStream.Write(bytes, 0, length);
    }
}

public class Chunk : MonoBehaviour
{
    public const int chunkSizeX = 16;
    public const int chunkSizeY = 16;
    public const int chunkSizeZ = 16;

    public Block[,,] blocks;

    public void Load(ChunkSaver saver, int x, int y, int z)
    {
        blocks = new Block[chunkSizeX, chunkSizeY, chunkSizeZ];
        for (int i = 0; i < chunkSizeX; i++)
            for (int j = 0; j < chunkSizeY; j++)
                for (int k = 0; k < chunkSizeZ; k++)
                {
                    if (j <= (i + k) / 5 && y == 0)
                    {
                        blocks[i, j, k] = Block.NewDirtBlock();
                    }
                }
    }
    public void Save(ChunkSaver saver, int x, int y, int z)
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