using Model;
using System;
using UnityEngine;

public class Tiles : MonoBehaviour {

    //10칸짜리
    public int width = 10;
    public int length = 10;
    //c# 문법 2차원 배열을 뜻함
    private Tile[,] tiles;
    private int autoIncrement = 1;

    void Start()
    {
        tiles = new Tile[width, length];

        // generate a grid.
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                tiles[x,z] = GenerateTile(x, z);
            }
        }
    }

    private Tile GenerateTile(int x, int z)
    {
        Mesh mesh = new Mesh();
        //vertices = 꼭지점 만들기
        mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0) };
        //triangles = 삼각형으로 만들어 4각형을 만듬
        mesh.triangles = new int[] { 1, 2, 3, 0, 1, 3 };

        GameObject obj = new GameObject("Tile"+ autoIncrement++);
        obj.AddComponent<MeshFilter>().mesh = mesh;
        obj.AddComponent<MeshCollider>();
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localPosition = new Vector3(x, 0, z);

        var tile = obj.AddComponent<Tile>();
        tile.Set(x, z);

        return tile;
    }


    public Tile GetTileByCoordinate(int x, int z)
    {
        if (x < 0 || z < 0)
            return null;
        if (x >= width || z >= length)
            return null;

        return tiles[x, z];
    }

    public Tile GetTileByPoint(Vector3 point)
    {
        int z = (int)Math.Round((point.z / 2f) + (point.x / 4f));
        int x = (int)Math.Round((point.z / 2f) - (point.x / 4f));
        
        if (x < 0 || x >= width)
            return null;

        if (z < 0 || z >= length)
            return null;

        return tiles[x, z];
    }
}
