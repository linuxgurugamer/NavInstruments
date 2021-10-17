//NavUtilities by kujuman, © 2014. All Rights Reserved.

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using FinePrint;
using static NavUtilLib.RegisterToolbar;

namespace NavInstruments.NavUtilLib
{
    namespace GlobalVariables
    {

        public static class Settings
        {
            //public static string gsFileURL = "GameData/KerbalScienceFoundation/NavInstruments/glideslopes.cfg";

            public static bool isKSPGUIActive = true;

            public static Rect hsiPosition = new Rect(50, 50, 640, 640);
            public static float hsiGUIscale = 0.5f;
            public static bool hsiState = false;

            public static Rect settingsGUI = new Rect(100, 50, 250, 180);

            public static Rect rwyEditorGUI = new Rect(50, 50, 450, 300);
            public static bool rwyEditorState = false;

            public static bool navAidsIsLoaded = false;

            public static bool enableFineLoc = true;

            public static bool loadCustom_rwyCFG = true;

            public static bool enableWindowsInIVA = true;

            public static bool hideNavBallWaypoint = false;

            public static bool hideRunwaysTooFar = true;
            public static double maxDistanceVisibleRunways = 100000;

            public static int appInstance;

            public static NavUtilLibApp appReference;

            /*public static void loadNavAids_not_working() {
				FlightData.rwyList.Clear();
				ConfigNode[] nodes = GameDatabase.Instance.GetConfigNodes("Runway");
				KSPLog.print("^^^^ NODES: " + nodes.Length);
				foreach (ConfigNode node in nodes) {
					Runway runway = new Runway();
					ConfigNode.LoadObjectFromConfig(runway, node);
					KSPLog.print("^^^^ RUNWAY: " + runway);
					FlightData.rwyList.Add(runway);
				}

				nodes = GameDatabase.Instance.GetConfigNodes("Glideslope");
				KSPLog.print("^^^^ GLIDE NODES: " + nodes.Length);
				foreach (ConfigNode node in nodes) {
					KSPLog.print("^^^^ GLIDE NODE: " + node);
					Glideslope glideslope = new Glideslope();
					KSPLog.print("^^^^ LOADED " + ConfigNode.LoadObjectFromConfig(glideslope, node));
					KSPLog.print("^^^^ GLIDESLOPE: " + glideslope);
					FlightData.gsList.Add(glideslope);
				}
			}*/

            public static void loadNavAids()
            {
                Log.Debug("NavUtil: Loading NavAid database...");
                FlightData.allRunways.Clear();
                FlightData.allRunways = ConfigLoader.GetRunwayListFromConfig();
                FlightData.gsList.Clear();
                FlightData.gsList = ConfigLoader.GetGlideslopeListFromConfig();

                FlightData.customRunways.Clear();
                FlightData.allRunways.ForEach(runway =>
                {
                    if (FlightGlobals.currentMainBody == null)
                    {
                        return;
                    }
                    if (FlightGlobals.currentMainBody.name == runway.body)
                    {
                        FlightData.currentBodyRunways.Add(runway);
                    }
                    if (runway.custom)
                    {
                        FlightData.customRunways.Add(runway);
                    }
                });

                /*DirectoryInfo folder = new DirectoryInfo(KSPUtil.ApplicationRootPath + "GameData/KerbalScienceFoundation/NavInstruments/Runways");

                if (folder.Exists)
                {
                    //FileInfo[] addlNavAidFiles = folder.GetFiles("*.rwy"); //this works great :D
                    FileInfo[] addlNavAidFiles = folder.GetFiles("*");


                    foreach (FileInfo f in addlNavAidFiles)
                    {
                        if ((f.Name.EndsWith("_rwy.cfg") && loadCustom_rwyCFG)|| f.Name.EndsWith(".rwy"))
                        {

                            if (enableDebugging)  Log.Info("NavUtil: found file " + f.Name.ToString());

                            if (f.Name == "custom.rwy" || (f.Name == "custom_rwy.cfg" && GlobalVariables.Settings.loadCustom_rwyCFG))
                            {
                                FlightData.customRunways.AddRange(NavUtilLib.ConfigLoader.GetRunwayListFromConfig("GameData/KerbalScienceFoundation/NavInstruments/Runways/" + f.Name));
                                if (enableDebugging)  Log.Info("NavUtil: Found " + f.Name + " with " + FlightData.customRunways.Count + " runway definitions");

                            }

                            if (enableDebugging) Log.Info("NavUtil: Found " + f.Name);

                            FlightData.rwyList.AddRange(NavUtilLib.ConfigLoader.GetRunwayListFromConfig("GameData/KerbalScienceFoundation/NavInstruments/Runways/" + f.Name));
                            //! FlightData.gsList.AddRange(NavUtilLib.ConfigLoader.GetGlideslopeListFromConfig("GameData/KerbalScienceFoundation/NavInstruments/Runways/" + f.Name));
                            //}
                        }
                    }
                }*/

                navAidsIsLoaded = true;
            }
        }

