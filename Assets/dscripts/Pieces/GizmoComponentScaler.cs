using UnityEngine;
namespace HaiThere.Playbook
{
  public class GizmoComponentScaler : GizmoComponent
  {
    protected override GizmoPiece BuildPrefab() => BuildPrefab<GizmoPieceScale>();

    protected override Mesh CreateMesh() => Builder.CreateCone(1f, 1f);
  }
}
