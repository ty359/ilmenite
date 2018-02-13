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
                        blocks[i, j, k] = Block.CreateBlockByID(1);
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

    public void RenderUpdate()
    {
        if (renderUpToDate || blocks == null)
            return;
        ChunkRenderHandle renderHandle = new ChunkRenderHandle(this);

        for (int i = 0; i < chunkSizeX; ++i)
            for (int j = 0; j < chunkSizeY; ++j)
                for (int k = 0; k < chunkSizeZ; ++k)
                    if (blocks[i, j, k] != null)
                    {
                        blocks[i, j, k].RenderAt(renderHandle, this.GetBlockCenterPosition(i, j, k), BlockVisibleStatus.AllVisible);
                    }
        renderHandle.Apply();

        renderUpToDate = true;
    }

    Vector3 GetBlockCenterPosition(int x, int y, int z)
    {
        return new Vector3(Block.sideLength * (x + 0.5f), Block.sideLength * (y + 0.5f), Block.sideLength * (z + 0.5f));
    }

    void Update()
    {
        RenderUpdate();
    }
}

public class ChunkRenderHandle
{
    public class MeshList
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector2> uv = new List<Vector2>();
        public List<int> triangles = new List<int>();
        public void ApplyTo(Mesh mesh)
        {
            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = triangles.ToArray();
        }
        public int VCount()
        {
            return vertices.Count;
        }
    }

    public MeshList meshList;
    public GameObject chunkObject;

    public ChunkRenderHandle(Chunk chunk)
    {
        meshList = new MeshList();
        chunkObject = chunk.gameObject;
    }

    public void Apply()
    {
        meshList.ApplyTo(chunkObject.GetComponent<MeshFilter>().mesh);
        chunkObject.GetComponent<MeshCollider>().sharedMesh = chunkObject.GetComponent<MeshFilter>().mesh;
//        throw new System.NotImplementedException();
    }
}