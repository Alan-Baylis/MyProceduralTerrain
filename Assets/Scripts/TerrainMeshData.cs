using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainMeshData {

    List<int> tris = new List<int>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    Dictionary<Vector3, int> vertices = new Dictionary<Vector3, int>();



    public int AddVert(Vector3 vert, int chunkSize, Vector3 OriginCenter)
    {
        int indice = 0;
        if (vertices.TryGetValue(vert, out indice))
        {
            return indice;
        }
        else
        {
            vertices.Add(vert, vertices.Count);
            Vector3 uvVec = vert - OriginCenter;
            uvVec.x = (uvVec.x + chunkSize / 2) / chunkSize;
            uvVec.z = (uvVec.z + chunkSize / 2) / chunkSize;
            uvs.Add(new Vector2(uvVec.x, uvVec.z));

            return vertices.Count - 1;
        }

    }

    public void AddTri(int index0, int index1, int index2)
    {
        tris.Add(index0);
        tris.Add(index1);
        tris.Add(index2);
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.Keys.ToArray<Vector3>();
        mesh.triangles = tris.ToArray();
        Debug.Log(tris.Count / 3 + " tris, " + vertices.Count + " verts");
        if (uvs.Count == vertices.Count)
        {
            mesh.uv = uvs.ToArray();
        }
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();


        return mesh;
    }
}
