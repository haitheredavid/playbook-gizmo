using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HaiThere.Playbook
{
	public class GizmoPieceScale : GizmoPiece
	{

		Vector3 scale = Vector3.one;
		Vector3 prevPos = Vector3.zero;

		public event UnityAction OnScaleChange;

		public Vector3 offset => new Vector3(0, 0, 1.5f);

		protected override void Setup()
		{
			if (meshFilter.mesh == null && meshFilter.sharedMesh == null)
			{
				transform.localScale = Vector3.zero;
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
			scale = Obj.transform.localScale;
			prevPos = transform.localPosition;
		}

		public override void OnDrag(PointerEventData eventData)
		{
			var delta = (eventData.delta.x + eventData.delta.y) * precision;

			scale.x = Mathf.Max(0f, scale.x + delta * 0.5f);
			scale.y = Mathf.Max(0f, scale.y + delta * 0.5f);
			scale.z = Mathf.Max(0f, scale.z + delta * 0.5f);
			Obj.transform.localScale = scale;

			prevPos.x = Mathf.Max(0f, prevPos.x + delta * 0.5f);
			transform.localPosition = new Vector3(prevPos.x, prevPos.y, prevPos.z);
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			material.color = PlaybookColors.InActive;

			scale = Obj.transform.localScale;
			prevPos = transform.localPosition;
			OnScaleChange?.Invoke();
		}


	}
}