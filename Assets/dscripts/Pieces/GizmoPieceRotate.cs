using UnityEngine;
using UnityEngine.EventSystems;

namespace HaiThere.Playbook
{

  public class GizmoPieceRotate : GizmoPiece
  {

    protected override void ApplyResult(Vector3 result)
    {
      obj.transform.localRotation = Quaternion.Euler(result);
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

    // public override void OnBeginDrag(PointerEventData eventData)
    // {
    //   material.color = axis switch
    //   {
    //     AxisType.X => PlaybookColors.AxisX,
    //     AxisType.Y => PlaybookColors.AxisY,
    //     AxisType.Z => PlaybookColors.AxisZ,
    //     _ => material.color
    //   };
    //
    //   prevAngle = transform.localRotation.eulerAngles;
    //
    //   if (obj != null)
    //   {
    //     objAngle = obj.transform.localRotation.eulerAngles;
    //   }
    // }
    //
    // public override void OnDrag(PointerEventData eventData)
    // {
    //   var delta = (eventData.delta.x + eventData.delta.y) * precision;
    //
    //   switch (axis)
    //   {
    //     case AxisType.X:
    //       delta *= -1;
    //       prevAngle += new Vector3(0, delta, 0);
    //       objAngle = new Vector3(objAngle.x, objAngle.y + delta, objAngle.z);
    //       break;
    //     case AxisType.Y:
    //       prevAngle += new Vector3(0, 0, delta);
    //       objAngle = new Vector3(objAngle.x, objAngle.y, objAngle.z + delta);
    //       break;
    //     case AxisType.Z:
    //       delta *= -1;
    //       prevAngle += new Vector3(0, 0, delta);
    //       objAngle = new Vector3(objAngle.x + delta, objAngle.y, objAngle.z);
    //       break;
    //   }
    //
    //   transform.localRotation = Quaternion.Euler(prevAngle.x, prevAngle.y, prevAngle.z);
    //
    //   if (obj != null)
    //   {
    //     obj.transform.localRotation = Quaternion.Euler(objAngle);
    //   }
    // }
    //
    //
    // public override void OnEndDrag(PointerEventData eventData)
    // {
    //   material.color = PlaybookColors.SetInactive(axis switch
    //   {
    //     AxisType.X => PlaybookColors.AxisX,
    //     AxisType.Y => PlaybookColors.AxisY,
    //     AxisType.Z => PlaybookColors.AxisZ,
    //     _ => material.color
    //   });
    // }
    //


  }
}
