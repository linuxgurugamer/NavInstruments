//NavUtilities by kujuman, © 2014. All Rights Reserved.

using UnityEngine;
using var = NavInstruments.NavUtilLib.GlobalVariables;
using ToolbarControl_NS;
using static NavUtilLib.RegisterToolbar;
using ClickThroughFix;

namespace NavInstruments
{
    public class KSPeHack { }
}

namespace NavInstruments.NavUtilLib
{
    [KSPAddon(KSPAddon.Startup.Flight, false)] //used to start up in flight, and be false
    public class NavUtilLibApp : MonoBehaviour
    {
        private void OnGUI()
        {
            //Log.Info("NavUtils: OnGUI()");


            if (var.Settings.isKSPGUIActive) // will hide GUI is F2 is pressed
            {
                if (var.Settings.hsiState) OnDraw();
                if (NavUtilLib.SettingsGUI.isActive) NavUtilLib.SettingsGUI.OnDraw();
                if (var.Settings.rwyEditorState) NavUtilGUI.RunwaysEditor.OnDraw();
                if (RunwayListGUI.isActive) RunwayListGUI.OnDraw();
            }
        }


        //this class is to help load textures via GameDatabase since we cannot use static classes

        //NavUtilLibApp app;

        //KSP.UI.Screens.ApplicationLauncherButton appButton;

        public bool isHovering = false;

        //private bool visible = false;

        //RUIPanelTabGroup pTG;

        //public GameObject anchor;

        //public GameObject cascadeBody;
        //public GameObject cascadeFooter;
        //public GameObject cascadeHeader;

        //public RUICascadingList cascadingList;

        //public UIButton hoverComponent;

        //public int maxHeight = 200;
        //public int minHeight = 100;

        //public UIInteractivePanel panel;

        //private System.Collections.Generic.List<bool> bList;




        //private IButton toolbarButton = null;

        ToolbarControl toolbarControl;


        private Rect windowPosition;
        private RenderTexture rt;

        private bool rwyHover = false;
        private bool gsHover = false;
        private bool closeHover = false;

        private Vector3 originalNavBallWaypointSize = Vector3.zero;
        private Vector3 originalIVANavBallWaypointSize = Vector3.zero;

        public void displayHSI()
        {
            Log.Debug("NavUtils: NavUtilLibApp.displayHSI()");

            if (!var.Settings.hsiState)
            {
                Activate(true);
                var.Settings.hsiState = true;
                Log.Debug("NavUtils: hsiState = " + var.Settings.hsiState);
            }
            else
            {
                Activate(false);

                var.Settings.hsiState = false;

                Log.Debug("NavUtils: hsiState = " + var.Settings.hsiState);
            }
        }



        public void Activate(bool state)
        {
            Log.Debug("NavUtils: NavUtilLibApp.Activate()");

            if (state)
            {
                rt = new RenderTexture(640, 640, 24, RenderTextureFormat.ARGB32);
                rt.Create();

                Log.Debug("NavUtil: Starting systems...");
                if (!var.Settings.navAidsIsLoaded)
                    var.Settings.loadNavAids();

                if (!var.Materials.isLoaded)
                    var.Materials.loadMaterials();

                //load settings to config
                //ConfigLoader.LoadSettings(var.Settings.settingsFileURL);

                //ConfigureCamera();
                windowPosition.x = var.Settings.hsiPosition.x;
                windowPosition.y = var.Settings.hsiPosition.y;


                Log.Debug("NavUtil: Systems started successfully!");
            }
            else
            {
                state = false;
                //RenderingManager.RemoveFromPostDrawQueue(3, OnDraw); //close the GUI
                var.Settings.hsiPosition.x = windowPosition.x;
                var.Settings.hsiPosition.y = windowPosition.y;

                ConfigLoader.SaveSettings();
            }
        }

