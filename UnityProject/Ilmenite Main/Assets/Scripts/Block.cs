using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public static float sideLength = 0.5f;
    public static float halfSideLength = sideLength / 2;

    public static Dictionary <int, Vector3> sides = new Dictionary <int, Vector3>
    {
        { 0, Vector3.right },
        { 1, Vector3.up },
        { 2, Vector3.forward },
        { 3, Vector3.left },
        { 4, Vector3.down },
        { 5, Vector3.back },
    };

    BlockProtoType protoType;
    BlockComponent component;

    public void RenderAt(ChunkRenderHandle handle, Vector3 position, BlockVisibleStatus visibleStatus)
    {
        protoType.RenderAt(handle, position, component, visibleStatus);
    }

    public static Block CreateBlockByID(int id)
    {
        return new Block
        {
            protoType = BlockProtoType.GetProtoTypeByID(id),
            component = null
        };
    } 
}

public abstract class BlockProtoType
{
    abstract public void RenderAt(ChunkRenderHandle handle, Vector3 position, BlockComponent component, BlockVisibleStatus visibleStatus);

    abstract public int GetBlockID();

    abstract public bool IsSolid();

    const int blockProtoTypePoolSize = 10000;
    internal static BlockProtoType[] blockProtoTypePool = new BlockProtoType[blockProtoTypePoolSize];

    public static BlockProtoType GetProtoTypeByID(int id)
    {
        return blockProtoTypePool[id];
    }
}

public class BlockComponent
{
}

public class BlockVisibleStatus
{
    byte FaceVisibleStatus;

    public static BlockVisibleStatus AllVisible = new BlockVisibleStatus();
    BlockVisibleStatus()
    {
        this.FaceVisibleStatus = (1 << 8) - 1;
    }

    public bool Visible()
    {
        return FaceVisibleStatus != 0;
    }

    public bool FaceVisible(int side)
    {
        return (FaceVisibleStatus >> side & 1) != 0;
    }
}

internal class SolidBlockProtoType : BlockProtoType
{
    int id;
    // 六个面的素材坐标
    internal Vector2[][] uvs;

    public override bool IsSolid()
    {
        return true;
    }

    public override void RenderAt(ChunkRenderHandle handle, Vector3 position, BlockComponent component, BlockVisibleStatus visibleStatus)
    {
        if (!visibleStatus.Visible())
            return;

        // TODO
        // 未验证的问题：为什么改为 Vector(1, 0, 0)会出BUG
        Vector3 baseDirc = new Vector3(0, 0, 1);

        foreach (var side in Block.sides)
        {
            if (visibleStatus.FaceVisible(side.Key))
            {
                BuildMeshFace(
                    handle.meshList,
                    position,
                    Quaternion.FromToRotation(baseDirc, side.Value),
                    uvs[side.Key]);
            }
        }
    }

    // 绘制position为中心的正方体的面
    static void BuildMeshFace(ChunkRenderHandle.MeshList meshList, Vector3 position, Quaternion rotation, Vector2[] uv)
    {
        int meshSize = meshList.VCount();
        // 从上往下看
        // 左下
        meshList.vertices.Add(rotation * new Vector3(-Block.halfSideLength, -Block.halfSideLength, -Block.halfSideLength) + position);
        meshList.uv.Add(uv[0]);
        // 右下
        meshList.vertices.Add(rotation * new Vector3(Block.halfSideLength, -Block.halfSideLength, -Block.halfSideLength) + position);
        meshList.uv.Add(uv[1]);
        // 右上
        meshList.vertices.Add(rotation * new Vector3(Block.halfSideLength, Block.halfSideLength, -Block.halfSideLength) + position);
        meshList.uv.Add(uv[2]);
        // 左上
        meshList.vertices.Add(rotation * new Vector3(-Block.halfSideLength, Block.halfSideLength, -Block.halfSideLength) + position);
        meshList.uv.Add(uv[3]);

        meshList.triangles.Add(meshSize);
        meshList.triangles.Add(meshSize + 3);
        meshList.triangles.Add(meshSize + 1);

        meshList.triangles.Add(meshSize + 1);
        meshList.triangles.Add(meshSize + 3);
        meshList.triangles.Add(meshSize + 2);
    }

    public override int GetBlockID()
    {
        return id;
    }

    internal SolidBlockProtoType(int id, Vector2[] uv)
    {
        this.id = id;
        this.uvs = new Vector2[][] {
            uv,uv,uv,uv,uv,uv
        };
    }
}

public static class BlockProtoTypeAssigner
{
    public static void Assign()
    {
        BlockProtoType.blockProtoTypePool[1] = 
            new SolidBlockProtoType(1, new Vector2[] {
                new Vector2(0, 0),
                new Vector2(32f/2048, 0),
                new Vector2(32f/2048,32f/2048),
                new Vector2(0,32f/2048)
            });
    }
}