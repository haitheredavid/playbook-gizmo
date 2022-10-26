using System;
using HaiThere.Playbook;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoverTester : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

	[SerializeField] GameObject obj;
	[SerializeField] Camera cam;
	[SerializeField] float speed = 100f;
	
	[SerializeField] AxisType axis;

	[Header("Local Position Test ")]
	[SerializeField] bool useLocalPosForDelta;
	[SerializeField] bool useLocalForAll;
	[SerializeField] bool logPos;

	[SerializeField] LineRenderer lr;

	Material objMaterial;
	Vector3 positionDelta;
	Vector3 objDelta;
	Vector3 newPos;

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

	void Start()
	{
		objMaterial = gameObject.GetComponent<MeshRenderer>().material;
		objMaterial.color = Color.grey;

		lr.enabled = false;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.forward);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.up);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
	}

	public void OnBeginDrag(PointerEventData data)
	{
		lr.enabled = true;
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

	public void OnDrag(PointerEventData data)
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
			result = Vector3.MoveTowards(transform.localPosition, newPos, speed * Time.deltaTime);
			transform.localPosition = result;

			if (logPos)
				Debug.Log("Dragging Local-" + transform.localPosition);
		}
		else
		{
			result = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
			transform.position = result;

			if (logPos)
				Debug.Log("Dragging-" + transform.position);
		}

		obj.transform.position = result + objDelta;
	}

	public void OnEndDrag(PointerEventData data)
	{
		lr.enabled = false;
		objMaterial.color = Color.gray;

		if (useLocalForAll && logPos)
			Debug.Log("Ending Local-" + transform.localPosition);
		else if (logPos)
			Debug.Log("Ending-" + transform.position);
	}

}