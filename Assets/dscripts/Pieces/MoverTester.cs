using HaiThere.Playbook;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoverTester : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

	[SerializeField] GameObject obj;
	[SerializeField] Camera cam;
	[SerializeField] float speed = 100f;

	[SerializeField] bool useDragHandler;

	Material objMaterial;
	Vector3 positionDelta;
	Vector3 objDelta;
	Vector3 newPos;

	Vector3 GetLockedMousePos(AxisType axisType = AxisType.Z)
	{
		var mp = cam.ScreenToWorldPoint(Input.mousePosition);
		switch (axisType)
		{
			case AxisType.X:
				mp.x = positionDelta.x;
				break;
			case AxisType.Y:
				mp.y = positionDelta.y;
				break;
			case AxisType.Z:
				mp.z = positionDelta.z;
				break;
		}

		return mp;
	}

	void Start()
	{
		objMaterial = gameObject.GetComponent<MeshRenderer>().material;
		objMaterial.color = Color.grey;
	}

	void OnMouseEnter()
	{
		if (useDragHandler)
			return;

		objMaterial.color = Color.red;
	}

	void OnMouseDown()
	{
		if (useDragHandler)
			return;

		objMaterial.color = Color.yellow;
		positionDelta = transform.position - GetLockedMousePos();
	}

	void OnMouseDrag()
	{
		if (useDragHandler)
			return;

		newPos = GetLockedMousePos();
		transform.position = Vector3.MoveTowards(
			transform.position,
			new Vector3(newPos.x + positionDelta.x, newPos.y + positionDelta.y, positionDelta.z),
			speed * Time.deltaTime
		);
	}

	void OnMouseExit()
	{
		if (useDragHandler)
			return;

		objMaterial.color = Color.grey;
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
		if (!useDragHandler)
			return;

		objMaterial.color = Color.yellow;
		positionDelta = data.pointerPressRaycast.worldPosition - transform.position;
		objDelta = obj.transform.position - transform.position;
	}

	public void OnDrag(PointerEventData data)
	{
		if (!useDragHandler)
			return;

		if (data.pointerCurrentRaycast.worldPosition.x == 0.0f || data.pointerCurrentRaycast.worldPosition.y == 0.0f)
		{
			newPos = GetLockedMousePos();
		}
		else
		{
			newPos = data.pointerCurrentRaycast.worldPosition - positionDelta;
		}

		var result = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
		transform.position = result;
		obj.transform.position = result + objDelta;
	}

	public void OnEndDrag(PointerEventData data)
	{
		if (!useDragHandler)
			return;

		objMaterial.color = Color.gray;
	}

}