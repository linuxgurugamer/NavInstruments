﻿#region NO_LOCALIZATION

#if false
using System;
using UnityEngine;
using System.Linq;

using var = NavInstruments.NavUtilLib.GlobalVariables;
using static NavUtilLib.RegisterToolbar;

namespace NavInstruments.NavUtilLib.Analog
{
    public class AnalogHSI : InternalModule
    {
        [KSPField]
        public string compassObject = "compassObject";

        [KSPField]
        public string crsRotObject = "crsRotObject";

        [KSPField]
        public string brgObject = "brgObject";

        [KSPField]
        public string locNeedleObject = "locNeedleObject";
        [KSPField]
        public float locMinDevMM = -111;
        [KSPField]
        public float locMaxDevMM = 111;
        [KSPField]
        public float locMMpDeg = 37;


        [KSPField]
        public string gsNeedleObject = "gsNeedleObject";
        [KSPField]
        public float gsMinDevMM = -150;
        [KSPField]
        public float gsMaxDevMM = 150;
        [KSPField]
        public float gsMMpDeg = 50;

        [KSPField]
        string dmeTenthsObject = "dmeTenthObject";
        [KSPField]
        string dmeOnesObject = "dmeOnesObject";
        [KSPField]
        string dmeTensObject = "dmeTensObject";
        [KSPField]
        string dmeHundredsObject = "dmeHundredsObject";

        [KSPField]
        float rotRateDeg = 80;

        [KSPField]
        float transRateMM = 30;

        //UI
        [KSPField]
        string hdgObject = "hdgObject";
        Transform hdgKnob;
        Quaternion hdgKnobInit;
        //InternalButton hdgBtn;

        InternalLabel rwyLabel;
        bool ranOnce = false;

        [KSPField]
        string crsObject = "crsObject";
        Transform crsKnob;
        Quaternion crsKnobInit;

        //[KSPField]
        //string rwyID = "runwayID";
        //InternalText rwyIDObject;



        Transform compass;
        Quaternion compassInit;
        //float compassCurrent;

        Transform locNeedle;
        Vector3 locInit;
        Quaternion locInitRot;
        float locCurrent;

        Transform gsNeedle;
        Vector3 gsInit;
        float gsCurrent;

        Transform crsBug;
        Quaternion crsBugInt;

        Transform brgBug;
        Quaternion brgBugInt;

        Transform[] dme = new Transform[4];
        Quaternion[] dmeInt = new Quaternion[4];


        public override void OnAwake()
        {
            //if (NavUtilLib.RegisterToolbar.Log.GetLogLevel() >KSP_Log.)
            if (Log.GetLogLevel() > KSP_Log.Log.LEVEL.INFO)
            {
                Transform[] t1 = internalProp.FindModelComponents<Transform>();
                foreach (Transform t0 in t1)
                {
                    Log.Info("NavUtil: found {0}", t0.name);
                }
            }

            Log.Info("NavUtil: Before compassObject");
            compass = internalProp.FindModelTransform(compassObject);
            compassInit = compass.transform.localRotation;

            Log.Info("NavUtil: Before brgObject");
            brgBug = internalProp.FindModelTransform(brgObject);
            brgBugInt = brgBug.transform.localRotation;

            Log.Info("NavUtil: Before locNeedleObject");
            locNeedle = internalProp.FindModelTransform(locNeedleObject);

            locNeedle.parent = brgBug;

            locInit = locNeedle.transform.localPosition;
            locInitRot = locNeedle.transform.localRotation;



            Log.Info("NavUtil: Before gsNeedleObject");
            gsNeedle = internalProp.FindModelTransform(gsNeedleObject);
            gsInit = gsNeedle.transform.localPosition;

            Log.Info("NavUtil: Before crsRotObject");
            crsBug = internalProp.FindModelTransform(crsRotObject);
            crsBugInt = crsBug.transform.localRotation;

            //UI
            crsKnob = internalProp.FindModelTransform(crsObject);
            hdgKnob = internalProp.FindModelTransform(hdgObject);
            crsKnobInit = crsKnob.transform.localRotation;
            hdgKnobInit = hdgKnob.transform.localRotation;

            InternalButton hdgBtn = InternalButton.Create(hdgKnob.gameObject);
            hdgBtn.OnDown(new InternalButton.InternalButtonDelegate(OnHdgBtnClick));



            InternalButton crsBtn = InternalButton.Create(crsKnob.gameObject);
            crsBtn.OnDown(new InternalButton.InternalButtonDelegate(OnCrsBtnClick));



            dme[0] = internalProp.FindModelTransform(dmeTenthsObject);
            dme[1] = internalProp.FindModelTransform(dmeOnesObject);
            dme[2] = internalProp.FindModelTransform(dmeTensObject);
            dme[3] = internalProp.FindModelTransform(dmeHundredsObject);

            dmeInt[0] = dme[0].transform.localRotation;
            dmeInt[1] = dme[1].transform.localRotation;
            dmeInt[2] = dme[2].transform.localRotation;
            dmeInt[3] = dme[3].transform.localRotation;

            Log.Info("NavUtil: Starting systems...");

            if (!NavUtilLib.GlobalVariables.Settings.navAidsIsLoaded)
                NavUtilLib.GlobalVariables.Settings.loadNavAids();

            if (!NavUtilLib.GlobalVariables.Materials.isLoaded)
                NavUtilLib.GlobalVariables.Materials.loadMaterials();

            Log.Info("NavUtil: Systems started successfully!");

        }