        public static class FlightData
        {
            public static List<Runway> allRunways = new List<Runway>();
            public static List<Runway> currentBodyRunways = new List<Runway>();
            public static int rwyIdx;

            public static List<float> gsList = new List<float>();
            public static int gsIdx;

            public static List<Runway> customRunways = new List<Runway>();
            public static int cRwyIdx;

            public static Runway selectedRwy;
            public static float selectedGlideSlope;
            public static Vessel currentVessel;
            public static CelestialBody currentBody = null;
            /// <summary>
            /// /////////
            /// </summary>
            private static double lastNavUpdateUT;
            public static void SetLastNavUpdateUT()
            {
                lastNavUpdateUT = Planetarium.GetUniversalTime();
            }
            public static double GetLastNavUpdateUT()
            {
                return lastNavUpdateUT;
            }

            public static double bearing;
            public static double dme;
            public static double elevationAngle;
            public static float locDeviation;
            public static float gsDeviation;
            public static float runwayHeading;

            public static bool fallback = false;

            private static Waypoint prevWaypoint = null;

            public static bool isINSMode()
            {
                return (selectedRwy != null) && selectedRwy.isINSTarget;
            }

            public static void updateNavigationData()
            {
                //see if information is current
                if (GetLastNavUpdateUT() != Planetarium.GetUniversalTime())
                {
                    if (currentBody == null || FlightGlobals.currentMainBody != currentBody)
                    {
                        rwyIdx = 0;
                        currentBody = FlightGlobals.currentMainBody;
                        currentBodyRunways.Clear();
                        for (int i = 0; i < allRunways.Count; i++)
                        {
                            if (allRunways[i].body == currentBody.name)
                            {
                                currentBodyRunways.Add(allRunways[i]);
                            }
                        }
                    }
                    selectedGlideSlope = gsList[gsIdx];
                    if (currentBodyRunways.Count == 0)
                    {
                        selectedRwy = null;
                        rwyIdx = 0;
                        fallback = true;
                    }
                    else
                    {
                        selectedRwy = currentBodyRunways[rwyIdx];
                        fallback = false;
                    }


                    //Since there seems to be no callback methods to determine whether waypoint has been set or changed, we have to refresh INS data on every update  
                    NavWaypoint navWaypoint = NavWaypoint.fetch;
                    if ((navWaypoint != null) && navWaypoint.IsActive && navWaypoint.Body == FlightGlobals.currentMainBody)
                    {
                        Waypoint waypoint = null;
                        if (prevWaypoint != null && navWaypoint.IsUsing(prevWaypoint))
                        {
                            waypoint = prevWaypoint;
                        }
                        else
                        {
                            foreach (Waypoint wp in FinePrint.WaypointManager.Instance().Waypoints)
                            {
                                if (navWaypoint.IsUsing(wp))
                                {
                                    waypoint = wp;
                                    break;
                                }
                            }
                            prevWaypoint = waypoint;
                        }

                        //If waypoint is fine then generate fake target runway every time
                        Runway insTarget = new Runway
                        {
                            isINSTarget = true,
                            ident = waypoint != null ? waypoint.name : navWaypoint.name,
                            body = navWaypoint.Body.name,
                            hdg = selectedRwy != null ? selectedRwy.hdg : 0,
                            altMSL = (float)(navWaypoint.Height + navWaypoint.Altitude),
                            locLatitude = (float)navWaypoint.Latitude,
                            locLongitude = (float)navWaypoint.Longitude,
                            gsLatitude = (float)navWaypoint.Latitude,
                            gsLongitude = (float)navWaypoint.Longitude
                        };
                        selectedRwy = insTarget;
                    }

                    currentVessel = FlightGlobals.ActiveVessel;
                    if (selectedRwy != null)
                    {
                        bearing = NavUtilLib.Utils.CalcBearingToBeacon(currentVessel, selectedRwy);
                        dme = NavUtilLib.Utils.CalcDistanceToBeacon(currentVessel, selectedRwy);
                        elevationAngle = NavUtilLib.Utils.CalcElevationAngle(currentVessel, selectedRwy);
                        //locDeviation = NavUtilLib.Utils.CalcLocalizerDeviation(bearing, selectedRwy);
                        locDeviation = (float)NavUtilLib.Utils.CalcLocalizerDeviation(currentVessel, selectedRwy);
                        gsDeviation = NavUtilLib.Utils.CalcGlideslopeDeviation(elevationAngle, selectedGlideSlope);

                        //
                        runwayHeading = (float)NavUtilLib.Utils.CalcProjectedRunwayHeading(currentVessel, selectedRwy);
                    }
                    else
                    {
                        bearing = 0;
                        dme = 0;
                        elevationAngle = 0;
                        locDeviation = 0;
                        gsDeviation = 0;
                        runwayHeading = 0;
                        selectedRwy = Runway.fallback();
                    }

                    SetLastNavUpdateUT();
                }
            }
        }


