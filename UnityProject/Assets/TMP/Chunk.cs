using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[RequireComponent(typeof(MeshFilter), typeof(Transform))]
public class Chunk : MonoBehaviour {

    public class Block
    {
        enum Direction
        {
            East = 0,
            West,
            Up,
            Down,
            South,
            North,
            In,
            Out
        }

        public enum BlockType
        {
            Air = 0,
            Dirt = 1,
            Grass = 3,
            Gravel = 4,
        }

        Chunk parentChunk;
        public BlockType type;
        byte x, y ,z;

        public Block(Chunk _parentChunk, byte _x, byte _y, byte _z, BlockType _type)
        {
            parentChunk = _parentChunk;
            x = _x;
            y = _y;
            z = _z;
            type = _type;
        }

        public void MeshUpdate(List<Vector3> meshVertices, List<int> meshTriangles, List<Vector2> meshUVs)
        {
            if (NeedUpdateFace(Direction.Up))
                MeshUpdateFace(Direction.Up, meshVertices, meshTriangles, meshUVs);
            if (NeedUpdateFace(Direction.Down))
                MeshUpdateFace(Direction.Down, meshVertices, meshTriangles, meshUVs);
            if (NeedUpdateFace(Direction.East))
                MeshUpdateFace(Direction.East, meshVertices, meshTriangles, meshUVs);
            if (NeedUpdateFace(Direction.West))
                MeshUpdateFace(Direction.West, meshVertices, meshTriangles, meshUVs);
            if (NeedUpdateFace(Direction.South))
                MeshUpdateFace(Direction.South, meshVertices, meshTriangles, meshUVs);
            if (NeedUpdateFace(Direction.North))
                MeshUpdateFace(Direction.North, meshVertices, meshTriangles, meshUVs);
        }

        // TODO
        bool NeedUpdateFace(Direction direction)
        {
            return true;
        }

        Vector3 GetVertice(int east, int up, int south)
        {
            Vector3 ret = new Vector3((x + east) * BlockXShift, (y + up) * BlockYShift, (z + south) * BlockZShift) + parentChunk.basePos;
            Debug.Log(ret.ToString());
            return ret;
        }

