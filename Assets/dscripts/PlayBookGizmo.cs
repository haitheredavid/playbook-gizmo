using System;
using System.Collections.Generic;
using UnityEngine;

namespace HaiThere.Playbook
{

	/// <summary>
	/// Object for creating the editing functionality of a mesh at runtime
	/// </summary>
	public class PlayBookGizmo : MonoBehaviour
	{

		[SerializeField] PlaybookUser user;
		[SerializeField] [Range(1f, 100f)] float movementSpeed = 100f;

		[SerializeField] GizmoPieceScale scale;
		[SerializeField] GizmoPieceMove move;
		[SerializeField] List<GizmoPieceRotate> rotate;
		[SerializeField] bool isActive;

		[SerializeField] Vector3 anchorOffset = new Vector3(0.2f, 0.0f, 0.2f);

		[SerializeField] MovementHandle movementHandle;

		GameObject tranformerParent, rotaterParent;

		public PlaybookObject Obj { get; private set; }

		public List<PlaybookGizmoElement> gizmos { get; private set; }

		public List<PlaybookGizmoPiece> AllPieces
		{
			get
			{
				var p = new List<PlaybookGizmoPiece>();
				p.AddRange(rotate);
				p.Add(scale);
				p.Add(move);
				return p;
			}
		}

		public bool IsActive
		{
			get => isActive;

			set
			{
				isActive = value;
				foreach (var piece in AllPieces)
				{
					piece.SetActive(value);
				}
			}
		}

		public void CreateGizmo(PlaybookUser newUser)
		{
			if (newUser == null)
			{
				Debug.Log("Playbook user is not valid to set");
				return;
			}

			user = newUser;
			movementHandle.movementSpeed = movementSpeed;
			movementHandle.Create();
		}

		public void SetActiveObj(PlaybookObject playbookObject)
		{
			if (playbookObject == null)
				return;

			Obj = playbookObject;
			transform.position = Obj.transform.position;

			foreach (var p in AllPieces)
			{
				p.Obj = Obj;
			}

			IsActive = true;
		}

		public void Awake()
		{
			if (movementHandle == null)
			{
				Debug.Log("Movement Handle is not valid");
				return;
			}

			gizmos = new List<PlaybookGizmoElement>();

			movementHandle.OnActionComplete += Reset;

			// rotaterParent = new GameObject("");
			// rotaterParent.transform.SetParent(transform, true);
			// foreach (var r in rotate)
			// {
			// 	r.transform.SetParent(rotaterParent.transform);
			// 	r.OnAxisClicked += SetAxisPosition;
			// 	scale.OnScaleChange += () => r.Obj = Obj;
			// }
			//
			// tranformerParent = new GameObject("Transformer");
			// tranformerParent.transform.SetParent(transform);
			// scale.transform.SetParent(tranformerParent.transform);
			// move.transform.SetParent(tranformerParent.transform);
			// tranformerParent.transform.localRotation = Quaternion.Euler(0, 90, 0);
			// foreach (var p in AllPieces)
			// {
			// 	p.Create();
			// }
		}

		void Reset()
		{
			transform.position = Obj != null ? new Vector3(
				Obj.transform.position.x - Obj.transform.localScale.x * 0.5f + anchorOffset.x,
				Obj.transform.position.y - Obj.transform.localScale.y * 0.5f + anchorOffset.y,
				Obj.transform.position.z - Obj.transform.localScale.z * 0.5f + anchorOffset.z
			) : Vector3.zero;
		}


		public void Update()
		{
			if (Obj == null)
				return;

			transform.position = Obj.transform.position;
		}

	}
}