        public class Materials
        {
            private static Materials instance;
            private Materials() { }

            public static Materials Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Materials();
                    }
                    return instance;
                }
            }

            public static bool isLoaded = false;
            public Material overlay = null;
            public Material pointer = null;
            public Material headingCard = null;
            public Material NDBneedle = null;
            public Material course = null;
            public Material localizer = null;
            public Material mkrbcn = null;
            public Material flag = null;
            public Material back = null;
            public Material whiteFont = null;

            public Material AI_overlay = null;
            public Material AI_throttleBar = null;
            public Material AI_VSILine = null;
            public Material AI_Ladder = null;
            public Material AI_Radar = null;
            public Material AI_RadarDial = null;

            public static void loadMaterials()
            {
                Log.Info("NavUtilLib: Updating materials...");

                Materials.Instance.overlay = NavUtilGraphics.loadMaterial("hsi_overlay", 640, 640);
                Materials.Instance.pointer = NavUtilGraphics.loadMaterial("hsi_gs_pointer", 640, 24);
                Materials.Instance.headingCard = NavUtilGraphics.loadMaterial("hsi_large_heading_card", 501, 501);
                Materials.Instance.NDBneedle = NavUtilGraphics.loadMaterial("hsi_NDB_needle", 15, 501);
                Materials.Instance.course = NavUtilGraphics.loadMaterial("hsi_course_needle", 221, 481);
                Materials.Instance.localizer = NavUtilGraphics.loadMaterial("hsi_course_deviation_needle", 5, 251);
                Materials.Instance.mkrbcn = NavUtilGraphics.loadMaterial("hsi_markerIndicator", 175, 180);
                Materials.Instance.flag = NavUtilGraphics.loadMaterial("hsi_flags", 64, 64);
                Materials.Instance.back = NavUtilGraphics.loadMaterial("hsi_back", 32, 32);
                Materials.Instance.whiteFont = NavUtilGraphics.loadMaterial("white_font", 256, 256);
                Materials.Instance.AI_overlay = NavUtilGraphics.loadMaterial("AI_OVERLAY", 640, 640);
                Materials.Instance.AI_throttleBar = NavUtilGraphics.loadMaterial("AI_THROTTLEBAR", 27, 164);
                Materials.Instance.AI_VSILine = NavUtilGraphics.loadMaterial("AI_VSILINE", 33, 4);
                Materials.Instance.AI_Ladder = NavUtilGraphics.loadMaterial("AI_LADDER", 906, 2048);
                Materials.Instance.AI_Radar = NavUtilGraphics.loadMaterial("AI_RADAR", 179, 179);
                Materials.Instance.AI_RadarDial = NavUtilGraphics.loadMaterial("AI_RADARDIAL", 86, 39);

                isLoaded = true;
            }
        }

        public class Audio
        {
            internal static Audio Instance = null;
            //public static Audio Instance => _instance ?? (_instance = new Audio());
            //public static Audio Instance;
            public static bool isLoaded = false;

            public GameObject audioplayer;
            public AudioSource markerAudio;
            //public static AudioSource playOnce;
            private AudioClip audio_click;
            private AudioClip audio_outer;
            private AudioClip audio_middle;
            private AudioClip audio_inner;

            private bool isPlaying = false;

            public static void InitAudio(bool force = false)
            {
                if (isLoaded & !force)
                    return;
                Log.Info("InitAudio");
                Log.Info("Initializing Audio...");
                if (Instance == null)
                    Instance = new Audio();
                Log.Info("Audio instantiated, initializing now");

                Instance.audioplayer = new GameObject();
                Instance.markerAudio = new AudioSource();
                //playOnce = new AudioSource();

                Instance.audio_click = Instance.getAudio("click");
                Instance.audio_outer = Instance.getAudio("outer");
                Instance.audio_middle = Instance.getAudio("middle");
                Instance.audio_inner = Instance.getAudio("inner");

                try
                {
                    {
                        Instance.markerAudio = Instance.audioplayer.AddComponent<AudioSource>();
                        Instance.markerAudio.volume = GameSettings.UI_VOLUME;
                        Instance.markerAudio.panStereo = 0;
                        Instance.markerAudio.dopplerLevel = 0;
                        Instance.markerAudio.bypassEffects = true;
                        Instance.markerAudio.loop = true;
                        Instance.markerAudio.rolloffMode = AudioRolloffMode.Linear;
                        //markerAudio.transform.SetParent(FlightCamera.fetch.mainCamera.transform);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error Loading Audio");
                    Log.Exception("AudioSource", e);
                }

                isLoaded = true;

            }
            private Audio()
            {
                Log.Info("Audio Instantiator");
                //InitAudio();
            }

            public void PlayClick()
            {
                Log.Info("Click!");
                if (this.markerAudio == null)
                    InitAudio(true);
                if (this.markerAudio != null && this.audio_click != null)
                    this.markerAudio.PlayOneShot(this.audio_click, 0.5f);
                else
                    if (this.markerAudio == null) Log.Error("Error doing PlayClick");
            }

            public void PlayOuter()
            {
                Log.Info("DME outer");
                if (this.markerAudio == null)
                    InitAudio(true);
                if (this.markerAudio != null && this.audio_outer != null)
                {
                    this.markerAudio.PlayOneShot(this.audio_outer, 0.8f);
                    this.isPlaying = true;
                }
                else
                    if (this.markerAudio == null) Log.Error("Error doing PlayOuter");
            }

            public void PlayMiddle()
            {
                Log.Info("DME middle");
                if (this.markerAudio == null)
                    InitAudio(true);
                if (this.markerAudio != null && this.audio_middle != null)
                {
                    this.markerAudio.PlayOneShot(this.audio_middle, 0.5f);
                    this.isPlaying = true;
                }
                else
                    if (this.markerAudio == null) Log.Error("Error doing PlayMiddle");
            }

            public void PlayInner()
            {
                Log.Info("DME inner");
                if (this.markerAudio == null)
                    InitAudio(true);
                if (this.markerAudio != null && this.audio_inner != null)
                {
                    this.markerAudio.PlayOneShot(this.audio_inner, 0.5f);
                    this.isPlaying = true;
                }
                else
                    if (this.markerAudio == null) Log.Error("Error doing PlayInner");
            }

            public const string MODDIR = "NavInstruments";
            public const string AUDIODIR = "Audio";
            private AudioClip getAudio(string clipName)
            {
                string path = MODDIR + "/" + AUDIODIR + "/" + clipName;
                var a = GameDatabase.Instance.GetAudioClip(path);
                if (a == null)
                    Log.Info("getAudio, no audioclip found");
                return a;
            }

            public void Stop()
            {
                if (this.markerAudio == null)
                    InitAudio(true);
                if (this.markerAudio != null && this.isPlaying)
                {
                    this.markerAudio.Stop();
                    this.isPlaying = false;
                }
                else
                    if (this.markerAudio == null) Log.Error("Error doing Stop");
            }
        }
    }
}
