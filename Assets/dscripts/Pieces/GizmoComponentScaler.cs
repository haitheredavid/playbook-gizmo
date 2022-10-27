using System;
using UnityEngine;
namespace HaiThere.Playbook
{
  public class GizmoComponentScaler : GizmoComponent
  {

    [Header("Handler Props")]
    [SerializeField] bool showLine = true;

    public override void Create()
    {

      var prefab = BuildPrefab<GizmoPieceScale>();

      // hard coded size since its just using a cube but this should change for a model later
      prefab.transform.localScale = new Vector3(.2f, .2f, .5f);

      foreach (AxisType axis in Enum.GetValues(typeof(AxisType)))
      {
        var instance = Instantiate(prefab, transform);
        instance.name = $"Mover_{axis}";
        instance.transform.localPosition = GetPiecePosition(axis);
        instance.transform.localRotation = Quaternion.Euler(GetPieceRotation(axis));
        instance.axis = axis;
        instance.parent = this;

        instance.OnSet += SetActivePiece;
        instance.Create();

        pieces.Add(instance);
      }

      Destroy(prefab.gameObject);
    }
   
  }
}
