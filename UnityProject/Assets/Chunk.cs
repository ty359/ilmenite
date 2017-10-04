using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    East = 0,
    West,
    Up,
    Down,
    South,
    North,
    In,
    Out,
}

public class TemporaryMesh
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();
}

public class BlockType
{
    public byte typeID;
    public Vector2[] cubeUVs;
}

public class Block
{
    public Block(byte id)
    {
        typeID = id;
    }
    public Block()
    {
    }

    // Scale
    public const float SIZE_X = 0.5F;
    public const float SIZE_Y = 0.5F;
    public const float SIZE_Z = 0.5F;

    // Block Type
    byte typeID;
    public static BlockType[] blockTypeList = new BlockType[256];
    public static void InitBlockTypeList()
    {
        InitAirBlock();
        InitDirtBlock();
    }
    static void InitAirBlock()
    {
        blockTypeList[0] = new BlockType();
        blockTypeList[0].typeID = 0;
        blockTypeList[0].cubeUVs = null;
    }
    static void InitDirtBlock()
    {
        blockTypeList[1] = new BlockType();
        blockTypeList[1].typeID = 1;
        blockTypeList[1].cubeUVs = new Vector2[]
        {
            new Vector2(0.00f, 0.75f),
            new Vector2(0.00f, 1.00f),
            new Vector2(0.25f, 1.00f),
            new Vector2(0.25f, 0.75f)
        };
    }

    // Render
    public void RenderAt(Vector3 pos, TemporaryMesh mesh)
    {
        if (blockTypeList[typeID].cubeUVs != null)
        {
            RenderCubeAt(pos, mesh);
        }
    }

    void RenderCubeAt(Vector3 pos, TemporaryMesh mesh)
    {
        RenderCubeFaceAt(pos, Direction.East, mesh);
        RenderCubeFaceAt(pos, Direction.West, mesh);
        RenderCubeFaceAt(pos, Direction.Up, mesh);
        RenderCubeFaceAt(pos, Direction.Down, mesh);
        RenderCubeFaceAt(pos, Direction.South, mesh);
        RenderCubeFaceAt(pos, Direction.North, mesh);
    }
    void RenderCubeFaceAt(Vector3 pos, Direction direction, TemporaryMesh mesh)
    {
        int verticesCountBase = mesh.vertices.Count;
        switch (direction)
        {
            case Direction.East:
                mesh.vertices.Add(GetVertice(pos, 1, 0, 0));
                mesh.vertices.Add(GetVertice(pos, 1, 1, 0));
                mesh.vertices.Add(GetVertice(pos, 1, 1, 1));
                mesh.vertices.Add(GetVertice(pos, 1, 0, 1));
                break;

            case Direction.West:
                mesh.vertices.Add(GetVertice(pos, 0, 0, 0));
                mesh.vertices.Add(GetVertice(pos, 0, 0, 1));
                mesh.vertices.Add(GetVertice(pos, 0, 1, 1));
                mesh.vertices.Add(GetVertice(pos, 0, 1, 0));
                break;

            case Direction.Up:
                mesh.vertices.Add(GetVertice(pos, 0, 1, 0));
                mesh.vertices.Add(GetVertice(pos, 0, 1, 1));
                mesh.vertices.Add(GetVertice(pos, 1, 1, 1));
                mesh.vertices.Add(GetVertice(pos, 1, 1, 0));
                break;

            case Direction.Down:
                mesh.vertices.Add(GetVertice(pos, 0, 0, 0));
                mesh.vertices.Add(GetVertice(pos, 1, 0, 0));
                mesh.vertices.Add(GetVertice(pos, 1, 0, 1));
                mesh.vertices.Add(GetVertice(pos, 0, 0, 1));
                break;

            case Direction.South:
                mesh.vertices.Add(GetVertice(pos, 0, 0, 1));
                mesh.vertices.Add(GetVertice(pos, 1, 0, 1));
                mesh.vertices.Add(GetVertice(pos, 1, 1, 1));
                mesh.vertices.Add(GetVertice(pos, 0, 1, 1));
                break;

            case Direction.North:
                mesh.vertices.Add(GetVertice(pos, 0, 0, 0));
                mesh.vertices.Add(GetVertice(pos, 0, 1, 0));
                mesh.vertices.Add(GetVertice(pos, 1, 1, 0));
                mesh.vertices.Add(GetVertice(pos, 1, 0, 0));
                break;
        }
        mesh.triangles.Add(verticesCountBase + 0);
        mesh.triangles.Add(verticesCountBase + 1);
        mesh.triangles.Add(verticesCountBase + 2);

        mesh.triangles.Add(verticesCountBase + 0);
        mesh.triangles.Add(verticesCountBase + 2);
        mesh.triangles.Add(verticesCountBase + 3);

        mesh.uvs.AddRange(blockTypeList[typeID].cubeUVs);
    }

    Vector3 GetVertice(Vector3 pos, int x, int y, int z)
    {
        return new Vector3(pos.x + x * SIZE_X, pos.y + y * SIZE_Y, pos.z + z * SIZE_Z);
    }

