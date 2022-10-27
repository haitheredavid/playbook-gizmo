using System.Collections.Generic;
using UnityEngine;

namespace HaiThere.Playbook
{

  /// <summary>
  /// Object for creating the editing functionality of a mesh at runtime
  /// </summary>
  public class PlayBookGizmo : MonoBehaviour
  {

    [SerializeField] PlaybookUser user;
    [SerializeField][Range(1f, 100f)] float movementSpeed = 100f;

    [SerializeField] List<GizmoPieceRotate> rotate;
    [SerializeField] bool isActive;

    [SerializeField] Vector3 anchorOffset = new Vector3(0.2f, 0.0f, 0.2f);

    GizmoComponentScaler scaler;
    GizmoComponentMover mover;
    PlaybookObject Obj;

    public List<GizmoComponent> gizmos { get; private set; }

    public List<GizmoPiece> AllPieces
    {
      get
      {
        var p = new List<GizmoPiece>();
        p.AddRange(rotate);
        return p;
      }
    }

    public bool IsActive
    {
      get => isActive;
      set
      {
        isActive = value;
        foreach (var piece in AllPieces)
        {
          piece.SetActive(value);
        }
      }
    }

    public void SetActiveObj(PlaybookObject playbookObject)
    {
      if (playbookObject == null)
        return;

      Obj = playbookObject;
      SetPosition();

      foreach (var g in gizmos)
      {
        g.activeObj = Obj.gameObject;
      }
    }

    public void Awake()
    {

      gizmos = new List<GizmoComponent>();

      mover = new GameObject("Mover").AddComponent<GizmoComponentMover>();
      mover.transform.SetParent(transform);
      mover.movementSpeed = movementSpeed;
      mover.viewer = user.Viewer;
      mover.isParented = true;
      mover.OnActionComplete += SetPosition;
      gizmos.Add(mover);

      scaler = new GameObject("Scaler").AddComponent<GizmoComponentScaler>();
      gizmos.Add(scaler);

      foreach (var gizmo in gizmos)
      {
        gizmo.Create();
      }

    }

    void SetPosition()
    {
      transform.position = Obj != null ? new Vector3(
        Obj.transform.position.x - Obj.transform.localScale.x * 0.5f + anchorOffset.x,
        Obj.transform.position.y - Obj.transform.localScale.y * 0.5f + anchorOffset.y,
        Obj.transform.position.z - Obj.transform.localScale.z * 0.5f + anchorOffset.z
      ) : Vector3.zero;
    }


  }
}
