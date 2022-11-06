using UnityEngine;
namespace HaiThere.Playbook
{
  public class GizmoComponentRotater : GizmoComponent
  {
    [Range(1, 100)] public int sides = 50;
    [Range(0f, 5f)] public float offsetSize = 1f;
    [Range(0.01f, 10f)] public float innerRadius = 2f;

    protected override GizmoPiece BuildPrefab()
    {
      var prefab = BuildPrefab<GizmoPieceRotate>();
      prefab.sides = sides;
      prefab.offsetSize = offsetSize;
      prefab.innerRadius = innerRadius;
      return prefab;
    }

  }
}
