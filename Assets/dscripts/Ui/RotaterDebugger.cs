using HaiThere.Playbook;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public interface IGizmoDebugger<in TGizmo> where TGizmo : GizmoComponent
{
  public void Attach(TGizmo obj);
  public bool isActive { get; set; }

}

public class RotaterDebugger : MonoBehaviour, IGizmoDebugger<GizmoComponentRotater>
{

  [SerializeField] Slider xSlider, ySlider, zSlider, wSlider;
  [SerializeField] GizmoComponentRotater comp;

  Quaternion quaternion;

  public bool isActive
  {
    get;
    set;
  }

  public void Awake()
  {
    if (xSlider != null) xSlider.onValueChanged.AddListener(value => quaternion.x = value);
    if (ySlider != null) ySlider.onValueChanged.AddListener(value => quaternion.y = value);
    if (zSlider != null) zSlider.onValueChanged.AddListener(value => quaternion.z = value);
    if (wSlider != null) wSlider.onValueChanged.AddListener(value => quaternion.w = value);
    
  }

  public void Attach(GizmoComponentRotater obj)
  {
    if (obj == null)
    {
      Debug.Log("Rotater Is null");
      return;
    }

    Debug.Log($"attaching {obj.GetType()}");

    comp = obj;
    comp.OnDebugging += v => isActive = v;
    if (comp.activeObj != null)
    {
      quaternion = comp.activeObj.transform.rotation;
      xSlider.SetValueWithoutNotify(quaternion.x);
      ySlider.SetValueWithoutNotify(quaternion.y);
      zSlider.SetValueWithoutNotify(quaternion.z);
      wSlider.SetValueWithoutNotify(quaternion.w);
    }
  }

  public void OnDrawGizmos()
  {
    if (comp == null || comp.activeObj == null)
      return;

    comp.activeObj.transform.rotation = quaternion;
    var t = comp.activeObj.transform;

    Gizmos.color = Color.green;
    Gizmos.DrawLine(t.position, t.position + t.forward);

    Gizmos.color = Color.blue;
    Gizmos.DrawLine(t.position, t.position + t.up);

    Gizmos.color = Color.red;
    Gizmos.DrawLine(t.position, t.position + t.right);

    Gizmos.matrix = t.localToWorldMatrix;
    Gizmos.DrawWireSphere(t.localPosition, comp.innerRadius + comp.offsetSize);
  }


}