        private void OnDraw()
        {
            Log.Debug("NavUtils: NavUtilLibApp.OnDraw()");

            Log.Debug("HSI: OnDraw()");
            if (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Flight || ((CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.IVA) && GlobalVariables.Settings.enableWindowsInIVA))
            {
                if ((windowPosition.xMin + windowPosition.width) < 20) windowPosition.xMin = 20 - windowPosition.width;
                if (windowPosition.yMin + windowPosition.height < 20) windowPosition.yMin = 20 - windowPosition.height;
                if (windowPosition.xMin > Screen.width - 20) windowPosition.xMin = Screen.width - 20;
                if (windowPosition.yMin > Screen.height - 20) windowPosition.yMin = Screen.height - 20;

                windowPosition = new Rect(windowPosition.x,
                     windowPosition.y,
                     (int)(var.Settings.hsiPosition.width * var.Settings.hsiGUIscale),
                     (int)(var.Settings.hsiPosition.height * var.Settings.hsiGUIscale)
                     );

                windowPosition = ClickThruBlocker.GUIWindow(-471466245, windowPosition, OnWindow, "Horizontal Situation Indicator");
            }
            Log.Debug(windowPosition.ToString());
        }

        private void DrawGauge(RenderTexture screen)
        {
            Log.Debug("NavUtils: NavUtilLibApp.DrawGauge()");

            var.FlightData.updateNavigationData();

            RenderTexture pt = RenderTexture.active;
            RenderTexture.active = screen;

            NavUtilLib.DisplayData.DrawHSI(screen, 1);

            //write text to screen
            //write runway info

            string runwayText = (var.FlightData.isINSMode() ? "INS" : "Runway") + ": " + var.FlightData.selectedRwy.ident;
            string glideslopeText = var.FlightData.isINSMode() ? "" : "Glideslope: " + string.Format("{0:F1}", var.FlightData.selectedGlideSlope) + "°  ";
            string elevationText = (var.FlightData.isINSMode() ? "Alt MSL" : "Elevation") + ": " + string.Format("{0:F0}", var.FlightData.selectedRwy.altMSL) + "m";

            runwayText = (rwyHover ? "→" : " ") + runwayText;
            glideslopeText = (gsHover ? "→" : " ") + glideslopeText;

            NavUtilLib.TextWriter.addTextToRT(screen, runwayText, new Vector2(20, screen.height - 40), var.Materials.Instance.whiteFont, .64f);
            NavUtilLib.TextWriter.addTextToRT(screen, glideslopeText + elevationText, new Vector2(20, screen.height - 64), var.Materials.Instance.whiteFont, .64f);

            NavUtilLib.TextWriter.addTextToRT(screen, NavUtilLib.Utils.numberFormatter((float)NavUtilLib.Utils.makeAngle0to360(FlightGlobals.ship_heading), true).ToString(), new Vector2(584, screen.height - 102), var.Materials.Instance.whiteFont, .64f);
            NavUtilLib.TextWriter.addTextToRT(screen, NavUtilLib.Utils.numberFormatter((float)NavUtilLib.Utils.makeAngle0to360(var.FlightData.bearing), true).ToString(), new Vector2(584, screen.height - 131), var.Materials.Instance.whiteFont, .64f);
            NavUtilLib.TextWriter.addTextToRT(screen, NavUtilLib.Utils.numberFormatter((float)NavUtilLib.Utils.makeAngle0to360(var.FlightData.selectedRwy.hdg), true).ToString(), new Vector2(35, screen.height - 124), var.Materials.Instance.whiteFont, .64f);
            NavUtilLib.TextWriter.addTextToRT(screen, NavUtilLib.Utils.numberFormatter((float)var.FlightData.dme / 1000, false).ToString(), new Vector2(45, screen.height - 563), var.Materials.Instance.whiteFont, .64f);

            if (closeHover)
                NavUtilLib.TextWriter.addTextToRT(screen, "    Close HSI", new Vector2(340, 15), var.Materials.Instance.whiteFont, .64f);

            RenderTexture.active = pt;
        }

