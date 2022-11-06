using HaiThere.Playbook;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GizmoDebugger : MonoBehaviour
{

  [SerializeField] RotaterDebugger rotater;

  public bool isInit { get; private set; } = false;

  public void Init(List<GizmoComponent> objs)
  {
    if (objs == null || !objs.Any())
    {
      Debug.Log("No active objects were found to attach");
      return;
    }

    foreach (var o in objs)
    {
      switch (o)
      {
        case GizmoComponentRotater c:
          rotater.Attach(c);
          break;
        case GizmoComponentMover c:
          break;
        case GizmoComponentScaler c:
          break;

      }
    }

    isInit = true;

  }

}