        private void OnHdgBtnClick()
        {
            Log.Info("OnHdgBtnClick, button: " + Event.current.button);
            if (Event.current.button == 0)
            {
                NavUtilLib.GlobalVariables.FlightData.rwyIdx++;
            }
            else
            {
                NavUtilLib.GlobalVariables.FlightData.rwyIdx--;
            }

            NavUtilLib.GlobalVariables.FlightData.rwyIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.rwyIdx, NavUtilLib.GlobalVariables.FlightData.currentBodyRunways.Count() - 1, 0);

            float rotAmt = ((float)NavUtilLib.GlobalVariables.FlightData.rwyIdx / (float)NavUtilLib.GlobalVariables.FlightData.currentBodyRunways.Count()) * 360;

            Log.Info("NavUtil: hdgKnob Rot " + rotAmt);

            hdgKnob.localRotation = hdgKnobInit * Quaternion.AngleAxis(rotAmt, Vector3.forward);

            rwyLabel.SetText(NavUtilLib.GlobalVariables.FlightData.currentBodyRunways[NavUtilLib.GlobalVariables.FlightData.rwyIdx].shortID);

            var.Audio.Instance.PlayClick();
        }

        private void OnCrsBtnClick()
        {
            Log.Info("OnCrsBtnClick, button: " + Event.current.button);
            if (Event.current.button == 0)
                NavUtilLib.GlobalVariables.FlightData.gsIdx++;
            else
                NavUtilLib.GlobalVariables.FlightData.gsIdx--;

            NavUtilLib.GlobalVariables.FlightData.gsIdx = NavUtilLib.Utils.indexChecker(NavUtilLib.GlobalVariables.FlightData.gsIdx, NavUtilLib.GlobalVariables.FlightData.gsList.Count() - 1, 0);

            crsKnob.localRotation = crsKnobInit * Quaternion.AngleAxis(((float)NavUtilLib.GlobalVariables.FlightData.gsIdx / (float)NavUtilLib.GlobalVariables.FlightData.gsList.Count()) * 360, Vector3.forward);

            var.Audio.Instance.PlayClick();
        }

        private void RunOnce()
        {
            //find internallabel
            Log.Info("NavUtil: iM look");
            InternalModule iM = internalProp.internalModules.First(m => m.GetType() == typeof(InternalLabel));

            Log.Info("NavUtil: iM found, casting...");

            rwyLabel = (InternalLabel)iM;

            Log.Info("NavUtil: iM Cast!");

            rwyLabel.SetText(NavUtilLib.GlobalVariables.FlightData.currentBodyRunways[NavUtilLib.GlobalVariables.FlightData.rwyIdx].shortID);

            ranOnce = true;
        }

