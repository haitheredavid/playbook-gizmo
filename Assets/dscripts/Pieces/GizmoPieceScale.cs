using UnityEngine;

namespace HaiThere.Playbook
{
  public class GizmoPieceScale : GizmoPiece
  {

    protected override void ApplyResult(Vector3 result)
    {
      if (obj != null) obj.transform.localScale = result + objDelta;
    }


  }
}
