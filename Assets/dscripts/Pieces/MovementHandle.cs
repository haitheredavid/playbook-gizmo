using System;
using System.Collections;
using System.Collections.Generic;
using HaiThere.Playbook;
using UnityEngine;

public interface IGizmoParent
{ }

public class MovementHandle : MonoBehaviour, IGizmoParent
{
	[Header("Gizmo Props")]
	[SerializeField] PlayBookGizmo parent;
	// TODO: Replace by getting value from parent
	[SerializeField] Camera tempCam;
	[SerializeField] AxisType currentAxis;

	[Header("Object Props")]
	[SerializeField] Mesh mesh;
	[SerializeField, Range(0.0f, 1.0f)] float positionOffset = 0.5f;
	[SerializeField, Range(0.0f, 1.0f)] float scaleOffset = 0.5f;

	[Header("Animations Props")]
	[SerializeField, Range(1f, 100f)] float movementSpeed = 100f;
	[SerializeField, Range(0.01f, 10f)] float lineAnimationLength = 0.1f;
	[SerializeField] float lineLength = 20f;

	[Header("Debug Position")]
	[SerializeField] bool useLocalPosForDelta;
	[SerializeField] bool useLocalForAll;
	[SerializeField] bool logPos;

	List<GizmoPieceMove2> pieces = new List<GizmoPieceMove2>();

	public void Start()
	{
		Initialize();
	}

	public void Initialize()
	{
		var prefab = new GameObject().AddComponent<GizmoPieceMove2>();

		prefab.GetComponent<MeshFilter>().mesh = mesh;
		prefab.GetComponent<MeshCollider>().sharedMesh = prefab.GetComponent<MeshFilter>().mesh;
		prefab.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Gizmo");

		// hard coded size since its just using a cube but this should change for a model later
		prefab.transform.localScale = new Vector3(.2f, .2f, .5f);
		prefab.Parent = this;

		foreach (AxisType axis in Enum.GetValues(typeof(AxisType)))
		{
			var instance = Instantiate(prefab, transform);
			instance.name = $"Mover_{axis}";
			instance.transform.localPosition = GetPiecePosition(axis);
			instance.transform.localRotation = Quaternion.Euler(GetPieceRotation(axis));
			instance.transform.SetParent(transform);
			instance.Create(axis);

			pieces.Add(instance);
		}

		Destroy(prefab.gameObject);
	}

	Vector3 GetPiecePosition(AxisType axis)
	{
		var offset = Mathf.Max(positionOffset, scaleOffset * 0.5f);
		return axis switch
		{
			AxisType.X => new Vector3(offset, 0, 0),
			AxisType.Y => new Vector3(0, offset, 0),
			AxisType.Z => new Vector3(0, 0, offset),
			_ => Vector3.zero
		};
	}

	Vector3 GetPieceRotation(AxisType axis)
	{
		return axis switch
		{
			AxisType.X => new Vector3(0, -90, -90),
			AxisType.Y => new Vector3(90, 0, 0),
			AxisType.Z => new Vector3(0, 0, -90),
			_ => Vector3.zero
		};
	}

}