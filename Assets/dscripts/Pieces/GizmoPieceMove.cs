using HaiThere.Playbook;
using UnityEngine;

public class GizmoPieceMove : GizmoPiece
{

  protected override void ApplyResult(Vector3 result)
  {
    obj.transform.position = result + objDelta;
  }






}
