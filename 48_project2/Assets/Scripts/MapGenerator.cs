using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
	// public Transform tilePrefab;
	// public Vector2 mapSize;

	// [Range(0,1)]
	// public float outlinePercentage;

	// void Start() {
	//     GenerateMap();
	// }
	// public void GenerateMap(){

	//     string holderName = "Generated Map";
	//     if(transform.Find(holderName)) {
	//         DestroyImmediate(transform.Find(holderName).gameObject);
	//     }
	//     Transform mapHolder = new GameObject(holderName).transform;
	//     mapHolder.parent = transform;

	//     for(int x = 0; x< mapSize.x; x++){
	//         for (int y = 0; y < mapSize.y; y++)
	//         {
	//             Vector3 tilePosition = new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
	//             Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
	//             newTile.localScale = Vector3.one * (1 - outlinePercentage);
	//             newTile.parent = mapHolder;
	//         }
	//     }
	// }
	Mesh mesh;
	Vector3[] vertices;
	int[] triangles;
	public int xSize = 20;
	public int zSize = 20;


	void Awake()
	{

		mesh = new Mesh();

		GetComponent<MeshFilter>().mesh = mesh;

		CreateShape();
		UpdateMesh();
	}

	public void CreateShape()
	{
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];

		for (int i = 0, z = 0; z <= zSize; z++)
		{
			for (int x = 0; x <= xSize; x++)
			{
				float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
				vertices[i] = new Vector3(x, y, z);
				i++;
			}
		}
		triangles = new int[xSize * zSize * 6];
		int vert = 0;
		int tris = 0;
		for (int z = 0; z < zSize; z++)
		{
			for (int x = 0; x < xSize; x++)
			{
				triangles[tris + 0] = vert + 0;
				triangles[tris + 1] = vert + xSize + 1;
				triangles[tris + 2] = vert + 1;
				triangles[tris + 3] = vert + 1;
				triangles[tris + 4] = vert + xSize + 1;
				triangles[tris + 5] = vert + xSize + 2;

				vert++;
				tris += 6;

			}
			vert++;
		}

	}
	void UpdateMesh()
	{
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
	private void OnDrawGizmos()
	{
		if (vertices == null)
		{
			return;
		}
		for (int i = 0; i < vertices.Length; i++)
		{
			Gizmos.DrawSphere(vertices[i], 0.1f);
		}
	}
}
