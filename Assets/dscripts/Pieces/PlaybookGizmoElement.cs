using System.Collections.Generic;
using UnityEngine;

namespace HaiThere.Playbook
{
  public abstract class PlaybookGizmoElement : MonoBehaviour
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

    protected List<PlaybookGizmoPiece> pieces = new List<PlaybookGizmoPiece>();


    public abstract void Create();

  }
}
