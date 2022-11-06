using HaiThere.Playbook;
using UnityEngine;

public interface IGizmoDebugger<in TGizmo> where TGizmo : GizmoComponent
{
  public void Attach(TGizmo obj);
  public bool isActive { get; set; }

}

public class RotaterDebugger : MonoBehaviour, IGizmoDebugger<GizmoComponentRotater>
{
  public bool isActive
  {
    get;
    set;
  }

  public void Attach(GizmoComponentRotater obj)
  {
    if (obj == null)
    {
      Debug.Log("Rotater Is null");
      return;
    }

    Debug.Log($"attaching {obj.GetType()}");

    obj.OnDebugging += v => isActive = v;
  }


}
