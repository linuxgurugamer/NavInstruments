//NavUtilities by kujuman, © 2014. All Rights Reserved.

using System;
using System.Collections.Generic;
using static NavUtilLib.RegisterToolbar;

namespace NavInstruments.NavUtilLib
{
    public static class ConfigLoader
    {
        static List<string> runways = new List<string>()
        {
            "defaultRunways.cfg",
            "customRunways.cfg",
            "HeliPads.cfg"
        };

        public const string DATADIR = "GameData/NavInstruments/PluginData/";
        public const string SETTINGS_FILE = "settings.cfg";
        //public const string DEFAULTRUNWAYS_FILE = "defaultRunways.cfg";
        public const string CUSTOMRUNWAYS_FILE = "customRunways.cfg";

        //public const string MAKINGHISTORYRUNWAYS_FILE = "makingHistoryRunways.cfg";
        //public const string HELIPADS_FILE = "HeliPads.cfg";


        public static System.Collections.Generic.List<Runway> GetRunwayListFromConfig()
        {
            System.Collections.Generic.List<Runway> r = new System.Collections.Generic.List<Runway>();

            //
            // Leave this here for anyone putting configs into other directories
            // Including configs which are conditional based on whether certain mods are installed or not
            //
            foreach (ConfigNode node in GameDatabase.Instance.GetConfigNodes("Runway"))
                r.Add(CreateRunwayFromNode(node));

            if (Expansions.ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
                runways.Add("makingHistoryRunways.cfg");

            foreach (var runway in runways)
            {
                ConfigNode rr = ConfigNode.Load(DATADIR + runway);
                if (rr != null)
                {
                    foreach (ConfigNode node in rr.GetNodes("Runway"))
                        r.Add(CreateRunwayFromNode(node));
                }

            }
#if false
            ConfigNode defaultRunways = ConfigNode.Load(DATADIR + DEFAULTRUNWAYS_FILE);
            if (defaultRunways != null)
            {
                foreach (ConfigNode node in defaultRunways.GetNodes("Runway"))
                    r.Add(CreateRunwayFromNode(node));
            }
            ConfigNode customRunways = ConfigNode.Load(DATADIR + CUSTOMRUNWAYS_FILE);
            if (customRunways != null)
            {
                foreach (ConfigNode node in customRunways.GetNodes("Runway"))
                    r.Add(CreateRunwayFromNode(node));
            }
#endif
            return r;
        }

        private static Runway CreateRunwayFromNode(ConfigNode node)
        {
            Log.Info("NavUtil: Found Runway Node");
            
            try
            {
                Runway rwy = Runway.createFrom(node);
                Log.Info("NavUtil: Found " + rwy.ident);
                return rwy;
            }
            catch (Exception)
            {
                Log.Info("NavUtil: Error loading runway");
                throw;
            }
        }

        public static void WriteCustomRunwaysToConfig(System.Collections.Generic.List<Runway> runwayList)
        {
            ConfigNode runways = new ConfigNode();
            foreach (Runway r in runwayList)
            {
                ConfigNode rN = new ConfigNode();

                rN.name = "NavUtilRunway";

                rN.AddValue("custom", true);

                rN.AddValue("ident", r.ident);
                rN.AddValue("shortID", r.shortID);
                rN.AddValue("hdg", r.hdg);
                rN.AddValue("body", r.body);
                rN.AddValue("altMSL", r.altMSL);
                rN.AddValue("gsLatitude", r.gsLatitude);
                rN.AddValue("gsLongitude", r.gsLongitude);
                rN.AddValue("locLatitude", r.locLatitude);
                rN.AddValue("locLongitude", r.locLongitude);

                rN.AddValue("outerMarkerDist", r.outerMarkerDist);
                rN.AddValue("middleMarkerDist", r.middleMarkerDist);
                rN.AddValue("innerMarkerDist", r.innerMarkerDist);

                runways.AddNode(rN);
            }

            runways.Save(DATADIR + CUSTOMRUNWAYS_FILE);

        }

        public static System.Collections.Generic.List<float> GetGlideslopeListFromConfig()
        {
            System.Collections.Generic.List<float> gsList = new System.Collections.Generic.List<float>();
            foreach (ConfigNode node in GameDatabase.Instance.GetConfigNodes("Glideslope"))
            {
                float gs = float.Parse(node.GetValue("glideslope"));
                gsList.Add(gs);
            }
            return gsList;
        }

        public static void LoadSettings()
        {
            Log.Info("NavUtil: Loading Settings");
            ConfigNode settingsFile = ConfigNode.Load(DATADIR + SETTINGS_FILE);
            if (settingsFile != null)
            {
                ConfigNode settings = null;
                if (settingsFile.TryGetNode("NavUtilSettings", ref settings))
                {
                    if (settings != null)
                    {
                        GlobalVariables.Settings.hsiGUIscale = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "guiScale", 0.5f);
                        GlobalVariables.Settings.enableFineLoc = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "enableFineLoc", true);
                        GlobalVariables.Settings.enableWindowsInIVA = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "enableWindowsInIVA", true);
                        GlobalVariables.Settings.loadCustom_rwyCFG = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "loadCustom_rwyCFG", true);
                        //GlobalVariables.Settings.useBlizzy78ToolBar = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "useBlizzy78ToolBar", false);
                        GlobalVariables.Settings.hsiPosition.x = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "hsiPositionX", 220f);
                        GlobalVariables.Settings.hsiPosition.y = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "hsiPositionY", 500f);
                        //GlobalVariables.Settings.hsiPosition.width = settings.GetValue<float>("hsiPositionWidth",???f);
                        //GlobalVariables.Settings.hsiPosition.height = settings.GetValue<float>("hsiPositionHeight", ???f);
                        GlobalVariables.Settings.rwyEditorGUI.x = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "rwyEditorGUIX", 387f);
                        GlobalVariables.Settings.rwyEditorGUI.y = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "rwyEditorGUIY", 132f);
                        //GlobalVariables.Settings.rwyEditorGUI.width = settings.GetValue<float>("rwyEditorGUIWidth", ???f);
                        //GlobalVariables.Settings.rwyEditorGUI.height = settings.GetValue<float>("rwyEditorGUIHeight", ???f);
                        GlobalVariables.Settings.settingsGUI.x = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "settingsGUIX", 75f);
                        GlobalVariables.Settings.settingsGUI.y = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "settingsGUIY", 75f);
                        GlobalVariables.Settings.hideNavBallWaypoint = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "hideNavBallWaypoint", false);


                        GlobalVariables.Settings.hideRunwaysTooFar = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "hideRunwaysTooFar", true);
                        GlobalVariables.Settings.maxDistanceVisibleRunways = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "maxDistanceVisibleRunways", 100000f);



                        //GlobalVariables.Settings.settingsGUI.width = settings.GetValue<float>("settingsGUIWidth", ???f);
                        //GlobalVariables.Settings.settingsGUI.height = settings.GetValue<float>("settingsGUIHeight", ???f);
                        {
#if DEBUG
                            bool debugMode = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings, "debug", true);