        public override void OnUpdate()
        {
            if (!ranOnce) RunOnce();

            //if (!NavUtilLib.GlobalVariables.Audio.isLoaded)
            //NavUtilLib.GlobalVariables.Audio.initializeAudio(); //we don't even play audio, so screw initializing it when all it'll do is cause errors


            //Log.Info("NavUtil: pos " + rwyIDObject.transform.localPosition);

            //rwyIDObject.transform.localRotation.SetEulerRotation(rwyIDObject.transform.localRotation.eulerAngles.x + 1,
            //    rwyIDObject.transform.localRotation.eulerAngles.y + 2,
            //    rwyIDObject.transform.localRotation.eulerAngles.z + 4);



            //float deltaNum = new float();
            float rotateLim = new float();
            float transLim = new float();

            rotateLim = rotRateDeg * Time.deltaTime;
            transLim = transRateMM * Time.deltaTime;

            //Update Data
            NavUtilLib.GlobalVariables.FlightData.updateNavigationData();


            //bool locFlag;
            //bool bcFlag;
            //bool gsFlag;

            //locFlag = bcFlag = false;
            //gsFlag = true;
            //if (NavUtilLib.GlobalVariables.FlightData.locDeviation > 10 && NavUtilLib.GlobalVariables.FlightData.locDeviation < 170 || NavUtilLib.GlobalVariables.FlightData.locDeviation < -10 && NavUtilLib.GlobalVariables.FlightData.locDeviation > -170)
            //    locFlag = true;

            //if (NavUtilLib.GlobalVariables.FlightData.locDeviation < -90 || NavUtilLib.GlobalVariables.FlightData.locDeviation > 90)
            //{
            //    bcFlag = true;
            //}



            //Rotate the compass card (not subject to rate limits)?
            //compassCurrent = Mathf.Clamp((float)Utils.makeAngle0to360(FlightGlobals.ship_heading - compassCurrent), -rotateLim, rotateLim);
            compass.localRotation = compassInit * Quaternion.AngleAxis(-FlightGlobals.ship_heading, Vector3.forward);

            //Rotate the course bug (not subject to rate limits)?
            crsBug.localRotation = crsBugInt * Quaternion.AngleAxis(-FlightGlobals.ship_heading + (float)NavUtilLib.GlobalVariables.FlightData.selectedRwy.hdg, Vector3.forward);

            //BRG
            brgBug.localRotation = brgBugInt * Quaternion.AngleAxis(-FlightGlobals.ship_heading + (float)NavUtilLib.GlobalVariables.FlightData.bearing, Vector3.forward);

            //Translate LOC needle
            //locCurrent = Mathf.Clamp((NavUtilLib.GlobalVariables.FlightData.locDeviation - locCurrent) * locMMpDeg, -transLim, transLim); //number of mm to move
            //locCurrent = Mathf.Clamp(locCurrent,locMinDevMM,locMaxDevMM); //limit the range of the needle



            locCurrent = Mathf.Clamp(NavUtilLib.GlobalVariables.FlightData.locDeviation * locMMpDeg, locMinDevMM, locMaxDevMM);

            //locNeedle.localRotation = locInitRot * Quaternion.AngleAxis(-FlightGlobals.ship_heading + (float)NavUtilLib.GlobalVariables.FlightData.bearing, Vector3.forward);

            locNeedle.localPosition = locInit + Vector3.right * locCurrent / 1000;







            gsCurrent = Mathf.Clamp(NavUtilLib.GlobalVariables.FlightData.gsDeviation * gsMMpDeg, gsMinDevMM, gsMaxDevMM);
            gsNeedle.localPosition = gsInit + Vector3.down * gsCurrent / 1000;


            //deltaNum = Mathf.Clamp((NavUtilLib.GlobalVariables.FlightData.locDeviation - currentLoc) * locMMpDeg, -transLim, transLim); //number of mm to move
            //deltaNum = Mathf.Clamp(deltaNum + currentLoc,);
            //deltaNum = 


            //Rotate DME
            float numHelper = 0;
            float fDME = (float)NavUtilLib.GlobalVariables.FlightData.dme / 1000;
            numHelper = fDME % 1; //tenths
            float digit;

            dme[0].localRotation = dmeInt[0] * Quaternion.AngleAxis((numHelper) * 360, Vector3.left);

            if (numHelper <= .9f) //not rolling over
            {
                digit = (int)Math.Abs(fDME / 1 % 10);
                dme[1].localRotation = dmeInt[1] * Quaternion.AngleAxis(
    digit * 36,
    Vector3.left);

                digit = (int)Math.Abs(fDME / 10 % 10);
                dme[2].localRotation = dmeInt[2] * Quaternion.AngleAxis(
    digit * 36,
    Vector3.left);

                digit = (int)Math.Abs(fDME / 100 % 10);
                dme[3].localRotation = dmeInt[3] *
                    Quaternion.AngleAxis(digit * 36,
    Vector3.left);
            }


            else
            {
                digit = Math.Abs(fDME / 1 % 10);
                dme[1].localRotation = dmeInt[1] * Quaternion.AngleAxis(
                    AnalogGaugeUtils.numberRot(digit, numHelper),
                    Vector3.left);


                digit = Math.Abs(fDME / 10 % 10);
                dme[2].localRotation = dmeInt[2] * Quaternion.AngleAxis(
                    AnalogGaugeUtils.numberRot(digit, numHelper),
                    Vector3.left);

                digit = Math.Abs(fDME / 100 % 10);
                dme[3].localRotation = dmeInt[3] * Quaternion.AngleAxis(
                    AnalogGaugeUtils.numberRot(digit, numHelper),
                   Vector3.left);
            }

            //Translate GS needle

            //Translate BKCRS Marker

            //Rotate GS Flag

            //Rotate LOC Flag


            ;
        }

    }

    //public class NavUtilRadioStack : InternalModule
    //{
    //    //InternalLabel label;


    //    [KSPField]
    //    string nRwyObjectName = "nRwyObjectName";

    //    RenderTexture rt;


    //    public override void OnAwake()
    //    {


    ////        Log.fino("NavUtil: NavUtilRadioStack OnAwake()");

    ////        Log.Info("NavUtil: Looking for {0}", nRwyObjectName);

    ////        // = this.internalProp.FindModelTransform(nRwyObjectName);

    ////        Transform[] t1 = internalProp.FindModelComponents<Transform>();

    ////        //Transform[] t2 = InternalModel.FindObjectsOfType<Transform>();

    ////        //Log.Info("NavUtil: " + internalModel.FindModelTransform("nRwyObjectName").name);




    //////        foreach (Transform t in t2)
    //////{
    //////                    Log.Info("NavUtil: {0}", t.name);
    //////}


    ////        //Transform[] t1 = internalProp.FindModelTransforms();

    ////        Log.Info("NavUtil: Added some to array " + t1.Count());

    ////       // Transform t;

    ////        foreach (Transform t0 in t1)
    ////        {
    ////            Log.Info("NavUtil: found {0}", t0.name);

    ////            if (t0.name == nRwyObjectName)
    ////            {
    ////                //t = t0;




    ////                Log.Info("NavUtil: transform location {0}", t0.position);

    ////                bool b = false;



    ////                if (t0.gameObject == null)
    ////                    b = true;

    ////                Log.Info("NavUtil: gameObject null? {0}", b);

    ////                InternalButton nRwy = InternalButton.Create(t0.gameObject);

    ////                Log.Info("NavUtil: After .Create()");
    ////                //Log.Info("NavUtil: Found {0} hh", nRwyObjectName);

    ////                //nRwy = new InternalButton();




    ////                //Log.Info("NavUtil: initilized button");



    ////                nRwy.OnTap(new InternalButton.InternalButtonDelegate(nRwyClick));
    ////            }
    ////        }





    //        ;
    //    }

    //    public override void OnUpdate()
    //    {
    //        ;
    //    }

    //    private void nRwyClick()
    //    {
    //        Log.Info("NavUtil: nRwyClick()");
    //    }

    //}



    static class AnalogGaugeUtils
    {
        public static float numberRot(float value, float dec)
        {
            float amt = (int)(value / 1) * 36;

            if ((value % 1) <= .9f || (int)(value * 10) % 10 <= .9f)
            {
                return amt;
            }

            //if (dec > .9f)
            //{
            dec -= .9f;
            amt += dec * 360;
            //}

            return amt;
        }


        public static void single_Axis_Rotate(GameObject gameObject, Char axisOfRot, float amountDegs)
        {
            Vector3 axisVector = Vector3.zero;

            switch (axisOfRot)
            {
                case 'X':
                case 'x':
                    axisVector.x = 1;
                    break;

                case 'Y':
                case 'y':
                    axisVector.y = 1;
                    break;

                case 'Z':
                case 'z':
                    axisVector.z = 1;
                    break;

                default:
                    Log.Warn("NavUtilLib.AnalogGaugeUtils.single_Axis_Rotate: No axis found");
                    break;
            }

            gameObject.transform.Rotate(axisVector, amountDegs);
        }

        public static void single_Axis_Translate(GameObject gameObject, Char axisOfTrans, float amountMillimeters)
        {
            Vector3 axisVector = Vector3.zero;

            switch (axisOfTrans)
            {
                case 'X':
                case 'x':
                    axisVector.x = amountMillimeters;
                    break;

                case 'Y':
                case 'y':
                    axisVector.y = amountMillimeters;
                    break;

                case 'Z':
                case 'z':
                    axisVector.z = amountMillimeters;
                    break;

                default:
                    Log.Warn("NavUtilLib.AnalogGaugeUtils.single_Axis_Translate: No axis found");
                    break;
            }

            gameObject.transform.Translate(axisVector);
        }

    }
}

#endif
#endregion