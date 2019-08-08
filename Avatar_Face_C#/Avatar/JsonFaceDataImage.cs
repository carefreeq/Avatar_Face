using UnityEngine;
using System.Collections.Generic;
namespace Avatar
{
    internal sealed class JsonFaceDataImage : IFaceImageData
    {
        public Dictionary<string, Vector2> DataImageData { get; private set; }
        public Texture2D DataImage { get; private set; }
        public JsonFaceDataImage(IJsonFaceData data, Texture2D tex)
        {
            Vector2 size = new Vector2((float)tex.width, (float)tex.height);
            DataImage = new Texture2D(tex.width, tex.height);
            DataImageData = new Dictionary<string, Vector2>();
            //left&right
            float sin = Mathf.Sin(data.Angle.z);
            float cos = Mathf.Cos(data.Angle.z);
            Matrix2x2 rota = new Matrix2x2(cos, sin, -sin, cos);
            //center&scale
            Vector2 center = data.JsonData[AvatarTools.FaceNose];
            center = new Vector2(center.x, size.y - center.y);
            //rota tex
            for (int x = 0; x < DataImage.width; x++)
                for (int y = 0; y < DataImage.height; y++)
                {
                    Vector2 v = new Vector2(x, y);
                    v -= center;
                    v *= rota;
                    v += center;
                    v = new Vector2(v.x / DataImage.width, v.y / DataImage.height);
                    Color color = tex.GetPixelBilinear(v.x, v.y);
                    DataImage.SetPixel(x, y, color);
                }
            DataImage.Apply();
            //rota point
            center = data.JsonData[AvatarTools.FaceNose];
            for (int i = 0; i < data.JsonKeys.Length; i++)
            {
                Vector2 v = data.JsonData[data.JsonKeys[i]];
                v -= center;
                v *= rota;
                v += center;
                v = new Vector2(v.x / size.x, 1 - v.y / size.y);
                DataImageData.Add(data.JsonKeys[i], v);
            }
        }
    }
}