    // Read & Write
    public void Read(BinaryReader reader)
    {
        typeID = reader.ReadByte();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(typeID);
    }
}

public class ChunkSaver
{
    public ChunkSaver(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file " + path + " do not exists!");
            Debug.Log("Create new save file!" + path);
            fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.SetLength(HEADER_X * HEADER_Y * HEADER_Z * sizeof(long));
        }
        else
        {
            fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
    }

    // Header Scale
    public const int HEADER_X = World.CHUNK_COUNT_X;
    public const int HEADER_Y = World.CHUNK_COUNT_Y;
    public const int HEADER_Z = World.CHUNK_COUNT_Z;

    FileStream fs;
    long HeaderOffset(int X, int Y, int Z)
    {
        return ((X) * HEADER_Y + Y) * HEADER_Z + Z;
    }

    public BinaryReader LoadChunk(int X, int Y, int Z)
    {
        fs.Seek(HeaderOffset(X, Y, Z), SeekOrigin.Begin);
        long chunkOffset = (new BinaryReader(fs)).ReadInt64();
        fs.Seek(chunkOffset, SeekOrigin.Begin);
        if (chunkOffset == 0)
            return null;
        else
            return new BinaryReader(fs);
    }

    public BinaryWriter SaveChunk(int X, int Y, int Z)
    {
        long chunkOffset = fs.Seek(0, SeekOrigin.End);
        fs.Seek(HeaderOffset(X, Y, Z), SeekOrigin.Begin);
        (new BinaryWriter(fs)).Write(chunkOffset);
        fs.Seek(0, SeekOrigin.End);
        return new BinaryWriter(fs);
    }
}

[RequireComponent(typeof(MeshFilter), typeof(Transform), typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    // Scale
    public const int BLOCK_COUNT_X = 16;
    public const int BLOCK_COUNT_Y = 16;
    public const int BLOCK_COUNT_Z = 16;

    public const float SIZE_X = BLOCK_COUNT_X * Block.SIZE_X;
    public const float SIZE_Y = BLOCK_COUNT_Y * Block.SIZE_Y;
    public const float SIZE_Z = BLOCK_COUNT_Z * Block.SIZE_Z;

    // Basic
    int X, Y, Z;
    Block[,,] blockMap;
    Vector3 basePos;
    Vector3 GetBlockPos(int x, int y, int z)
    {
        return basePos + new Vector3(x * Block.SIZE_X, y * Block.SIZE_Y, z * Block.SIZE_Z);
    }

    // Render
    Mesh mesh;
    bool needRenderUpdate = true;
    public void RenderUpdate()
    {
        if (!needRenderUpdate)
            return;
        if (blockMap == null)
            throw new System.Exception("Try to run RenderUpdate() while the chunk is not loaded!");
        TemporaryMesh tmpMesh = new TemporaryMesh();
        for (int i = 0; i < BLOCK_COUNT_X; ++i)
            for (int j = 0; j < BLOCK_COUNT_Y; ++j)
                for (int k = 0; k < BLOCK_COUNT_Z; ++k)
                    blockMap[i, j, k].RenderAt(GetBlockPos(i, j, k), tmpMesh);
        mesh.vertices = tmpMesh.vertices.ToArray();
        mesh.triangles = tmpMesh.triangles.ToArray();
        mesh.uv = tmpMesh.uvs.ToArray();
        needRenderUpdate = false;
    }

    // Load & Unload
    public void Load(ChunkSaver chunkSaver)
    {
        blockMap = new Block[BLOCK_COUNT_X, BLOCK_COUNT_Y, BLOCK_COUNT_Z];
        BinaryReader reader = chunkSaver.LoadChunk(X, Y, Z);
        for (int i = 0; i < BLOCK_COUNT_X; ++i)
            for (int j = 0; j < BLOCK_COUNT_Y; ++j)
                for (int k = 0; k < BLOCK_COUNT_Z; ++k)
                {
                    blockMap[i, j, k] = new Block();
                    blockMap[i, j, k].Read(reader);
                }
    }

    public void Unload(ChunkSaver chunkSaver)
    {
        BinaryWriter writer = chunkSaver.SaveChunk(X, Y, Z);
        for (int i = 0; i < BLOCK_COUNT_X; ++i)
            for (int j = 0; j < BLOCK_COUNT_Y; ++j)
                for (int k = 0; k < BLOCK_COUNT_Z; ++k)
                {
                    blockMap[i, j, k].Write(writer);
                }
        blockMap = null;
    }

    // Init
    public void Reset(int x, int y, int z)
    {
        GetComponent<Transform>().position = new Vector3(x * SIZE_X, y * SIZE_Y, z * SIZE_Z);
        basePos = GetComponent<Transform>().position;
        mesh = GetComponent<MeshFilter>().mesh;
        X = x;
        Y = y;
        Z = z;
    }

    public void Free()
    {
        mesh.Clear();
        blockMap = null;
    }

    // Use this for initialization
    void Start()
    {
    }

	// Update is called once per frame
	void Update()
    {
    }
}
