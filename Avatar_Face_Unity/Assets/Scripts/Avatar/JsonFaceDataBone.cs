using UnityEngine;
using System.Collections.Generic;
namespace Avatar
{
    internal sealed class JsonFaceDataBone : Dictionary<string, BoneInfo>
    {
        public JsonFaceDataBone(IJsonFaceData data)
        {
            //left&right
            Vector2 v0 = data.JsonData[AvatarTools.FaceLeft];
            Vector2 v1 = data.JsonData[AvatarTools.FaceRight];
            //center&scale
            Vector2 center = (v0 + v1) * 0.5f;
            float scale = (v0 - v1).magnitude;
            //sin&cos
            float sin = Mathf.Sin(data.Angle.z);
            float cos = Mathf.Cos(data.Angle.z);
            Matrix2x2 rotaMat = new Matrix2x2(cos, sin, -sin, cos);
            //rota point
            for (int i = 0; i < data.JsonKeys.Length; i++)
            {
                Vector2 v = -(data.JsonData[data.JsonKeys[i]] - center) / scale;
                v *= rotaMat;
                Add(data.JsonKeys[i], new BoneInfo() { position = v });
            }
        }
    }
}
