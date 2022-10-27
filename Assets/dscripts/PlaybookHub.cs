using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HaiThere.Playbook
{
  public class PlaybookHub : MonoBehaviour
  {
    [SerializeField] PlayBookGizmo gizmo;
    [SerializeField] PlaybookUser user;
    [SerializeField] GameObject playBookObjectPrefab;
    [SerializeField] List<PlaybookObject> objects = new List<PlaybookObject>();
    
    PlaybookObject obj;

    public void CreateNewObject()
    {
      Debug.Log("Creating New Object");

      if (playBookObjectPrefab == null)
      {
        Debug.Log("Set a prefab first!");
        return;
      }

      var item = Instantiate(playBookObjectPrefab).GetComponent<PlaybookObject>();

      objects.Add(item);
      gizmo.SetActive(true);

      obj = item;
      user.SetTarget(item.transform);
      gizmo.SetActiveObj(item);
    }

    public void ClearAllObjects()
    {
      Debug.Log("Clearing all objects");

      if (objects.Any())
      {
        for (int i = objects.Count; i > 0; i--)
        {
          objects[i - 1].OnClicked -= ObjectSelected;
          Destroy(objects[i - 1].gameObject);
        }
      }

      objects = new List<PlaybookObject>();
      gizmo.SetActive(false);
    }

    public void ObjectSelected(PlaybookObject newObj)
    {

    }

    void Start()
    {
      gizmo.user = user;
      gizmo.OnObjectModified += user.LookAtTarget;
      gizmo.Create();
      gizmo.SetActive(false);
    }

  }
}
