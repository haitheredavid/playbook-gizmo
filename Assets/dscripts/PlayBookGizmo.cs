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

    [SerializeField] MovementHandle movementHandle;
    
    public PlaybookObject Obj { get; private set; }

    public List<PlaybookGizmoElement> gizmos { get; private set; }

    public List<PlaybookGizmoPiece> AllPieces
    {
      get
      {
        var p = new List<PlaybookGizmoPiece>();
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
      movementHandle.activeObj = Obj.gameObject;

      // foreach (var p in AllPieces)
      // {
      //   p.Obj = Obj;
      // }
      //
      // IsActive = true;
    }

    public void Awake()
    {

      gizmos = new List<PlaybookGizmoElement>();
      movementHandle = new GameObject("Mover").AddComponent<MovementHandle>();
      movementHandle.transform.SetParent(transform);
      movementHandle.movementSpeed = movementSpeed;
      movementHandle.viewer = user.Viewer;
      movementHandle.isParented = true;
      movementHandle.OnActionComplete += SetPosition;
      movementHandle.Create();

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
