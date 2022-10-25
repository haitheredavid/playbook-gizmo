using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HaiThere.Playbook
{

	public class GizmoPieceRotate : GizmoPiece, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
	{

		[SerializeField] AxisType axis;

		[SerializeField, Range(1, 100)] int sides = 50;

		[SerializeField, Range(0f, 5f)] float offsetSize = 1f;
		[SerializeField, Range(0.01f, 10f)] float innerRadius = 2f;

		Vector3 prevAngle;
		Vector3 objAngle;

		public event UnityAction<AxisType, Vector3> OnAxisClicked;

		public override void OnEnable()
		{
			base.OnEnable();
			prevAngle = transform.localRotation.eulerAngles;
			material.color = PlaybookColors.SetInactive(axis switch
			{
				AxisType.X => PlaybookColors.Rotation_X,
				AxisType.Y => PlaybookColors.Rotation_Y,
				AxisType.Z => PlaybookColors.Rotation_Z,
				_ => material.color
			});
		}

		public void Create(AxisType axisType)
		{
			axis = axisType;

			transform.localRotation = axis switch
			{
				AxisType.X => Quaternion.Euler(-90, 0, 0),
				AxisType.Y => Quaternion.Euler(0, 0, 0),
				AxisType.Z => Quaternion.Euler(0, 90, 0),
				_ => transform.localRotation
			};

			Create();
		}

		public override void Create()
		{
			var points = new List<Vector3>();

			points.AddRange(PointRing(meshFilter.transform.localPosition, sides, innerRadius));
			points.AddRange(PointRing(meshFilter.transform.localPosition, sides, innerRadius + offsetSize));

			var triSides = points.Count / 2;
			var triangles = new List<int>();

			for (int index = 0; index < triSides; index++)
			{
				triangles.Add(index);
				triangles.Add(index + triSides);
				triangles.Add((index + 1) % triSides);

				triangles.Add(index);
				triangles.Add(triSides + (triSides + index - 1) % triSides);
				triangles.Add(index + triSides);
			}

			if (Application.isPlaying)
			{
				if (!ObjectValid(meshFilter.mesh))
				{
					meshFilter.mesh = new Mesh();
				}

				meshFilter.mesh.Clear();
				meshFilter.mesh.SetVertices(points);
				meshFilter.mesh.SetTriangles(triangles, 0);
				meshCollider.sharedMesh = meshFilter.mesh;
			}
			else
			{
				if (!ObjectValid(meshFilter.sharedMesh))
				{
					meshFilter.sharedMesh = new Mesh();
				}

				meshFilter.sharedMesh.Clear();
				meshFilter.sharedMesh.SetVertices(points);
				meshFilter.sharedMesh.SetTriangles(triangles, 0);
				meshCollider.sharedMesh = meshFilter.sharedMesh;
			}
		}

		public override void OnBeginDrag(PointerEventData eventData)
		{
			material.color = axis switch
			{
				AxisType.X => PlaybookColors.Rotation_X,
				AxisType.Y => PlaybookColors.Rotation_Y,
				AxisType.Z => PlaybookColors.Rotation_Z,
				_ => material.color
			};

			prevAngle = transform.localRotation.eulerAngles;

			if (Obj != null)
			{
				objAngle = Obj.transform.localRotation.eulerAngles;
			}
		}

		public override void OnDrag(PointerEventData eventData)
		{
			var delta = (eventData.delta.x + eventData.delta.y) * precision;

			switch (axis)
			{
				case AxisType.X:
					delta *= -1;
					prevAngle += new Vector3(0, delta, 0);
					objAngle = new Vector3(objAngle.x, objAngle.y + delta, objAngle.z);
					break;
				case AxisType.Y:
					prevAngle += new Vector3(0, 0, delta);
					objAngle = new Vector3(objAngle.x, objAngle.y, objAngle.z + delta);
					break;
				case AxisType.Z:
					delta *= -1;
					prevAngle += new Vector3(0, 0, delta);
					objAngle = new Vector3(objAngle.x + delta, objAngle.y, objAngle.z);
					break;
			}

			transform.localRotation = Quaternion.Euler(prevAngle.x, prevAngle.y, prevAngle.z);

			if (Obj != null)
			{
				Obj.transform.localRotation = Quaternion.Euler(objAngle);
			}
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			material.color = PlaybookColors.SetInactive(axis switch
			{
				AxisType.X => PlaybookColors.Rotation_X,
				AxisType.Y => PlaybookColors.Rotation_Y,
				AxisType.Z => PlaybookColors.Rotation_Z,
				_ => material.color
			});
		}

		protected override void UpdateToNewObject()
		{
			var size = Obj.objectBounds.size;

			innerRadius = Mathf.Max(
				size.x * (Obj.transform.localScale.x * 0.8f),
				size.y * (Obj.transform.localScale.y * 0.8f),
				size.z * (Obj.transform.localScale.z * 0.8f)
			);

			Create();
		}

		static IEnumerable<Vector3> PointRing(Vector3 center, int sides, float radius)
		{
			const float T = 2 * Mathf.PI;
			var points = new List<Vector3>();
			var circSteps = 1f / sides;
			var radianSteps = circSteps * T;

			for (var i = 0; i < sides; i++)
			{
				var radian = radianSteps * i;
				points.Add(
					center
					+ new Vector3(Mathf.Cos(radian) * radius,
					              Mathf.Sin(radian) * radius,
					              0)
				);
			}

			return points;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log("Clicked");
			OnAxisClicked?.Invoke(axis, eventData.pointerCurrentRaycast.worldPosition);
		}
	}
}