        private void OnWindow(int WindowID)
        {
            Log.Debug("NavUtils: NavUtilLibApp.OnWindow()");

            Log.Debug("HSI: OnWindow()");



            Rect rwyBtn = new Rect(20 * var.Settings.hsiGUIscale,
                13 * var.Settings.hsiGUIscale,
                200 * var.Settings.hsiGUIscale,
                20 * var.Settings.hsiGUIscale);

            Rect gsBtn = new Rect(20 * var.Settings.hsiGUIscale,
                38 * var.Settings.hsiGUIscale,
                200 * var.Settings.hsiGUIscale,
                20 * var.Settings.hsiGUIscale);

            Rect closeBtn = new Rect(330 * var.Settings.hsiGUIscale,
                580 * var.Settings.hsiGUIscale,
                300 * var.Settings.hsiGUIscale,
                50 * var.Settings.hsiGUIscale);

            if (GUI.Button(closeBtn, new GUIContent("CloseBtn", "closeOn")))
            {
                //displayHSI();
                Log.Debug("CloseHSI");
                //appButton.SetFalse(true);
                toolbarControl.SetFalse(true);
                //goto CloseWindow;
            }

            if (GUI.tooltip == "closeOn")
                closeHover = true;
            else
                closeHover = false;


            if (GUI.Button(rwyBtn, new GUIContent("Next Runway", "rwyOn")) && !var.FlightData.isINSMode()) //doesn't let runway to be switched in INS mode
            {
                if (Event.current.alt)
                {
                    RunwayListGUI.show(windowPosition);
                }
                else
                {
                    int curIdx = var.FlightData.rwyIdx;
                    do
                    {
                        if (Event.current.button == 0)
                        {
                            var.FlightData.rwyIdx++;
                        }
                        else
                        {
                            var.FlightData.rwyIdx--;
                        }
                        var.FlightData.rwyIdx = NavUtilLib.Utils.indexChecker(var.FlightData.rwyIdx, var.FlightData.currentBodyRunways.Count - 1, 0);
                    } while (NavUtilLib.Utils.TooFarAway(var.FlightData.currentBodyRunways[var.FlightData.rwyIdx]) && 
                                curIdx != var.FlightData.rwyIdx);
                    if (curIdx == var.FlightData.rwyIdx)
                        ScreenMessages.PostScreenMessage ("No runway within visible distance", 5f, ScreenMessageStyle.UPPER_CENTER);
                }
            }

            if (GUI.tooltip == "rwyOn")
                rwyHover = true;
            else
                rwyHover = false;


            if (GUI.Button(gsBtn, new GUIContent("Next G/S", "gsOn")))
            {
                if (Event.current.button == 0)
                {
                    var.FlightData.gsIdx++;
                }
                else
                {
                    var.FlightData.gsIdx--;
                }

                var.FlightData.gsIdx = NavUtilLib.Utils.indexChecker(var.FlightData.gsIdx, var.FlightData.gsList.Count - 1, 0);
            }

            if (GUI.tooltip == "gsOn")
                gsHover = true;
            else
                gsHover = false;

            rt.Create();

            DrawGauge(rt);
            GUI.DrawTexture(new Rect(0, 0, windowPosition.width, windowPosition.height), rt, ScaleMode.ScaleToFit);

            //GUI.DrawTexture(new Rect(0, 0, windowPosition.width, windowPosition.height), var.Materials.Instance.overlay.mainTexture);

            GUI.DragWindow();
        }


        internal const string MODID = "NavInstruments";
        internal const string MODNAME = "NavInstruments";

        public const string DATADIR = "GameData/NavInstruments/PluginData/";
        public const string MODDIR = "NavInstruments/";
        public const string TOOLBARDIR = "PluginData/Textures/Toolbar/";

        void Awake()
        {
            Log.Debug("NavUtils: NavUtilLibApp.Awake()");

            //load settings to config
            ConfigLoader.LoadSettings();


            toolbarControl = gameObject.AddComponent<ToolbarControl>();
            toolbarControl.AddToAllToolbars(null,
                    null,
                    onAppLaunchHoverOn,
                    onAppLaunchHoverOff,
                    onAppLaunchEnable,
                    onAppLaunchDisable,
                    KSP.UI.Screens.ApplicationLauncher.AppScenes.FLIGHT,
                    MODID,
                    "NavLibButton",
                    MODDIR + TOOLBARDIR + "toolbarButton3838",
                    MODDIR + TOOLBARDIR + "toolbarButton",
                    MODNAME
                );
            toolbarControl.AddLeftRightClickCallbacks(onAppLaunchToggle, SettingsGUI.startSettingsGUI);
            GameEvents.onGUIApplicationLauncherUnreadifying.Add(onDestroy);
            GameEvents.onGameSceneLoadRequested.Add(onDestroy);

            GameEvents.onShowUI.Add(ShowGUI);
            GameEvents.onHideUI.Add(HideGUI);



            var.Settings.appInstance = this.GetInstanceID();
            var.Settings.appReference = this;

        }

