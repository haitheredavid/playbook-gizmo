using UnityEngine;

namespace HaiThere.Playbook
{
  public class GizmoPieceScale : GizmoPiece
  {
    protected override void ApplyResult(Vector3 result) => obj.transform.localScale = result;
  }
}
