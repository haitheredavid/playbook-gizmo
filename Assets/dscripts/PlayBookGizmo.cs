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

		[SerializeField] GizmoPieceScale scale;
		[SerializeField] GizmoPieceMove move;
		[SerializeField] List<GizmoPieceRotate> rotate;
		[SerializeField] bool isActive;

		GameObject tranformerParent, rotaterParent;

		public PlaybookObject Obj { get; private set; }

		public List<GizmoPiece> AllPieces
		{
			get
			{
				var p = new List<GizmoPiece>();
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

		public void OnEnable()
		{
			rotaterParent = new GameObject("Rotaters");
			rotaterParent.transform.SetParent(transform, true);
			foreach (var r in rotate)
			{
				r.transform.SetParent(rotaterParent.transform);
				r.OnAxisClicked += SetAxisPosition;
				scale.OnScaleChange += () => r.Obj = Obj;
			}

			tranformerParent = new GameObject("Transformer");
			tranformerParent.transform.SetParent(transform);
			scale.transform.SetParent(tranformerParent.transform);
			move.transform.SetParent(tranformerParent.transform);
			tranformerParent.transform.localRotation = Quaternion.Euler(0, 90, 0);
			foreach (var p in AllPieces)
			{
				p.Create();
			}
		}

		void SetAxisPosition(AxisType axis, Vector3 pos)
		{
			tranformerParent.transform.localRotation = axis switch
			{
				AxisType.X => Quaternion.Euler(-90, 0, 0),
				AxisType.Y => Quaternion.Euler(0, 0, 0),
				AxisType.Z => Quaternion.Euler(0, 90, 0),
				_ => tranformerParent.transform.localRotation
			};

			move.Axis = axis;
		}

		public void Update()
		{
			if (Obj == null)
				return;

			transform.position = Obj.transform.position;
		}

	}
}