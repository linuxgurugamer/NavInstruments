﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using UnityEngine;
using NavInstruments.NavUtilLib.GlobalVariables;
using NavInstruments.NavUtilLib;
using static NavUtilLib.RegisterToolbar;

namespace NavInstruments.NavUtilRPM
{
    class ModuleNavUtilsInfo : PartModule
    {
        new public void OnAwake()
        {
            Log.Info("NavUtils: ModuleNavUtilsInfo.OnAwake()");

            //load settings from config
            NavUtilLib.ConfigLoader.LoadSettings();
            Log.Info("NavUtilRPM.Awake.hsiState: " + NavUtilLib.GlobalVariables.Settings.hsiState);

            //load navigation data
            if (!NavUtilLib.GlobalVariables.Settings.navAidsIsLoaded)
                NavUtilLib.GlobalVariables.Settings.loadNavAids();  
        }

        //public void OnUpdate()
        //{
        //    ;
        //}

        //selectedGlideSlope, bearing, dme, elevationAngle, locDeviation, gsDeviation, runwayHeading
        public object NavInfo(string s)
        {
            Log.Info("NavUtils: ModuleNavUtilsInfo.NavInfo() " + s);

            FlightData.updateNavigationData();

            s = s.ToUpper();

            switch (s)
            {
                case "SELECTEDGLIDESLOPE":
				return FlightData.selectedGlideSlope;

                case "BEARING":
                    return FlightData.bearing;

                case "DME":
                    return FlightData.dme;

                case "ELEVATIONANGLE":
                    return FlightData.elevationAngle;

                case "LOCALIZERDEVIATION":
                    return FlightData.locDeviation;

                case "GLIDESLOPEDEVIATION":
                    return FlightData.gsDeviation;

                case "RUNWAYHEADING":
                    return FlightData.runwayHeading;

                case "SELECTEDRUNWAYALT":
                    return FlightData.selectedRwy.altMSL;

                case "SELECTEDRUNWAYIDENT":
                    return FlightData.selectedRwy.ident;

                case "SELECTEDRUNWAYSHORTID":
                    return FlightData.selectedRwy.shortID;

                case "SELECTEDRUNWAYLAT":
                    return FlightData.selectedRwy.locLatitude;

                case "SELECTEDRUNWAYLON":
                    return FlightData.selectedRwy.locLongitude;

                default:
                    return null;
            }
        }
    }
}
