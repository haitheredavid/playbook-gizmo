using System;
using UnityEngine;
namespace HaiThere.Playbook
{
  public class GizmoComponentScaler : GizmoComponent
  {
    protected override GizmoPiece BuildPrefab() => BuildPrefab<GizmoPieceScale>();

  }
}
