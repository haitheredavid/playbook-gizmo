using HaiThere.Playbook;


public class GizmoComponentMover : GizmoComponent
{

  protected override GizmoPiece BuildPrefab() => BuildPrefab<GizmoPieceMove>();


}
