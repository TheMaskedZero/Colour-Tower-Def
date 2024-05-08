using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourCoordinateConverter : MonoBehaviour
{
    Color sRGB = new Color(0, 0, 0);
    Vector3 XYZ = new Vector3(0, 0, 0);
    Vector2 xy = new Vector2(0, 0);
    float X = 0;
    float Y = 0;
    float Z = 0;
    float[,] matrixd65 = {{3.2404542f, -1.5371385f, -0.4985314f},{-0.9692660f,  1.8760108f,  0.0415560f},{0.0556434f, -0.2040259f,  1.0572252f}};
    public Color ConvertxyTosRGB(Vector2 xy, float luminance = 0.3f)
    {
        XYZ = ConvertxyYToXYZ(xy, luminance);
        sRGB = ConvertXYZTosRGB(XYZ, matrixd65);

        return sRGB;
    }

    Vector3 ConvertxyYToXYZ(Vector2 xy, float Y)
    {
        if (xy[1] == 0)
        {
            return new Vector3(0, 0, 0);
        }
        X = (xy[0] * Y) / xy[1];
        Z = ((1 - xy[0] - xy[1]) * Y) / xy[1];
        XYZ = new Vector3(X, Y, Z);
        return XYZ;
    }

    Color ConvertXYZTosRGB(Vector3 XYZ, float[,] matrix)
    {
        float r = XYZ[0] * matrix[0, 0] + XYZ[1] * matrix[0, 1] + XYZ[2] * matrix[0, 2];
        float g = XYZ[0] * matrix[1, 0] + XYZ[1] * matrix[1, 1] + XYZ[2] * matrix[1, 2];
        float b = XYZ[0] * matrix[2, 0] + XYZ[1] * matrix[2, 1] + XYZ[2] * matrix[2, 2];
        Vector3 rgb = new Vector3(r, g, b);
        sRGB = new Color(r,g,b);

        return sRGB;
    }
    float GammaCorrection(float channel)
    {
        if (channel < 0.0031308f)
        {
            channel = channel * 12.92f;
        }
        else
        {
            channel = (Mathf.Pow(channel, (1f / 2.4f)) * 1.055f) - 0.055f;
        }


        return channel;
    }

}
