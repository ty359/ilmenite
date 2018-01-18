using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Block
{
    public int blockID;
    public static Block NewAirBlock()
    {
        return new Block { blockID = 0 };
    }
    public static Block NewDirtBlock()
    {
        return new Block { blockID = 1 };
    }
    public Vector2[] GetBlockFaceUV(int side)
    {
        float uvBase = 64f / 2048f;
        if (blockID == 1)
            return new Vector2[] {
                new Vector2(0, 0),
                new Vector2(uvBase, 0),
                new Vector2(uvBase, uvBase),
                new Vector2(0, uvBase)
            };
        else
            return null;
    }

    public bool IsSolid()
    {
        if (blockID != 0)
            return true;
        else
            return false;
    }

    // 绘制position为中心的正方体的面
    public static void BuildMeshFace(MeshList meshList, Vector3 position, Quaternion rotation, Vector2[] uv)
    {
        int meshSize = meshList.VCount();
        // 从上往下看
        // 左下
        meshList.vertices.Add(rotation * new Vector3(-.5f, -.5f, -.5f) + position);
        meshList.uv.Add(uv[0]);
        // 右下
        meshList.vertices.Add(rotation * new Vector3( .5f, -.5f, -.5f) + position);
        meshList.uv.Add(uv[1]);
        // 右上
        meshList.vertices.Add(rotation * new Vector3( .5f,  .5f, -.5f) + position);
        meshList.uv.Add(uv[2]);
        // 左上
        meshList.vertices.Add(rotation * new Vector3(-.5f,  .5f, -.5f) + position);
        meshList.uv.Add(uv[3]);

        meshList.triangles.Add(meshSize);
        meshList.triangles.Add(meshSize + 3);
        meshList.triangles.Add(meshSize + 1);

        meshList.triangles.Add(meshSize + 1);
        meshList.triangles.Add(meshSize + 3);
        meshList.triangles.Add(meshSize + 2);
    }
}