#else
                bool debugMode = SpaceTuxUtility.ConfigNodeUtils.SafeLoad(settings,"debug", false);
#endif
                            Log.SetLevel(debugMode ? KSP_Log.Log.LEVEL.INFO : KSP_Log.Log.LEVEL.ERROR);
                        }
                    }
                }
            }
        }

        public static void SaveSettings()
        {
            Log.Info("NavUtil: Saving Settings");
            //SETTINGS.Clear();

            ConfigNode sN = new ConfigNode();

            sN.AddValue("guiScale", GlobalVariables.Settings.hsiGUIscale);
            sN.AddValue("enableFineLoc", GlobalVariables.Settings.enableFineLoc);
            sN.AddValue("enableWindowsInIVA", GlobalVariables.Settings.enableWindowsInIVA);
            sN.AddValue("loadCustom_rwyCFG", GlobalVariables.Settings.loadCustom_rwyCFG);
            sN.AddValue("hideNavBallWaypoint", GlobalVariables.Settings.hideNavBallWaypoint);
            //sN.AddValue("useBlizzy78ToolBar", GlobalVariables.Settings.useBlizzy78ToolBar);

            sN.AddValue("hsiPositionX", GlobalVariables.Settings.hsiPosition.x);
            sN.AddValue("hsiPositionY", GlobalVariables.Settings.hsiPosition.y);
            //sN.AddValue("hsiPositionWidth", GlobalVariables.Settings.hsiPosition.width);
            //sN.AddValue("hsiPositionHeight", GlobalVariables.Settings.hsiPosition.height);
            sN.AddValue("rwyEditorGUIX", GlobalVariables.Settings.rwyEditorGUI.x);
            sN.AddValue("rwyEditorGUIY", GlobalVariables.Settings.rwyEditorGUI.y);
            //sN.AddValue("rwyEditorGUIWidth", GlobalVariables.Settings.rwyEditorGUI.width);
            //sN.AddValue("rwyEditorGUIHeight", GlobalVariables.Settings.rwyEditorGUI.height);
            sN.AddValue("settingsGUIX", GlobalVariables.Settings.settingsGUI.x);
            sN.AddValue("settingsGUIY", GlobalVariables.Settings.settingsGUI.y);
            //sN.AddValue("settingsGUIWidth", GlobalVariables.Settings.settingsGUI.width);
            //sN.AddValue("settingsGUIHeight", GlobalVariables.Settings.settingsGUI.height);


            sN.AddValue("hideRunwaysTooFar", GlobalVariables.Settings.hideRunwaysTooFar);
            sN.AddValue("maxDistanceVisibleRunways", GlobalVariables.Settings.maxDistanceVisibleRunways);


            sN.AddValue("debug", Log.GetLogLevel() > KSP_Log.Log.LEVEL.INFO);

            ConfigNode file = new ConfigNode();
            file.AddNode("NavUtilSettings", sN);
            file.Save(DATADIR + SETTINGS_FILE);
        }
    }
}

