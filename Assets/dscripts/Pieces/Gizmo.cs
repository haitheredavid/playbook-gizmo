using UnityEngine;
namespace HaiThere.Playbook
{
  public abstract class Gizmo : MonoBehaviour
  {
    public abstract void Create();
    public abstract void SetActive(bool status);

  }
}
