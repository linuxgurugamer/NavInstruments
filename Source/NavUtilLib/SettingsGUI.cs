//NavUtilities by kujuman, © 2014. All Rights Reserved.

using KSP.Localization;
using System.Linq;
using UnityEngine;
using ClickThroughFix;

namespace NavInstruments.NavUtilLib
{
    public static class SettingsGUI
    {
        public static Rect winPos;
        public static bool isActive = false;

        public static void startSettingsGUI()
        {
            if (!isActive)
            {

                SettingsGUI.winPos = NavUtilLib.GlobalVariables.Settings.settingsGUI;

                if (!NavUtilLib.GlobalVariables.Settings.navAidsIsLoaded)
                {
                    NavUtilLib.GlobalVariables.Settings.loadNavAids();
                }
                isActive = !isActive;
            }
            else
            {

                NavUtilLib.GlobalVariables.Settings.settingsGUI = SettingsGUI.winPos;
                isActive = !isActive;
            }
        }


        public static void OnDraw()
        {
            if ((SettingsGUI.winPos.xMin + SettingsGUI.winPos.width) < 20) SettingsGUI.winPos.xMin = 20 - SettingsGUI.winPos.width;
            if (SettingsGUI.winPos.yMin + SettingsGUI.winPos.height < 20) SettingsGUI.winPos.yMin = 20 - SettingsGUI.winPos.height;
            if (SettingsGUI.winPos.xMin > Screen.width - 20) SettingsGUI.winPos.xMin = Screen.width - 20;
            if (SettingsGUI.winPos.yMin > Screen.height - 20) SettingsGUI.winPos.yMin = Screen.height - 20;
#if false
            SettingsGUI.winPos = ClickThruBlocker.GUIWindow(206574909, SettingsGUI.winPos, OnWindow, Localizer.Format("#LOC_NavInst_NavUtil_Settings"));
#else
            SettingsGUI.winPos = ClickThruBlocker.GUILayoutWindow(206574909, SettingsGUI.winPos, Window, Localizer.Format("#LOC_NavInst_NavUtil_Settings"));
#endif
        }

#if false
        private static void OnWindow(int winID)
        {
            if (GUI.Button(new Rect(5, 15, 65, 20), Localizer.Format("#LOC_NavInst_Previous")))
            {
                NavUtilLib.GlobalVariables.FlightData.rwyIdx--;
                NavUtilLib.GlobalVariables.FlightData.rwyIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.rwyIdx, NavUtilLib.GlobalVariables.FlightData.currentBodyRunways.Count() - 1, 0);
            }

            if (GUI.Button(new Rect(75, 15, 95, 20), Localizer.Format("#LOC_NavInst_Runway_list")))
            {
                RunwayListGUI.show(winPos);
            }

            if (GUI.Button(new Rect(175, 15, 65, 20), Localizer.Format("#LOC_NavInst_Next")))
            {
                NavUtilLib.GlobalVariables.FlightData.rwyIdx++;
                NavUtilLib.GlobalVariables.FlightData.rwyIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.rwyIdx, NavUtilLib.GlobalVariables.FlightData.currentBodyRunways.Count() - 1, 0);
            }

            if (GUI.Button(new Rect(5, 40, 115, 20), Localizer.Format("#LOC_NavInst_Previous_G_S")))
            {
                NavUtilLib.GlobalVariables.FlightData.gsIdx--;
                NavUtilLib.GlobalVariables.FlightData.gsIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.gsIdx, NavUtilLib.GlobalVariables.FlightData.gsList.Count() - 1, 0);
            }

            if (GUI.Button(new Rect(130, 40, 115, 20), Localizer.Format("#LOC_NavInst_Next_G_S")))
            {
                NavUtilLib.GlobalVariables.FlightData.gsIdx++;
                NavUtilLib.GlobalVariables.FlightData.gsIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.gsIdx, NavUtilLib.GlobalVariables.FlightData.gsList.Count() - 1, 0);
            }

            GUI.Label(new Rect(5, 75, 115, 25), Localizer.Format("#LOC_NavInst_HSI_GUI_scale"));

            if (GUI.Button(new Rect(130, 75, 100, 25), Localizer.Format("#LOC_NavInst_Default_Scale")))
            {
                NavUtilLib.GlobalVariables.Settings.hsiGUIscale = 0.5f;


            }

            if (GUI.Button(new Rect(5, 150, 115, 20), Localizer.Format("#LOC_NavInst_Custom_Rwys")))
                NavUtilGUI.RunwaysEditor.startGUI();

            //GUI.Label(new Rect(125, 150, 90, 20), "Popup in IVA?");

            GlobalVariables.Settings.hideNavBallWaypoint = GUI.Toggle(new Rect(30, 120, 240, 20), GlobalVariables.Settings.hideNavBallWaypoint, Localizer.Format("#LOC_NavInst_Hide_NavBall_waypoint_ico"));

            GlobalVariables.Settings.enableWindowsInIVA = GUI.Toggle(new Rect(125, 150, 120, 20), GlobalVariables.Settings.enableWindowsInIVA, Localizer.Format("#LOC_NavInst_Popup_in_IVA"));


            NavUtilLib.GlobalVariables.Settings.hsiGUIscale = GUI.HorizontalSlider(new Rect(5, 105, 240, 30), NavUtilLib.GlobalVariables.Settings.hsiGUIscale, 0.1f, 1.0f);

            GUI.DragWindow();
        }
