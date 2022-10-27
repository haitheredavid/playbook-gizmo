using UnityEngine;

namespace HaiThere.Playbook
{
  public class PlaybookUser : MonoBehaviour
  {

    public Camera viewer;
    public float distanceOffset = 10f;
    public float speed = 5;

    Transform target;

    Vector3 prevPos;
    Vector2 rotation = Vector2.zero;

    public Camera Viewer
    {
      get => viewer;
      set => viewer = value;
    }

    public void SetTarget(Transform t)
    {
      target = t;
      LookAtTarget();
    }

    public void LookAtTarget()
    {
      transform.position = target.position;
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
