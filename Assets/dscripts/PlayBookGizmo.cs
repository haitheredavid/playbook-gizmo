using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object for creating the editing functionality of a mesh at runtime
/// </summary>
public class PlayBookGizmo : MonoBehaviour
{

	MeshFilter filter;
	[SerializeField, Range(1, 100)] int sides = 10;
	[SerializeField, Range(0.01f, 10f)] float outerRadius = 5f;
	[SerializeField, Range(0.01f, 10f)] float innerRadius = 2f;

	[SerializeField] List<Vector3> points;
	[SerializeField] List<int> triangles;

	public void Start()
	{
		filter = gameObject.GetComponent<MeshFilter>();
		CreateRing(filter);
	}

	void OnValidate()
	{
		CreateRing(filter);
	}

	void CreateRing(MeshFilter mf)
	{
		if (!ObjectValid(mf))
			return;

		points = new List<Vector3>();
		points.AddRange(PointRing(mf.transform.position, sides, outerRadius));
		points.AddRange(PointRing(mf.transform.position, sides, innerRadius));

		var triSides = points.Count / 2;
		triangles = new List<int>();

		for (int index = 0; index < triSides; index++)
		{
			triangles.Add(index);
			triangles.Add(index + triSides);
			triangles.Add((index + 1) % triSides);

			triangles.Add(index);
			triangles.Add(triSides + (triSides + index - 1) % triSides);
			triangles.Add(index + triSides);
		}

		if (!ObjectValid(mf.mesh))
		{
			mf.mesh = new Mesh();
		}

		mf.mesh.Clear();
		mf.mesh.SetVertices(points);
		mf.mesh.SetTriangles(triangles, 0);
	}

	public static IEnumerable<Vector3> PointRing(Vector3 center, int sides, float radius)
	{
		const float T = 2 * Mathf.PI;
		var points = new List<Vector3>();
		var circSteps = 1f / sides;
		var radianSteps = circSteps * T;

		for (var i = 0; i < sides; i++)
		{
			var radian = radianSteps * i;
			points.Add(center
			           + new Vector3(Mathf.Cos(radian) * radius,
			                         Mathf.Sin(radian) * radius,
			                         0)
			);
		}

		return points;
	}

	static bool ObjectValid<TObj>(TObj mesh) where TObj : UnityEngine.Object
	{
		if (mesh == null)
		{
			Debug.LogWarning($"Invalid {typeof(TObj)} passed");
			return false;
		}

		return true;
	}

}