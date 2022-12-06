﻿//NavUtilities by kujuman, © 2014. All Rights Reserved.


using UnityEngine;
using mat = NavInstruments.NavUtilLib.GlobalVariables.Materials;
using ToolbarControl_NS;
using static NavUtilLib.RegisterToolbar;

namespace NavInstruments.NavUtilLib
{
    public static class NavUtilGraphics
    {
        public static RenderTexture drawCenterRotatedImage(float headingDeg, Vector2 centerPercent, Material mat, RenderTexture screen, float xOffset, float yOffset)
        {
            float w = (float)mat.mainTexture.width / (float)screen.width / 2;
            float h = (float)mat.mainTexture.height / (float)screen.height / 2;

            headingDeg = (float)Utils.makeAngle0to360(headingDeg);
            headingDeg = (float)Utils.CalcRadiansFromDeg(headingDeg);

            Vector2[] corners = new Vector2[4];

            corners[0] = new Vector2(-w + xOffset, -h + yOffset);
            corners[1] = new Vector2(+w + xOffset, -h + yOffset);
            corners[2] = new Vector2(+w + xOffset, +h + yOffset);
            corners[3] = new Vector2(-w + xOffset, +h + yOffset);

            for (int i = 0; i < corners.Length; i++)
            {
                float x = corners[i].x;
                corners[i].x = (Mathf.Cos(headingDeg) * corners[i].x) + (Mathf.Sin(headingDeg) * corners[i].y);

                corners[i].y = (Mathf.Cos(headingDeg) * corners[i].y) - (Mathf.Sin(headingDeg) * x);
            }

            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Color(Color.red);
            GL.Begin(GL.QUADS);

            GL.TexCoord2(0, 0);
            GL.Vertex3(centerPercent.x + corners[0].x, centerPercent.y + corners[0].y, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex3(centerPercent.x + corners[1].x, centerPercent.y + corners[1].y, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex3(centerPercent.x + corners[2].x, centerPercent.y + corners[2].y, 0);

            GL.TexCoord2(0, 1);
            GL.Vertex3(centerPercent.x + corners[3].x, centerPercent.y + corners[3].y, 0);

            GL.End();

            return screen;
        }

        public static RenderTexture drawMovedImage(Material mat, RenderTexture screen, Vector2 bottomLeftPercent, bool useCenterOfMat, bool fitToScreen)
        {
            float w = (float)mat.mainTexture.width / (float)screen.width;
            float h = (float)mat.mainTexture.height / (float)screen.height;

            if(fitToScreen)
            {
                w= screen.width;
                h = screen.height;
                useCenterOfMat = false;
            }

            if (useCenterOfMat)
            {
                bottomLeftPercent.x -= .5f * w;
                bottomLeftPercent.y -= .5f * h;
            }

            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Color(Color.red);
            GL.Begin(GL.QUADS);

            GL.TexCoord2(0, 0);
            GL.Vertex3(bottomLeftPercent.x, bottomLeftPercent.y, 0);

            GL.TexCoord2(0, 1);
            GL.Vertex3(bottomLeftPercent.x, h + bottomLeftPercent.y, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex3(w + bottomLeftPercent.x, h + bottomLeftPercent.y, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex3(w + bottomLeftPercent.x, bottomLeftPercent.y, 0);
            GL.End();

            return screen;
        }

        public static RenderTexture drawMovedImagePortion(Material mat, float bottom, float top, float left, float right, RenderTexture screen, Vector2 bottomLeftPercent, bool useCenterOfMat)
        {
            float w = (float)mat.mainTexture.width / (float)screen.width;
            float h = (float)mat.mainTexture.height / (float)screen.height;


            w *= (right - left);
            h *= (top - bottom);

            if (useCenterOfMat)
            {
                bottomLeftPercent.x -= .5f * w;
                bottomLeftPercent.y -= .5f * h;
            }

            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Color(Color.red);
            GL.Begin(GL.QUADS);

            GL.TexCoord2(left, bottom);
            GL.Vertex3(bottomLeftPercent.x, bottomLeftPercent.y, 0);

            GL.TexCoord2(left, top);
            GL.Vertex3(bottomLeftPercent.x, h + bottomLeftPercent.y, 0);

            GL.TexCoord2(right, top);
            GL.Vertex3(w + bottomLeftPercent.x, h + bottomLeftPercent.y, 0);

            GL.TexCoord2(right, bottom);
            GL.Vertex3(w + bottomLeftPercent.x, bottomLeftPercent.y, 0);

            GL.End();

            return screen;
        }


       // public const string DATADIR = "GameData/NavInstruments/PluginData/";
        public const string TEXTUREDIR = "Textures/";
        public static Material loadMaterial(string name, int width, int height)
		{
            Log.Info("NavUtilLib: Loading Material " + name);

            //Shader unlit = Shader.Find("KSP/Alpha/Unlit Transparent");
            Shader unlit = Shader.Find("Sprites/Default");

            Texture2D tex = new Texture2D(2, 2);
                ToolbarControl.LoadImageFromFile(ref tex, DATADIR + TEXTUREDIR + name);
            Material mat = new Material(unlit)
            {
                color = Color.white,
                //mainTexture = Asset.Texture2D.LoadFromFile(width, height, "Textures", name)
                mainTexture = tex
            };
            
            return mat;
        }
    }
}
