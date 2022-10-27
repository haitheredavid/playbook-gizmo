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

    [SerializeField] GizmoPieceScale scale;
    [SerializeField] List<GizmoPieceRotate> rotate;
    [SerializeField] bool isActive;

    [SerializeField] Vector3 anchorOffset = new Vector3(0.2f, 0.0f, 0.2f);

    [SerializeField] GizmoComponentMover gizmoComponentMover;
    
    public PlaybookObject Obj { get; private set; }

    public List<GizmoComponent> gizmos { get; private set; }

    public List<GizmoPiece> AllPieces
    {
      get
      {
        var p = new List<GizmoPiece>();
        p.AddRange(rotate);
        p.Add(scale);
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
      gizmoComponentMover.activeObj = Obj.gameObject;

      // foreach (var p in AllPieces)
      // {
      //   p.Obj = Obj;
      // }
      //
      // IsActive = true;
    }

    public void Awake()
    {

      gizmos = new List<GizmoComponent>();
      gizmoComponentMover = new GameObject("Mover").AddComponent<GizmoComponentMover>();
      gizmoComponentMover.transform.SetParent(transform);
      gizmoComponentMover.movementSpeed = movementSpeed;
      gizmoComponentMover.viewer = user.Viewer;
      gizmoComponentMover.isParented = true;
      gizmoComponentMover.OnActionComplete += SetPosition;
      gizmoComponentMover.Create();

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