#else
        private static void Window(int winID)
        {
            using (new GUILayout.HorizontalScope())
            {
                //GUILayout.BeginHorizontal();
                if (GUILayout.Button(Localizer.Format("#LOC_NavInst_Previous"), GUILayout.Width(65)))
                {
                    NavUtilLib.GlobalVariables.FlightData.rwyIdx--;
                    NavUtilLib.GlobalVariables.FlightData.rwyIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.rwyIdx, NavUtilLib.GlobalVariables.FlightData.currentBodyRunways.Count() - 1, 0);
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(Localizer.Format("#LOC_NavInst_Runway_list"), GUILayout.Width(95)))
                {
                    RunwayListGUI.show(winPos);
                }

                GUILayout.FlexibleSpace();
                if (GUILayout.Button(Localizer.Format("#LOC_NavInst_Next"), GUILayout.Width(65)))
                {
                    NavUtilLib.GlobalVariables.FlightData.rwyIdx++;
                    NavUtilLib.GlobalVariables.FlightData.rwyIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.rwyIdx, NavUtilLib.GlobalVariables.FlightData.currentBodyRunways.Count() - 1, 0);
                }
                //GUILayout.EndHorizontal();
            }
            GUILayout.Space(5);
            using (new GUILayout.HorizontalScope())
            {
                //GUILayout.BeginHorizontal();
                if (GUILayout.Button(Localizer.Format("#LOC_NavInst_Previous_G_S"), GUILayout.Width(115)))
                {
                    NavUtilLib.GlobalVariables.FlightData.gsIdx--;
                    NavUtilLib.GlobalVariables.FlightData.gsIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.gsIdx, NavUtilLib.GlobalVariables.FlightData.gsList.Count() - 1, 0);
                }

                if (GUILayout.Button(Localizer.Format("#LOC_NavInst_Next_G_S"), GUILayout.Width(115)))
                {
                    NavUtilLib.GlobalVariables.FlightData.gsIdx++;
                    NavUtilLib.GlobalVariables.FlightData.gsIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.gsIdx, NavUtilLib.GlobalVariables.FlightData.gsList.Count() - 1, 0);
                }
                //GUILayout.EndHorizontal();
            }

            GUILayout.Space(5);
            using (new GUILayout.HorizontalScope())
            {
                //GUILayout.BeginHorizontal();
                GUILayout.Label(Localizer.Format("#LOC_NavInst_HSI_GUI_scale"));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(Localizer.Format("#LOC_NavInst_Default_Scale"), GUILayout.Width(100)))
                {
                    NavUtilLib.GlobalVariables.Settings.hsiGUIscale = 0.5f;
                }
                GUILayout.FlexibleSpace();
                //GUILayout.EndHorizontal();
            }
            GUILayout.Space(5);
            using (new GUILayout.HorizontalScope())
            {
                //GUILayout.BeginHorizontal();
                NavUtilLib.GlobalVariables.Settings.hsiGUIscale =
                    GUILayout.HorizontalSlider(NavUtilLib.GlobalVariables.Settings.hsiGUIscale, 0.1f, 1.0f);
                //GUILayout.EndHorizontal();
            }
            GUILayout.Space(5);
            using (new GUILayout.HorizontalScope())
            {
                //GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                GlobalVariables.Settings.hideNavBallWaypoint = GUILayout.Toggle(GlobalVariables.Settings.hideNavBallWaypoint, Localizer.Format("#LOC_NavInst_Hide_NavBall_waypoint_ico"));

                //GUILayout.EndHorizontal();
            }
            GUILayout.Space(5);
            using (new GUILayout.HorizontalScope())
            {
                //GUILayout.BeginHorizontal();

                if (GUILayout.Button(Localizer.Format("#LOC_NavInst_Custom_Rwys"), GUILayout.Width(115)))
                    NavUtilGUI.RunwaysEditor.startGUI();

                //GUI.Label(new Rect(125, 150, 90, 20), "Popup in IVA?");

                GUILayout.FlexibleSpace();
                GlobalVariables.Settings.enableWindowsInIVA = GUILayout.Toggle(GlobalVariables.Settings.enableWindowsInIVA, Localizer.Format("#LOC_NavInst_Allow_popup_in_IVA"));
                //GUILayout.EndHorizontal();
            }
            GUILayout.Space(5);
            using (new GUILayout.HorizontalScope())
            {
                //GUILayout.BeginHorizontal();
                GlobalVariables.Settings.hideRunwaysTooFar = GUILayout.Toggle(GlobalVariables.Settings.hideRunwaysTooFar, Localizer.Format("#LOC_NavInst_Don_t_show_runways_furthe"));
                //GUILayout.EndHorizontal();
            }
            GUILayout.Space(5);
            using (new GUILayout.HorizontalScope())
            {
                //GUILayout.BeginHorizontal();
                GUI.enabled = (GlobalVariables.Settings.maxDistanceVisibleRunways > 5000);
                GUILayout.Label("    ");
                if (GUILayout.Button("<", GUILayout.Width(25)))
                {
                    GlobalVariables.Settings.maxDistanceVisibleRunways -= 1000f;
                }
                GUI.enabled = true;
                GUILayout.Label((GlobalVariables.Settings.maxDistanceVisibleRunways / 1000).ToString("F0") + " " + Localizer.Format("#LOC_NavInst_Km"));
                if (GUILayout.Button(">", GUILayout.Width(25)))
                {
                    GlobalVariables.Settings.maxDistanceVisibleRunways += 1000f;
                }
                GUILayout.FlexibleSpace();
                //GUILayout.EndHorizontal();
            }

            GUI.DragWindow();
        }

#endif

    }
}