        void Update()
        {

            NavWaypoint navWaypoint = NavWaypoint.fetch;

            //TODO optimize these searches
            if (navWaypoint.IsActive && CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.IVA)
            {
                InternalNavBall navBall = null;
                foreach (InternalProp prop in CameraManager.Instance.IVACameraActiveKerbal.InPart.internalModel.props)
                {
                    navBall = (InternalNavBall)prop.internalModules.Find(module => module.GetType().Equals(typeof(InternalNavBall)));
                    if (navBall != null)
                    {
                        break;
                    }
                }
                if (navBall != null)
                {
                    if (originalIVANavBallWaypointSize.Equals(Vector3.zero))
                    {
                        originalIVANavBallWaypointSize = navBall.navWaypointVector.localScale;
                    }
                    navBall.navWaypointVector.localScale = GlobalVariables.Settings.hideNavBallWaypoint ? Vector3.zero : originalIVANavBallWaypointSize;
                }
            }

            if (originalNavBallWaypointSize.Equals(Vector3.zero))
            {
                originalNavBallWaypointSize = navWaypoint.Visual.transform.localScale;
            }
            navWaypoint.Visual.transform.localScale = GlobalVariables.Settings.hideNavBallWaypoint ? Vector3.zero : originalNavBallWaypointSize;
        }

        /*private Transform findWaypointVisual() {
			Transform navBall = FlightUIModeController.Instance.navBall.transform.FindChild("IVAEVACollapseGroup");
			if (navBall == null) {
				return null;
			}
			Transform navBallVectors = navBall.FindChild("NavBallVectorsPivot");
			if (navBallVectors == null) {
				return null;
			}
			return navBallVectors.FindChild("NavWaypointVisual");
		}*/

        void ShowGUI()
        {
            var.Settings.isKSPGUIActive = true;
        }

        void HideGUI()
        {
            var.Settings.isKSPGUIActive = false;
        }




        public void onDestroy(GameScenes g)
        {
            Log.Debug("NavUtils: Destorying App 1");
            toolbarControl.OnDestroy();
            Destroy(toolbarControl);

            //GameEvents.onGUIApplicationLauncherReady.Remove(AddButton);

            //if (appButton != null)
            {
                Log.Debug("NavUtils: Destorying App 2");


                //save settings to config
                ConfigLoader.SaveSettings();

                var.Settings.hsiState = false;

                //KSP.UI.Screens.ApplicationLauncher.Instance.RemoveModApplication(appButton);
            }
        }



        //void OnGUIReady()
        //{
        //    Log.Debug("NavUtils: NavUtilLibApp.OnGUIReady()");

        //    if (KSP.UI.Screens.ApplicationLauncher.Ready && !var.Settings.useBlizzy78ToolBar)
        //    {
        //        appButton = KSP.UI.Screens.ApplicationLauncher.Instance.AddModApplication(
        //            onAppLaunchToggleOn,
        //            onAppLaunchToggleOff,
        //            onAppLaunchHoverOn,
        //            onAppLaunchHoverOff,
        //            onAppLaunchEnable,
        //            onAppLaunchDisable,
        //            KSP.UI.Screens.ApplicationLauncher.AppScenes.FLIGHT,
        //            (Texture)GameDatabase.Instance.GetTexture("KerbalScienceFoundation/NavInstruments/CommonTextures/toolbarButton3838", false)
        //          );
        //        ;
        //    }

        //    app = this;

        //    //panel = new UIInteractivePanel();
        //    //panel.draggable = true;
        //    //panel.index = 1;



        //}

        void onAppLaunchToggle()
        {
            Log.Debug("NavUtils: onAppLaunchToggleOn");
            if (isHovering && Event.current.alt)
                NavUtilLib.SettingsGUI.startSettingsGUI();
            else
                displayHSI();

            Log.Debug("NavUtils: onAppLaunchToggleOn End");
        }



#if false
        void onAppLaunchToggleOff()
        {
            Log.Debug("NavUtils: onAppLaunchToggleOff");
            if (isHovering && Event.current.alt)
                NavUtilLib.SettingsGUI.startSettingsGUI();
            else
                displayHSI();

        }

#endif
        void onAppLaunchHoverOn()
        {
            Log.Debug("onHover");

            isHovering = true;
        }
        void onAppLaunchHoverOff()
        {
            Log.Debug("offHover");
            isHovering = false;
        }
        void onAppLaunchEnable()
        {
            Log.Debug("NavUtils: onAppLaunchEnable");
        }
        void onAppLaunchDisable()
        {
            Log.Debug("NavUtils: onAppLaunchDisable");
        }

        bool isApplicationTrue()
        {
            return false;
        }

    }
}
