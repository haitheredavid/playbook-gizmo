using UnityEngine;
namespace HaiThere.Playbook
{
  public class GizmoComponentRotater : GizmoComponent
  {
    [Range(1, 100)] public int sides = 50;
    [Range(0f, 5f)] public float offsetSize = 1f;
    [Range(0.01f, 10f)] public float innerRadius = 2f;

    protected override Vector3 prefabScale => new Vector3(0.2f, pieceScale, pieceScale);

    protected override Mesh CreateMesh()
    {
      return Resources.Load<Mesh>("CylinderPiece");
      // return Builder.CreateCircle(sides, innerRadius, offsetSize);
    }

    protected override Vector3 GetPiecePosition(AxisType axis)
    {
      var pos = new Vector3(0.5f, 0f, 0.5f);
      return axis switch
      {
        AxisType.X => pos,
        AxisType.Y => pos,
        AxisType.Z => pos,
        _ => Vector3.zero
      };
    }

    protected override Vector3 GetPieceRotation(AxisType axis)
    {
      return axis switch
      {
        AxisType.X => new Vector3(0, 0, 0),
        AxisType.Y => new Vector3(0, 0, 90),
        AxisType.Z => new Vector3(0, 90, 0),
        _ => Vector3.zero
      };
    }
    
    protected override GizmoPiece BuildPrefab()
    {
      pieceScale = 0.8f;
      return BuildPrefab<GizmoPieceRotate>();
    }

  }
}
