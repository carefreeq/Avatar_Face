using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System;
namespace Avatar
{
    internal class JsonFaceData : IJsonFaceData
    {
        public Dictionary<string, Vector2> JsonData { get; private set; }
        public GenderType Gender { get; private set; }
        public int Age { get; private set; }
        public Vector3 Angle { get; private set; }
        public string Glass { get; private set; }
        public string[] JsonKeys { get; private set; }
        public JsonFaceData(JsonData data)
        {
            JsonData att = data["attributes"];
            Gender = (GenderType)Enum.Parse(typeof(GenderType), (string)att["gender"]["value"]);
            Age = (int)att["age"]["value"];
            float _az = AvatarTools.ReadJsonFloat(att["headpose"]["yaw_angle"]) * Mathf.Deg2Rad;
            float _ax = AvatarTools.ReadJsonFloat(att["headpose"]["pitch_angle"]) * Mathf.Deg2Rad;
            float _ay = AvatarTools.ReadJsonFloat(att["headpose"]["roll_angle"]) * Mathf.Deg2Rad;
            Angle = new Vector3(_ax, _ay, _az);
            //Glass = att["glass"]["value"].ToString();
            data = data["landmark"];
            JsonData = new Dictionary<string, Vector2>();
            JsonKeys = AvatarTools.GetJsonKeys(data);
            for (int i = 0; i < JsonKeys.Length; i++)
            {
                Vector2 v = new Vector2(AvatarTools.ReadJsonFloat(data[JsonKeys[i]]["x"]), AvatarTools.ReadJsonFloat(data[JsonKeys[i]]["y"]));
                JsonData.Add(JsonKeys[i], v);
            }
            //rewrite angle.z
            Vector2 v0 = JsonData[AvatarTools.FaceLeft];
            Vector2 v1 = JsonData[AvatarTools.FaceRight];
            float _angle = Vector2.Dot(Vector2.right, (v1 - (v0 + v1) / 2f).normalized);
            _angle = Mathf.Acos(_angle);
            _angle = v1.y > v0.y ? -_angle : _angle;
            Angle = new Vector3(_ax, _ay, _angle);
        }
    }
}
