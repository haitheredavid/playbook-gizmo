using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HaiThere.Playbook
{
	public enum AxisType
	{
		X,
		Y,
		Z
	}

	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
	public abstract class PlaybookGizmoPiece : PlaybookGizmoElement, IDragHandler, IBeginDragHandler, IEndDragHandler
	{

		[SerializeField, HideInInspector] protected MeshFilter meshFilter;
		[SerializeField, HideInInspector] protected MeshRenderer meshRenderer;
		[SerializeField, HideInInspector] protected MeshCollider meshCollider;

		[SerializeField, Range(0.01f, 10f)] protected float precision = 0.5f;

		protected Material material;

		PlaybookObject _obj;

		public PlaybookObject Obj
		{
			get => _obj;
			set
			{
				_obj = value;
				UpdateToNewObject();
			}
		}

		protected virtual void UpdateToNewObject()
		{ }

		public virtual void SetActive(bool status)
		{
			if (meshRenderer)
				meshRenderer.enabled = status;
			if (meshCollider)
				meshCollider.enabled = status;
		}

		protected static bool ObjectValid<TObj>(TObj mesh) where TObj : UnityEngine.Object
		{
			return mesh != null;
		}

		public override void Create()
		{
			meshCollider = gameObject.GetComponent<MeshCollider>();
			meshFilter = gameObject.GetComponent<MeshFilter>();
			meshRenderer = gameObject.GetComponent<MeshRenderer>();
			material = meshRenderer.material;

			SetupPiece();
		}

		protected abstract void SetupPiece();

		public abstract void OnDrag(PointerEventData eventData);

		public abstract void OnBeginDrag(PointerEventData eventData);

		public abstract void OnEndDrag(PointerEventData eventData);
	}
}