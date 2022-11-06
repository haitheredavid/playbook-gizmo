using UnityEngine;
using UnityEngine.EventSystems;

namespace HaiThere.Playbook
{

  public class GizmoPieceRotate : GizmoPiece
  {

    public int sides { get; set; } = 50;
    public float offsetSize{ get; set; }  = 1f;
    public float innerRadius { get; set; } = 2f;

    Vector3 prevAngle;
    Vector3 objAngle;


    public void Start()
    {
      prevAngle = transform.localRotation.eulerAngles;
      material.color = PlaybookColors.SetInactive(axis switch
      {
        AxisType.X => PlaybookColors.AxisX,
        AxisType.Y => PlaybookColors.AxisY,
        AxisType.Z => PlaybookColors.AxisZ,
        _ => material.color
      });
    }

    // public void Create(AxisType axisType)
    // {
    //   axis = axisType;
    //
    //   transform.localRotation = axis switch
    //   {
    //     AxisType.X => Quaternion.Euler(-90, 0, 0),
    //     AxisType.Y => Quaternion.Euler(0, 0, 0),
    //     AxisType.Z => Quaternion.Euler(0, 90, 0),
    //     _ => transform.localRotation
    //   };
    //
    //   Create();
    // }

    public override void Create()
    {
      base.Create();
      var mesh = Builder.CreateCircle(meshFilter.transform.localPosition, sides, innerRadius, offsetSize);

      if (Application.isPlaying)
      {
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = meshFilter.mesh;
      }
      else
      {
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = meshFilter.sharedMesh;
      }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
      material.color = axis switch
      {
        AxisType.X => PlaybookColors.AxisX,
        AxisType.Y => PlaybookColors.AxisY,
        AxisType.Z => PlaybookColors.AxisZ,
        _ => material.color
      };

      prevAngle = transform.localRotation.eulerAngles;

      if (obj != null)
      {
        objAngle = obj.transform.localRotation.eulerAngles;
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

      if (obj != null)
      {
        obj.transform.localRotation = Quaternion.Euler(objAngle);
      }
    }


    public override void OnEndDrag(PointerEventData eventData)
    {
      material.color = PlaybookColors.SetInactive(axis switch
      {
        AxisType.X => PlaybookColors.AxisX,
        AxisType.Y => PlaybookColors.AxisY,
        AxisType.Z => PlaybookColors.AxisZ,
        _ => material.color
      });
    }
    protected override void ApplyResult(Vector3 result)
    {
    }
    // protected override void UpdateToNewObject()
    // {
    // 	var size = Obj.objectBounds.size;
    //
    // 	innerRadius = Mathf.Max(
    // 		size.x * (Obj.transform.localScale.x * 0.8f),
    // 		size.y * (Obj.transform.localScale.y * 0.8f),
    // 		size.z * (Obj.transform.localScale.z * 0.8f)
    // 	);
    //
    // 	Create();
    // }

  }
}
