using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using Color = UnityEngine.Color;

public class Heightmap : MonoBehaviour
{
    Texture2D toWork;
    Dictionary<Point, Angle> angles = new Dictionary<Point, Angle>();
    float highest = 0f;
    Texture2D worked;
    RenderTexture textureTest;
    public ComputeShader test;

    AngleOfPixel[] data;
    public struct AngleOfPixel
    {
        public Vector3 colorVector;
        public float angle;
    };
    public void process()
    {
        
        toWork = FindObjectOfType<NormalMap>().getTexture();
        textureTest = new RenderTexture(toWork.width, toWork.height, 24);
        textureTest.enableRandomWrite = true;
        test.SetTexture(0, "Result", textureTest);
        test.SetTexture(0, "Working", toWork);
        data = new AngleOfPixel[textureTest.width * textureTest.height];
        ComputeBuffer testBuffer = new ComputeBuffer(data.Length, (sizeof(float)) + (sizeof(float) * 3));

        testBuffer.SetData(data);
        test.SetBuffer(0, "stuffs", testBuffer);
        test.Dispatch(0, textureTest.width,textureTest.height, 1);

        testBuffer.GetData(data);

        testBuffer.Dispose();
        AngleOfPixel temp;
        for (int i = 0; i < 100; i++)
        {
            temp = data[i];
            Debug.Log(temp.colorVector);
            Debug.Log(temp.angle);
        }
        GetComponent<MeshRenderer>().material.mainTexture = textureTest;
        
         
    }

    public void work()
    {
        toWork = FindObjectOfType<NormalMap>().getTexture();
        worked = new Texture2D(toWork.width, toWork.height, TextureFormat.ARGB32, false);
        Color c;
        for (int x = 0; x < toWork.width; x++)
        {
            for (int y = 0; y < toWork.height; y++)
            {
                c = toWork.GetPixel(x, y);
                //rex = x, green = y, blue = z
                //maybe green and y reverse cause unity
                angles.Add(new Point(x, y), new Angle(new Vector3(c.r, c.g, c.b)));
                if (Vector3.Angle(Vector3.up, new Vector3(c.r, c.g, c.b)) > highest)
                {
                    highest = Vector3.Angle(Vector3.up, new Vector3(c.r, c.g, c.b));
                }
            }
        }
        foreach (Point point in angles.Keys)
        {
            worked.SetPixel(point.X, point.Y, Color.Lerp(Color.black, Color.white, angles[point].getAngle() / highest));
        }
        worked.Apply();
        GetComponent<MeshRenderer>().material.mainTexture = worked;
    }

    Texture2D export;
    public void saveImage()
    {
        RenderTexture.active = textureTest;
        export = new Texture2D(textureTest.width, textureTest.height, TextureFormat.ARGB32, false);
        export.ReadPixels(new Rect(0, 0, textureTest.width, textureTest.height), 0, 0);
        File.WriteAllBytes(@"D:\testHeight.png", export.EncodeToPNG());
    }
}
