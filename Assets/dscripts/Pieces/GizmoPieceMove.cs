using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HaiThere.Playbook
{
	public class GizmoPieceMove : GizmoPiece
	{

		Vector3 prevPos = Vector3.zero;

		public AxisType Axis { get; set; }

		const float offset = 1.3f;

		float GetOffset()
		{
			if (Obj == null)
				return offset;

			var size = Obj.objectBounds.size;

			return Mathf.Max(
				       size.x * (Obj.transform.localScale.x * 0.8f),
				       size.y * (Obj.transform.localScale.y * 0.8f),
				       size.z * (Obj.transform.localScale.z * 0.8f)
			       )
			       + offset;
		}

		public override void Create()
		{
			if (meshFilter.mesh == null && meshFilter.sharedMesh == null)
			{
				transform.localScale = Vector3.one;
				var prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
				if (Application.isPlaying)
				{
					meshFilter.mesh = Instantiate(prefab.GetComponent<MeshFilter>().mesh);
					meshCollider.sharedMesh = meshFilter.mesh;
					Destroy(prefab);
				}
				else
				{
					meshFilter.sharedMesh = Instantiate(prefab.GetComponent<MeshFilter>().sharedMesh);
					meshCollider.sharedMesh = meshFilter.sharedMesh;
					DestroyImmediate(prefab);
				}
			}

			meshCollider.sharedMesh = Application.isPlaying ? meshFilter.mesh : meshFilter.sharedMesh;
			material.color = PlaybookColors.InActive;
		}

		public override void OnBeginDrag(PointerEventData eventData)
		{
			material.color = PlaybookColors.Active;
			prevPos = Obj.transform.localPosition;
		}

		public override void OnDrag(PointerEventData eventData)
		{
			var delta = (eventData.delta.x + eventData.delta.y) * precision;
			
			switch (Axis)
			{
				case AxisType.X:
					prevPos.x = Mathf.Max(0f, prevPos.x + delta * 0.5f);
					prevPos.y = 0;
					prevPos.z = 0;
					break;
				case AxisType.Y:
					prevPos.x = 0;
					prevPos.y = Mathf.Max(0f, prevPos.y + delta * 0.5f);
					prevPos.z = 0;
					break;
				case AxisType.Z:
					prevPos.x = 0;
					prevPos.y = 0;
					prevPos.z = Mathf.Max(0f, prevPos.z + delta * 0.5f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			Obj.transform.localPosition = prevPos;
			transform.localPosition = prevPos;
		}


		void Update()
		{
			Debug.DrawLine(transform.localPosition, Vector3.forward);
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			material.color = PlaybookColors.InActive;
			prevPos = Obj.transform.localPosition;
		}

	}
}