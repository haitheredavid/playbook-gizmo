using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HaiThere.Playbook
{
	public class PlaybookHub : MonoBehaviour
	{
		[SerializeField] PlayBookGizmo gizmo;

		[SerializeField] GameObject playBookObjectPrefab;

		[SerializeField] List<PlaybookObject> objects = new List<PlaybookObject>();

		const float floatAmount = 0.1f;

		public void CreateNewObject()
		{
			Debug.Log("Creating New Object");

			if (playBookObjectPrefab == null)
			{
				Debug.Log("Set a prefab first!");
				return;
			}

			var obj = Instantiate(playBookObjectPrefab).GetComponent<PlaybookObject>();
			// obj.transform.localPosition = new Vector3(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));
			obj.OnClicked += ObjectSelected;

			objects.Add(obj);
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
			gizmo.IsActive = false;
		}

		public void ObjectSelected(PlaybookObject obj)
		{
			gizmo.SetActiveObj(obj);
		}

		void OnEnable()
		{
			gizmo.IsActive = false;
		}

	}
}