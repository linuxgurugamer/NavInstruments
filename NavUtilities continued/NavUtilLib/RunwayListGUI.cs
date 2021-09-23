using System;
using UnityEngine;

namespace NavUtilLib
{
	public class RunwayListGUI
	{

		public static Rect winPos = new Rect(100, 100, 182, 360);
		public static bool isActive = false;
		public const int WINDOW_ID = 87694923;

		private static Vector2 rwyListVector = Vector2.zero;

		public static void show(Rect parentRect) {
			isActive = !isActive;
			float x = parentRect.xMin - winPos.width - 10;
			if (x < 0) {
				x = parentRect.xMax + 10;
			}
			float y = parentRect.yMin;
			if (y + winPos.height > Screen.height) {
				y = Screen.height - winPos.height;
			}
			winPos.x = x;
			winPos.y = y;
		}

		public static void OnDraw()
		{
			if ((winPos.xMin + winPos.width) < 20) winPos.xMin = 20 - winPos.width;
			if (winPos.yMin + winPos.height < 20) winPos.yMin = 20 - winPos.height;
			if (winPos.xMin > Screen.width - 20) winPos.xMin = Screen.width - 20;
			if (winPos.yMin > Screen.height - 20) winPos.yMin = Screen.height - 20;
				winPos = GUI.Window(WINDOW_ID, winPos, OnWindow, "Runway list");
		}

		public static void OnWindow(int winId) {
			rwyListVector = GUI.BeginScrollView(new Rect(10, 20, 170, 300), rwyListVector, new Rect(0, 0, 150, GlobalVariables.FlightData.currentBodyRunways.Count * 25));
			for (int i = 0; i < GlobalVariables.FlightData.currentBodyRunways.Count; i++) {
				if (GUI.Button(new Rect(2, (i * 25), 140, 20), GlobalVariables.FlightData.currentBodyRunways[i].ident)
					&& !GlobalVariables.FlightData.isINSMode()) {
					GlobalVariables.FlightData.rwyIdx = i;
					GlobalVariables.FlightData.updateNavigationData();
					isActive = false;
				}
			}
			GUI.EndScrollView();
			if (GUI.Button(new Rect(60, 320, 60, 30), "Close")) {
				isActive = false;
			}
			GUI.DragWindow();
		}
	}
}