        void AddTexture(Direction direction, List<Vector2> meshUVs)
        {
            Vector2 uvWidth = new Vector2(0.25f, 0.25f);
            Vector2 uvCorner = new Vector2(0.00f, 0.75f);

            uvCorner.x += (float)(type - 1) / 4;
            meshUVs.Add(uvCorner);
            meshUVs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
            meshUVs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
            meshUVs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));
        }

        void MeshUpdateFace(Direction direction, List<Vector3> meshVertices, List<int> meshTriangles, List<Vector2> meshUVs)
        {
            int verticesCountBase = meshVertices.Count;
            switch (direction)
            {
                case Direction.East:
                    meshVertices.Add(GetVertice(1, 0, 0));
                    meshVertices.Add(GetVertice(1, 1, 0));
                    meshVertices.Add(GetVertice(1, 1, 1));
                    meshVertices.Add(GetVertice(1, 0, 1));
                    break;

                case Direction.West:
                    meshVertices.Add(GetVertice(0, 0, 0));
                    meshVertices.Add(GetVertice(0, 0, 1));
                    meshVertices.Add(GetVertice(0, 1, 1));
                    meshVertices.Add(GetVertice(0, 1, 0));
                    break;

                case Direction.Up:
                    meshVertices.Add(GetVertice(0, 1, 0));
                    meshVertices.Add(GetVertice(0, 1, 1));
                    meshVertices.Add(GetVertice(1, 1, 1));
                    meshVertices.Add(GetVertice(1, 1, 0));
                    break;

                case Direction.Down:
                    meshVertices.Add(GetVertice(0, 0, 0));
                    meshVertices.Add(GetVertice(1, 0, 0));
                    meshVertices.Add(GetVertice(1, 0, 1));
                    meshVertices.Add(GetVertice(0, 0, 1));
                    break;

                case Direction.South:
                    meshVertices.Add(GetVertice(0, 0, 1));
                    meshVertices.Add(GetVertice(1, 0, 1));
                    meshVertices.Add(GetVertice(1, 1, 1));
                    meshVertices.Add(GetVertice(0, 1, 1));
                    break;

                case Direction.North:
                    meshVertices.Add(GetVertice(0, 0, 0));
                    meshVertices.Add(GetVertice(0, 1, 0));
                    meshVertices.Add(GetVertice(1, 1, 0));
                    meshVertices.Add(GetVertice(1, 0, 0));
                    break;
            }
            meshTriangles.Add(verticesCountBase + 0);
            meshTriangles.Add(verticesCountBase + 1);
            meshTriangles.Add(verticesCountBase + 2);

            meshTriangles.Add(verticesCountBase + 0);
            meshTriangles.Add(verticesCountBase + 2);
            meshTriangles.Add(verticesCountBase + 3);

            AddTexture(direction, meshUVs);
        }
    }

    public class Saver
    {
        Saver(string str)
        {
            currentSavePath = str;
        }
        string currentSavePath;

        void LoadChunkShift(FileStream file, int x, int y, int z, out long shift, out long length)
        {
            file.Seek(((x) * WorldYSize + y) * WorldZSize + z, SeekOrigin.Begin);
            byte[] data = new byte[16];
            file.Read(data, 0, 16);
            shift = System.BitConverter.ToInt64(data, 0);
            length = System.BitConverter.ToInt64(data, 8);
        }

        void SaveChunkShift(FileStream file, int x, int y, int z, long shift, long length)
        {
            file.Seek(((x) * WorldYSize + y) * WorldZSize + z, SeekOrigin.Begin);
            file.Write(System.BitConverter.GetBytes(shift), 0, 8);
            file.Write(System.BitConverter.GetBytes(length), 0, 8);
        }

        public void Save(Chunk chunk)
        {
            if (chunk != null)
            {
                FileStream file = new FileStream(currentSavePath, FileMode.Append, FileAccess.Write);
                byte[] data = chunk.SaveToBytes();
                SaveChunkShift(file, chunk.x, chunk.y, chunk.z, file.Length, data.Length);
                file.Seek(0, SeekOrigin.End);
                file.Write(data, 0, data.Length);
                file.Close();
            }
        }

        public void Load(Chunk chunk)
        {
            if (chunk != null)
            {
                FileStream file = new FileStream(currentSavePath, FileMode.Open, FileAccess.Read);
                long shift, length;
                LoadChunkShift(file, chunk.x, chunk.y, chunk.z, out shift, out length);
                byte[] data = new byte[length];
                file.Seek(shift, SeekOrigin.Begin);
                file.Read(data, 0, (int)length);
                chunk.LoadFromBytes(data);
                file.Close();
            }
        }
    }

    public const int ChunkXSize = 16;
    public const int ChunkYSize = 16;
    public const int ChunkZSize = 16;

    public const float BlockXShift = .5f;
    public const float BlockYShift = .5f;
    public const float BlockZShift = .5f;

    public const float ChunkXShift = ChunkXSize * BlockXShift;
    public const float ChunkYShift = ChunkYSize * BlockYShift;
    public const float ChunkZShift = ChunkZSize * BlockZShift;

    public static int WorldXSize = 256;
    public static int WorldYSize = 16;
    public static int WorldZSize = 256;

    static Chunk[,,] chunkMap = new Chunk[WorldXSize, WorldYSize, WorldZSize];

    int x = 0, y = 0, z = 0;
    Block[,,] blockMap = new Block[ChunkXSize, ChunkYSize, ChunkZSize];

    Mesh meshFilter;
    Vector3 basePos;

    void UpdateBlock(byte x, byte y, byte z, Block.BlockType type)
    {
        blockMap[x, y, z] = new Block(this, x, y, z, type);
    }

    void UpdateMesh()
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int i = 0; i < ChunkXSize; i++)
        {
            for (int j = 0; j < ChunkYSize; j++)
            {
                for (int k = 0; k < ChunkZSize; k++)
                {
                    if (blockMap[i, j, k] != null)
                    {
                        blockMap[i, j, k].MeshUpdate(verts, tris, uvs);
                    }
                }
            }
        }
        meshFilter.vertices = verts.ToArray();
        meshFilter.triangles = tris.ToArray();
        meshFilter.uv = uvs.ToArray();
    }

    byte[] SaveToBytes()
    {
        List<byte> res = new List<byte>();
        for (int i = 0; i < ChunkXSize; i++)
        {
            for (int j = 0; j < ChunkYSize; j++)
            {
                for (int k = 0; k < ChunkZSize; k++)
                {
                    if (blockMap[i, j, k] == null)
                        res.Add(0);
                    else
                        res.Add((byte)blockMap[i, j, k].type);
                }
            }
        }
        return res.ToArray();
    }

    void LoadFromBytes(byte[] s)
    {
        int nowI = 0;
        for (int i = 0; i < ChunkXSize; i++)
        {
            for (int j = 0; j < ChunkYSize; j++)
            {
                for (int k = 0; k < ChunkZSize; k++)
                {
                    if (s[nowI] != 0)
                        blockMap[i, j, k] = new Block(this, (byte)i, (byte)j, (byte)k, (Block.BlockType)s[nowI]);
                    else
                        blockMap[i, j, k] = null;
                }
            }
        }
    }

    // TODO
    // Use this for initialization
    void Start () {
        chunkMap[x, y, z] = this;

        basePos = GetComponent<Transform>().transform.position;
        meshFilter = GetComponent<MeshFilter>().mesh;
        for (int i = 0; i < ChunkXSize; i++)
        {
            for (int j = 0; j < ChunkZSize; j++)
            {
                if (Random.Range(0, 2) < 1)
                    UpdateBlock((byte)i, 0, (byte)j, Block.BlockType.Grass);
                else
                    UpdateBlock((byte)i, 0, (byte)j, Block.BlockType.Dirt);
            }
        }
        UpdateMesh();
    }

    // TODO
    // Update is called once per frame
    void Update () {
    }
}
*/