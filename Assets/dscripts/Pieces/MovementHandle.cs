using System;
using System.Collections;
using System.Collections.Generic;
using HaiThere.Playbook;
using UnityEngine;

public interface IGizmoParent
{
	public void Attach(GameObject obj);

	Vector3 pointerPos { get; }
}

public class MovementHandle : MonoBehaviour, IGizmoParent
{
	[Header("Gizmo Props")]
	[SerializeField] PlayBookGizmo parent;
	[SerializeField] GameObject tempObj;
	// TODO: Replace by getting value from parent
	[SerializeField] Camera tempCam;

	[Header("Object Props")]
	[SerializeField] Mesh mesh;
	[SerializeField, Range(0.0f, 1.0f)] float positionOffset = 0.5f;
	[SerializeField, Range(0.0f, 1.0f)] float scaleOffset = 0.5f;

	[Header("Animations Props")]
	[SerializeField, Range(1f, 100f)] float movementSpeed = 100f;
	[SerializeField] bool showLine = true;

	[Header("Debug Position")]
	[SerializeField] bool useLocalPos = true;
	[SerializeField] bool logPos;

	public GizmoPieceMove2 activePiece { get; private set; }

	public AxisType activeAxis => activePiece == null ? AxisType.X : activePiece.axis;

	List<GizmoPieceMove2> pieces = new List<GizmoPieceMove2>();

	public GameObject activeObj { get; private set; }

	public void Start()
	{
		Initialize();
		Attach(tempObj);
	}

	public void Initialize()
	{
		var prefab = new GameObject().AddComponent<GizmoPieceMove2>();

		prefab.GetComponent<MeshFilter>().mesh = mesh;
		prefab.GetComponent<MeshCollider>().sharedMesh = prefab.GetComponent<MeshFilter>().mesh;
		prefab.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Gizmo");

		prefab.showLine = showLine;
		prefab.isLocal = useLocalPos;
		prefab.movementSpeed = movementSpeed;

		// hard coded size since its just using a cube but this should change for a model later
		prefab.transform.localScale = new Vector3(.2f, .2f, .5f);

		foreach (AxisType axis in Enum.GetValues(typeof(AxisType)))
		{
			var instance = Instantiate(prefab, transform);
			instance.name = $"Mover_{axis}";
			instance.transform.localPosition = GetPiecePosition(axis);
			instance.transform.localRotation = Quaternion.Euler(GetPieceRotation(axis));
			instance.axis = axis;
			instance.parent = this;

			instance.OnClick += SetActivePiece;

			instance.Create();

			pieces.Add(instance);
		}

		Destroy(prefab.gameObject);
	}

	void SetActivePiece(GizmoPieceMove2 piece)
	{
		if (piece == null)
			return;

		if (activePiece != null)
		{
			if (activePiece.Equals(piece))
				return;

			activePiece.OnStart -= StartActionHook;
			activePiece.OnEnd -= EndActionHook;
		}

		activePiece = piece;
		activePiece.OnStart += StartActionHook;
		activePiece.OnEnd += EndActionHook;
	}

	void StartActionHook()
	{ }

	void EndActionHook()
	{
		activePiece.transform.localPosition = GetPiecePosition(activeAxis);

		transform.position = new Vector3(
			activeObj.transform.position.x + activeObj.transform.lossyScale.x,
			activeObj.transform.position.y - activeObj.transform.lossyScale.y,
			activeObj.transform.position.z - activeObj.transform.lossyScale.z
		);
	}

	public void Attach(GameObject obj)
	{
		activeObj = obj;
		
		foreach (var piece in pieces)
		{
			piece.Attach(obj.transform);
		}
	}

	public Vector3 pointerPos
	{
		get => tempCam == null ? Vector3.zero : tempCam.ScreenToWorldPoint(Input.mousePosition);
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