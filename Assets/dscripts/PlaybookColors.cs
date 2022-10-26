using System;
using UnityEngine;

namespace HaiThere.Playbook
{
	static class PlaybookColors
	{

		public static Color32 SetInactive(Color32 c)
		{
			return new Color32(c.r, c.g, c.b, 100);
		}

		public static Color32 Active = new Color32(68, 159, 208, 255);
		public static Color32 InActive = Color.gray;

		public static Color32 AxisX = Color.green;
		public static Color32 AxisY = Color.red;
		public static Color32 AxisZ = Color.blue;

		public static Color32 GetAxisColor(AxisType axis)
		{
			return axis switch
			{
				AxisType.X => AxisX,
				AxisType.Y => AxisY,
				AxisType.Z => AxisZ,
				_ => Active
			};
		}
	}
}