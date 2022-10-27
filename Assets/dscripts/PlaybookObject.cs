using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HaiThere.Playbook
{
	public class PlaybookObject : MonoBehaviour, IPointerClickHandler
	{

		public static Vector3 GetLowestPoint<T>(Transform origin) where T : Collider => origin.GetComponent<T>().bounds.min;

		public event UnityAction<PlaybookObject> OnClicked;

		Bounds bounds;

		public Bounds objectBounds
		{
			get => bounds;
			private set => bounds = value;
		}

		public void Awake()
		{
			GetExtents(true);
		}

		public Vector3 GetExtents(bool recalculate)
		{
			if (objectBounds != default && !recalculate)
				return objectBounds.extents;

			var referenceTransform = transform;
			objectBounds = new Bounds(Vector3.zero, Vector3.zero);
			GrabAndEncapsulate(transform, ref bounds);
			return objectBounds.extents;

			// Quick snag from https://forum.unity.com/threads/getting-the-bounds-of-the-group-of-objects.70979/#post-6440477
			void GrabAndEncapsulate(Transform child, ref Bounds b)
			{
				var mesh = child.GetComponent<MeshFilter>();
				if (mesh)
				{
					var lsBounds = mesh.sharedMesh.bounds;
					var wsMin = child.TransformPoint(lsBounds.center - lsBounds.extents);
					var wsMax = child.TransformPoint(lsBounds.center + lsBounds.extents);
					b.Encapsulate(referenceTransform.InverseTransformPoint(wsMin));
					b.Encapsulate(referenceTransform.InverseTransformPoint(wsMax));
				}

				foreach (Transform grandChild in child.transform)
				{
					GrabAndEncapsulate(grandChild, ref b);
				}
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnClicked?.Invoke(this);
		}

	}
}