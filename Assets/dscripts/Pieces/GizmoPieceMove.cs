using System;
using System.Collections;
using HaiThere.Playbook;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GizmoPieceMove : GizmoPiece, IPointerClickHandler
{
	public AxisType axis { get; set; }

	public float movementSpeed { get; set; } = 100f;

	public bool showLine { get; set; } = true;

	public bool isLocal { get; set; } = true;

	public GizmoComponent parent { get; set; }

	public Transform obj { get; set; }
	LineRenderer lr;
	Vector3 positionDelta;
	Vector3 objDelta;
	Vector3 newPos;

	public event UnityAction OnStart;

	public event UnityAction OnEnd;

	public event UnityAction<GizmoPieceMove> OnClick;

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
	

	protected override void SetupPiece()
	{
		material.color = PlaybookColors.GetAxisColor(axis);

		lr = gameObject.AddComponent<LineRenderer>();
		lr.enabled = showLine;
		lr.loop = false;
		lr.endWidth = lr.startWidth = 0.02f;
		lr.material = Resources.Load<Material>("Line");
		lr.material.color = PlaybookColors.GetAxisColor(axis);
		lr.SetPositions(new[] { Vector3.zero, Vector3.zero });
	}

	public override void OnBeginDrag(PointerEventData data)
	{
		positionDelta = GetPosition(data.pointerPressRaycast.worldPosition, isLocal ? transform.localPosition : transform.position);

		if (obj != null)
			objDelta = obj.position - (isLocal ? transform.localPosition : transform.position);

		OnStart?.Invoke();
		OnClick?.Invoke(this);

		if (showLine)
			lr.SetPosition(0, transform.position);

		Debug.Log($"{name}-Start");
	}

	public override void OnDrag(PointerEventData data)
	{
		if (data.pointerCurrentRaycast.worldPosition.x == 0.0f || data.pointerCurrentRaycast.worldPosition.y == 0.0f)
		{
			newPos = GetPosition(parent.pointerPos, positionDelta);
		}
		else
		{
			newPos = GetPosition(data.pointerCurrentRaycast.worldPosition, positionDelta);
		}

		Vector3 result;
		if (isLocal)
		{
			result = Vector3.MoveTowards(transform.localPosition, newPos, movementSpeed * Time.deltaTime);
			transform.localPosition = result;
		}
		else
		{
			result = Vector3.MoveTowards(transform.position, newPos, movementSpeed * Time.deltaTime);
			transform.position = result;
		}

		if (obj != null)
			obj.transform.position = result + objDelta;

		if (showLine)
			lr.SetPosition(1, transform.position);

		Debug.Log($"{name}-Drag");
	}

	public override void OnEndDrag(PointerEventData data)
	{
		if (showLine)
		{
			lr.SetPosition(0, transform.position);
		}

		Debug.Log($"{name}-End");
		OnEnd?.Invoke();
	}

	public bool Equals(GizmoPieceMove value)
	{
		return value != null && value.axis == axis;
	}

	public void OnPointerClick(PointerEventData data)
	{
		Debug.Log($"{name}-Click");
		OnClick?.Invoke(this);
	}

	
}