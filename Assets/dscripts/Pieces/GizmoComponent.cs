using System.Collections.Generic;
using UnityEngine;

namespace HaiThere.Playbook
{
  public abstract class GizmoComponent : MonoBehaviour
  {
    [SerializeField] protected GameObject obj;

    public Vector3 pointerPos { get; protected set; }

    public GameObject activeObj
    {
      get => obj;
      set
      {

        if (value == null)
          return;

        obj = value;

        foreach (var piece in pieces)
        {
          piece.focusObj = value.transform;
        }
      }
    }

    protected List<GizmoPiece> pieces = new List<GizmoPiece>();


    public abstract void Create();

  }
}
