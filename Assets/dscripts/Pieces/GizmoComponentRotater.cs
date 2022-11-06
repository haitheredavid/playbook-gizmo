namespace HaiThere.Playbook
{
  public class GizmoComponentRotater : GizmoComponent
  {
    protected override GizmoPiece BuildPrefab() => BuildPrefab<GizmoPieceRotate>();
  }
}
