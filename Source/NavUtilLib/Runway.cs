//NavUtilities by kujuman, © 2014. All Rights Reserved.

using System;

namespace NavInstruments.NavUtilLib
{
	public class Runway
    {
        [KSPField]
        public string ident = "runwayID";

		[KSPField]
        public string shortID = "sID4";

        [KSPField]
        public float hdg = 90;

        [KSPField]
        public string body = "Kerbin";

        [KSPField]
        public float altMSL = 100;

        [KSPField]
		public float gsLatitude;

        [KSPField]
		public float gsLongitude;

        [KSPField]
		public float locLatitude;

        [KSPField]
		public float locLongitude;

        [KSPField]
		public float outerMarkerDist = 10000;

        [KSPField]
		public float middleMarkerDist = 2200;

        [KSPField]
		public float innerMarkerDist = 200;

		[KSPField]
		public bool custom = false;
        
        public bool isINSTarget = false; //true indicates that the runway is not the actual runway and is used as a target point for INS 

		public static Runway fallback() {
            Runway runway = new Runway
            {
                ident = "---",
                shortID = "---",
                hdg = 0,
                body = "",
                altMSL = 0,
                gsLatitude = 0,
                gsLongitude = 0,
                locLatitude = 0,
                locLongitude = 0,
                outerMarkerDist = 0,
                middleMarkerDist = 0,
                innerMarkerDist = 0
            };
            return runway;
		}
        
        public static Runway createFrom(ConfigNode node)
        {
            string rwy_ident = node.GetValue("ident");
            string customValue = node.GetValue("custom");

            Runway runway = new Runway
            {
                ident = rwy_ident,
                shortID = node.GetValue("shortID"),
                custom = customValue != null && bool.Parse(customValue),
                hdg = float.Parse(node.GetValue("hdg")),
                body = node.GetValue("body"),
                altMSL = float.Parse(node.GetValue("altMSL")),
                gsLatitude = float.Parse(node.GetValue("gsLatitude")),
                gsLongitude = float.Parse(node.GetValue("gsLongitude")),
                locLatitude = float.Parse(node.GetValue("locLatitude")),
                locLongitude = float.Parse(node.GetValue("locLongitude")),
                outerMarkerDist = float.Parse(node.GetValue("outerMarkerDist")),
                middleMarkerDist = float.Parse(node.GetValue("middleMarkerDist")),
                innerMarkerDist = float.Parse(node.GetValue("innerMarkerDist"))
            };

            if (runway.shortID.Length > 4)
                runway.shortID.Remove(4);
                
            return runway;
        }
        
        public ConfigNode ToConfigNode(bool custom = false)
        {
            ConfigNode rN = new ConfigNode
            {
                name = "Runway"
            };
            
            rN.AddValue("custom", custom);
            
            rN.AddValue("ident", this.ident);
            rN.AddValue("shortID", this.shortID);
            rN.AddValue("hdg", this.hdg);
            rN.AddValue("body", this.body);
            rN.AddValue("altMSL", this.altMSL);
            rN.AddValue("gsLatitude", this.gsLatitude);
            rN.AddValue("gsLongitude", this.gsLongitude);
            rN.AddValue("locLatitude", this.locLatitude);
            rN.AddValue("locLongitude", this.locLongitude);
            rN.AddValue("outerMarkerDist", this.outerMarkerDist);
            rN.AddValue("middleMarkerDist", this.middleMarkerDist);
            rN.AddValue("innerMarkerDist", this.innerMarkerDist);

            return rN;
        }
    }
}
