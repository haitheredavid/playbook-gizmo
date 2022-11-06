using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace HaiThere.Playbook
{

  /// <summary>
  /// Object for creating the editing functionality of a mesh at runtime
  /// </summary>
  public class Gizmo : MonoBehaviour
  {
    public PlaybookUser user;


    [SerializeField][Range(1f, 100f)] float movementSpeed = 100f;
    [SerializeField] bool enableMovement, enableScaling, enableRotation;
    [SerializeField] Vector3 anchorOffset = new Vector3(0.2f, 0.0f, 0.2f);
    [SerializeField] bool debug;
    [SerializeField] GizmoDebugger debugger;


    GizmoComponentScaler scaler;
    GizmoComponentMover mover;
    GizmoComponentRotater rotater;
    PlaybookObject Obj;

    public Vector3 Anchor { get; set; }

    public List<GizmoComponent> gizmos { get; private set; }

    public bool isDebugging
    {
      get => debug;
      set
      {
        debug = value;
        if (gizmos == null || !gizmos.Any())
        {
          Debug.Log("No Active components to set to debug");
          return;
        }

        if (debugger == null)
        {
          Debug.Log("No container for debuggers is found, set that up!");
          return;
        }

        if (!debugger.isInit)
        {
          debugger.Init(gizmos);
        }

        foreach (var g in gizmos)
        {
          g.debug = value;
        }
      }
    }


    public void SetActiveObj(PlaybookObject playbookObject)
    {
      if (playbookObject == null)
        return;

      Obj = playbookObject;
      var center = playbookObject.objectBounds.center;
      var size = playbookObject.objectBounds.size;
      Anchor = new Vector3(center.x - size.x, center.y, center.z - size.z);

      SetPosition();

      foreach (var g in gizmos)
      {
        g.activeObj = Obj.gameObject;
      }
    }

    void Awake()
    {
      gizmos = new List<GizmoComponent>();

      if (enableMovement)
      {
        mover = new GameObject("Mover").AddComponent<GizmoComponentMover>();
        gizmos.Add(mover);
      }

      if (enableScaling)
      {
        scaler = new GameObject("Scaler").AddComponent<GizmoComponentScaler>();
        scaler.positionOffset = scaler.scaleOffset * 2f + 0.15f;
        gizmos.Add(scaler);
      }

      if (enableRotation)
      {
        rotater = new GameObject("Rotater").AddComponent<GizmoComponentRotater>();
        gizmos.Add(rotater);
      }

    }

    public void Create()
    {
      foreach (var gizmo in gizmos)
      {
        gizmo.transform.SetParent(transform);
        gizmo.movementSpeed = movementSpeed;
        gizmo.viewer = user.Viewer;
        gizmo.isParented = true;
        gizmo.OnActionComplete += ComponentComplete;
        gizmo.Create();
      }

    }

    void ComponentComplete()
    {
      SetPosition();
      OnObjectModified?.Invoke();
    }


    void SetPosition()
    {
      transform.position = Obj != null ? new Vector3(
        Obj.transform.position.x + Anchor.x + anchorOffset.x,
        Obj.transform.position.y + Anchor.y + anchorOffset.y,
        Obj.transform.position.z + Anchor.z + anchorOffset.z
      ) : Vector3.zero;
    }

    public void SetActive(bool b)
    {
      foreach (var g in gizmos)
      {
        g.SetActive(b);
      }
    }

    public event UnityAction OnObjectModified;
  }
}
