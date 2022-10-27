using UnityEngine;

namespace HaiThere.Playbook
{
	public class PlaybookUser : MonoBehaviour
	{

		[SerializeField] Camera viewer;

		public Camera Viewer
		{
			get => viewer;
			set => viewer = value;
		}

	}
}