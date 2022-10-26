using System;
using System.Collections;
using HaiThere.Playbook;
using UnityEngine;
using UnityEngine.EventSystems;

public class GizmoPieceMove2 : GizmoPiece
{
	[SerializeField] GameObject obj;
	[SerializeField] Camera cam;

	[SerializeField] AxisType axis;

	[Header("Positioning Props")]
	[SerializeField] bool useLocalPosForDelta;
	[SerializeField] bool useLocalForAll;
	[SerializeField] bool logPos;

	[Header("Animations Props")]
	[SerializeField, Range(1f, 100f)] float movementSpeed = 100f;
	[SerializeField, Range(0.01f, 10f)] float lineAnimationLength = 0.1f;
	[SerializeField] float lineLength = 20f;

	[SerializeField] LineRenderer lr;
	[SerializeField] bool showLine;

	Material objMaterial;
	Vector3 positionDelta;
	Vector3 objDelta;
	Vector3 newPos;

	Coroutine lineAnimation;

	public IGizmoParent Parent { get; set; }

	Vector3 GetPosition(Vector3 pos, Vector3 offset)
	{
		return axis switch
		{
			AxisType.X => new Vector3(pos.x - offset.x, positionDelta.y, positionDelta.z),
			AxisType.Y => new Vector3(positionDelta.x, pos.y - offset.y, positionDelta.z),
			AxisType.Z => new Vector3(positionDelta.x, positionDelta.y, pos.z - offset.z),
			_ => pos - offset
		};
	}

	public void Create(AxisType axisType)
	{
		axis = axisType;
		Create();
	}

	public override void Create()
	{
		objMaterial = gameObject.GetComponent<MeshRenderer>().material;
		objMaterial.color = PlaybookColors.SetInactive(PlaybookColors.GetAxisColor(axis));

		lr = gameObject.AddComponent<LineRenderer>();
		lr.SetPositions(new[] { Vector3.zero, Vector3.zero });
		lr.material = Resources.Load<Material>("Line");
		lr.material.color = PlaybookColors.GetAxisColor(axis);
		lr.enabled = false;
	}

	public override void OnBeginDrag(PointerEventData data)
	{
		if (showLine)
		{
			lr.enabled = true;

			if (lineAnimation != null)
			{
				StopCoroutine(lineAnimation);
			}

			lineAnimation = StartCoroutine(LineDraw(Vector3.zero, Vector3.zero, new Vector3(0, 0, lineLength), new Vector3(0, 0, lineLength * -1)));
		}
		else
		{
			lr.enabled = false;
		}

		objMaterial.color = Color.yellow;
		lr.material.color = Color.yellow;

		if (useLocalPosForDelta || useLocalForAll)
		{
			positionDelta = GetPosition(data.pointerPressRaycast.worldPosition, transform.localPosition);
			objDelta = obj.transform.position - transform.localPosition;

			if (logPos)
				Debug.Log("Starting Local-" + transform.localPosition);
		}
		else
		{
			positionDelta = GetPosition(data.pointerPressRaycast.worldPosition, transform.position);
			objDelta = obj.transform.position - transform.position;

			if (logPos)
				Debug.Log("Starting-" + transform.position);
		}
	}

	public override void OnDrag(PointerEventData data)
	{
		if (data.pointerCurrentRaycast.worldPosition.x == 0.0f || data.pointerCurrentRaycast.worldPosition.y == 0.0f)
		{
			if (logPos)
				Debug.Log("Position from Mouse");

			newPos = GetPosition(cam.ScreenToWorldPoint(Input.mousePosition), positionDelta);
		}
		else
		{
			if (logPos)
				Debug.Log("Position from Event");

			newPos = GetPosition(data.pointerCurrentRaycast.worldPosition, positionDelta);
		}

		Vector3 result;
		if (useLocalForAll)
		{
			result = Vector3.MoveTowards(transform.localPosition, newPos, movementSpeed * Time.deltaTime);
			transform.localPosition = result;

			if (logPos)
				Debug.Log("Dragging Local-" + transform.localPosition);
		}
		else
		{
			result = Vector3.MoveTowards(transform.position, newPos, movementSpeed * Time.deltaTime);
			transform.position = result;

			if (logPos)
				Debug.Log("Dragging-" + transform.position);
		}

		obj.transform.position = result + objDelta;
	}

	public override void OnEndDrag(PointerEventData data)
	{
		if (showLine)
		{
			if (lineAnimation != null)
			{
				StopCoroutine(lineAnimation);
			}

			lineAnimation = StartCoroutine(LineDraw(lr.GetPosition(0), lr.GetPosition(1), Vector3.zero, Vector3.zero));
		}
		else
		{
			lr.enabled = false;
		}

		objMaterial.color = Color.gray;

		if (useLocalForAll && logPos)
			Debug.Log("Ending Local-" + transform.localPosition);
		else if (logPos)
			Debug.Log("Ending-" + transform.position);
	}

	IEnumerator LineDraw(Vector3 startPosA, Vector3 startPosB, Vector3 endPosA, Vector3 endPosB)
	{
		lr.SetPosition(0, startPosA);
		lr.SetPosition(1, startPosB);

		for (var t = 0f; t < lineAnimationLength; t += Time.deltaTime)
		{
			var step = Mathf.SmoothStep(0, 1, t / lineAnimationLength);
			lr.SetPosition(0, Vector3.Lerp(Vector3.zero, endPosA, step));
			lr.SetPosition(1, Vector3.Lerp(Vector3.zero, endPosB, step));
			yield return null;
		}

		lr.SetPosition(0, endPosA);
		lr.SetPosition(0, endPosA);
	}
}