using UnityEngine;

namespace HaiThere.Playbook
{
  public class PlaybookUser : MonoBehaviour
  {

    public Camera viewer;
    public float distanceOffset = 10f;
    public float speed = 5;
    Vector3 target = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    public Camera Viewer
    {
      get => viewer;
      set => viewer = value;
    }

    public void SetTarget(Transform t)
    {
      target = t.position;
      LookAtTarget();
    }

    public void LookAtTarget()
    {
      transform.position = target;
      viewer.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
      transform.Translate(new Vector3(0, 0, -distanceOffset));
    }

    void Update()
    {
      if (!Input.GetMouseButton(1))
        return;

      var yAxis = Input.GetAxis("Mouse Y");
      var xAxis = Input.GetAxis("Mouse X");

      rotation.x += yAxis * speed;
      rotation.y += xAxis * speed;

      LookAtTarget();
    }


  }
}
