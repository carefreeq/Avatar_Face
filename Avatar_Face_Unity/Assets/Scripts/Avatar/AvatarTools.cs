using UnityEngine;
using System.Collections;
using LitJson;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Avatar
{
    internal delegate void PostCallBack(bool isDone, string json, Texture2D tex);
    /// <summary>
    /// 工具类
    /// </summary>
    public static class AvatarTools
    {

        /// <summary>
        /// AvatarData 文件路径
        /// </summary>
        public static string AvatarDataFolderPath { get { return Application.persistentDataPath + "/AvatarDatas"; } }
        /// <summary>
        /// AvatarData 文件扩展名
        /// </summary>
        public static readonly string AvatarDataExtension = ".avatar";
        internal static string GetAvatarDataFilePath(string name) { return AvatarTools.AvatarDataFolderPath + "/" + name + AvatarTools.AvatarDataExtension; }
        /// <summary>
        /// Face++ URL
        /// </summary>
        public static string FaceURL = @"https://api-cn.faceplusplus.com/facepp/v3/detect";
        /// <summary>
        /// Face++ API_Key
        /// </summary>
        public static string API_Key = null;
        /// <summary>
        /// Face++ API_Secret
        /// </summary>
        public static string API_Secret = null;
        #region face++ joint name
        internal static readonly string FaceName = "Face";
        internal static readonly string FaceLeft = "contour_left1";
        internal static readonly string FaceRight = "contour_right1";
        internal static readonly string FaceNose = "nose_tip";
        internal static readonly string FaceChin = "contour_chin";
        internal static readonly List<string> FaceBonesMap = new List<string>
        {
            "contour_left1","contour_right1",
            "contour_left2","contour_right2",
            "contour_left3","contour_right3",
            "contour_left4","contour_right4",
            "contour_left5","contour_right5",
            "contour_left6","contour_right6",
            "contour_left7","contour_right7",
            "contour_left8","contour_right8",
            "contour_left9","contour_right9",

            "mouth_left_corner","mouth_right_corner" ,
            "mouth_upper_lip_left_contour1","mouth_upper_lip_right_contour1" ,
            "mouth_upper_lip_left_contour2","mouth_upper_lip_right_contour2" ,
            "mouth_upper_lip_left_contour3","mouth_upper_lip_right_contour3" ,
            "mouth_lower_lip_left_contour1","mouth_lower_lip_right_contour1" ,
            "mouth_lower_lip_left_contour2","mouth_lower_lip_right_contour2" ,
            "mouth_lower_lip_left_contour3","mouth_lower_lip_right_contour3" ,

            "nose_left","nose_right" ,
            "nose_contour_left1","nose_contour_right1" ,
            "nose_contour_left2","nose_contour_right2" ,
            "nose_contour_left3","nose_contour_right3" ,

            "left_eye_right_corner","right_eye_left_corner" ,
            "left_eye_upper_right_quarter","right_eye_upper_left_quarter" ,
            "left_eye_top","right_eye_top" ,
            "left_eye_upper_left_quarter","right_eye_upper_right_quarter" ,
            "left_eye_left_corner","right_eye_right_corner" ,
            "left_eye_lower_left_quarter","right_eye_lower_right_quarter" ,
            "left_eye_bottom","right_eye_bottom",
            "left_eye_lower_right_quarter","right_eye_lower_left_quarter" ,
            "left_eye_pupil","right_eye_pupil",
            "left_eye_center","right_eye_center" ,

            "left_eyebrow_right_corner","right_eyebrow_left_corner",
            "left_eyebrow_upper_right_quarter","right_eyebrow_upper_left_quarter",
            "left_eyebrow_upper_middle","right_eyebrow_upper_middle",
            "left_eyebrow_upper_left_quarter","right_eyebrow_upper_right_quarter",
            "left_eyebrow_left_corner","right_eyebrow_right_corner" ,
            "left_eyebrow_lower_left_quarter","right_eyebrow_lower_right_quarter",
            "left_eyebrow_lower_middle","right_eyebrow_lower_middle" ,
            "left_eyebrow_lower_right_quarter","right_eyebrow_lower_left_quarter",

            "contour_chin",
            "mouth_upper_lip_top",
            "mouth_upper_lip_bottom",
            "mouth_lower_lip_top",
            "mouth_lower_lip_bottom",
            "nose_tip",
            "nose_contour_lower_middle",
        };
        #endregion

        internal static IEnumerator PostTex(Texture2D tex, PostCallBack func)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (AvatarTools.API_Key != null && AvatarTools.API_Secret != null)
                {
                    WWWForm form = new WWWForm();
                    form.AddField("api_key", AvatarTools.API_Key);
                    form.AddField("api_secret", AvatarTools.API_Secret);
                    form.AddBinaryData("image_file", tex.EncodeToPNG());
                    form.AddField("return_landmark", 1);
                    form.AddField("return_attributes", "gender,age,smiling,glass,headpose");
                    WWW www = new WWW(FaceURL, form);
                    AvatarDebug.Log("http=>post:start!");
                    yield return www;
                    if (www.error != null)
                    {
                        AvatarDebug.Log("http=>post:error!\n" + www.error, LogType.Error);
                        func(false, www.error, tex);
                    }
                    else
                    {
                        AvatarDebug.Log("http=>post:done!\n" + www.text);
                        func(true, www.text, tex);
                    }
                }
                else
                {
                    AvatarDebug.Log("http=>post:fild! API_Key & API_Secret is null!", LogType.Error);
                }
            }
            else
            {
                AvatarDebug.Log("http=>post:fild! internet disable!", LogType.Error);
            }
        }
        internal static float ReadJsonFloat(JsonData data)
        {
            float x = 0;
            if (data.IsDouble)
                x = (float)(double)data;
            else if (data.IsInt)
                x = (int)data;
            else
                Debug.LogWarning("read fail of type(float)!");
            return x;
        }
        internal static string[] GetJsonKeys(JsonData data)
        {
            string[] str = data.ToJson().Split(new string[] { "{", "}," }, StringSplitOptions.RemoveEmptyEntries);
            string[] _str = new string[str.Length / 2];
            for (int i = 0; i < _str.Length; i++)
                _str[i] = Regex.Match(str[i + i], "(?<=^\").*?(?=\":)").Value;
            return _str;
        }
        internal static JsonData ReadFaceJson(string data)
        {
            return JsonMapper.ToObject(data)["faces"][0];
        }


        /// <summary>
        /// 读取本地的Avatar数据列表
        /// </summary>
        public static AvatarData[] ReadAvtarDatas()
        {
            AvatarDebug.Log("ReadAvtarDatas start-----");
            List<AvatarData> avatats = new List<AvatarData>();

            if (Directory.Exists(AvatarTools.AvatarDataFolderPath))
            {
                DirectoryInfo dir = new DirectoryInfo(AvatarTools.AvatarDataFolderPath);
                FileInfo[] infos = dir.GetFiles();
                for (int i = 0; i < infos.Length; i++)
                {
                    if (infos[i].Extension == AvatarTools.AvatarDataExtension)
                    {
                        byte[] data = new byte[infos[i].Length];
                        using (FileStream fs = infos[i].Open(FileMode.Open))
                        {
                            fs.Read(data, 0, data.Length);
                        }
                        avatats.Add((AvatarData)ByteToObject(data));
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(AvatarTools.AvatarDataFolderPath);
            }
            AvatarDebug.Log("ReadAvtarDatas done! sum:" + avatats.Count);
            return avatats.ToArray();
        }
        /// <summary>
        /// 保存Avatar数据到本地
        /// </summary>
        public static void SaveAvatarData(AvatarData data)
        {
            AvatarDebug.Log("SaveAvatarData start-----");
            if (!Directory.Exists(AvatarTools.AvatarDataFolderPath))
                Directory.CreateDirectory(AvatarTools.AvatarDataFolderPath);
            string path = GetAvatarDataFilePath(data.GUID);
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                byte[] _data = ObjectToByte(data);
                fs.Write(_data, 0, _data.Length);
            }
            AvatarDebug.Log("SaveAvatarData done!");
        }
        /// <summary>
        /// 删除本地的Avatar数据
        /// </summary>
        public static void DeleteAvatarDatas(AvatarData data) 
        {
            File.Delete(AvatarTools.GetAvatarDataFilePath(data.GUID));
        }
        internal static byte[] ObjectToByte(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }
        internal static object ByteToObject(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return bf.Deserialize(ms);
            }
        }
        //tex manager
        private static Vector2 TransformUV(Vector3 pos, Bounds bound)
        {
            //transfrom z axis dir
            Vector3 _pos = new Vector3(pos.x, pos.z, pos.y);
            _pos = _pos - bound.min;
            return new Vector2(_pos.x / bound.size.x, _pos.z / bound.size.z);
        }
        internal static void FaceTexMapper(IFaceImageData imageData, IFaceData faceData, int width = 512, int height = 512)
        {
            Vector2 left = TransformUV(faceData.FaceBones[AvatarTools.FaceLeft].position, faceData.FaceMesh.bounds);
            Vector2 right = TransformUV(faceData.FaceBones[AvatarTools.FaceRight].position, faceData.FaceMesh.bounds);
            Vector2 nose = TransformUV(faceData.FaceBones[AvatarTools.FaceNose].position, faceData.FaceMesh.bounds);
            Vector2 chin = TransformUV(faceData.FaceBones[AvatarTools.FaceChin].position, faceData.FaceMesh.bounds);
            Vector2 center = nose;
            float x_scale = Vector2.Distance(right, left);
            float y_scale = Vector2.Distance((right + left) / 2f, chin);
            Texture2D tex = new Texture2D(width, height);
            //OutputTex = new Texture2D(base.FaceImageData.FinalImage.width, base.FaceImageData.FinalImage.height);
            Vector2 _left = imageData.DataImageData[AvatarTools.FaceLeft];
            Vector2 _right = imageData.DataImageData[AvatarTools.FaceRight];
            x_scale = x_scale / Vector2.Distance(_left, _right);
            y_scale = y_scale / Vector2.Distance((_left + _right) / 2f, imageData.DataImageData[AvatarTools.FaceChin]);
            Vector2 _center = imageData.DataImageData[AvatarTools.FaceNose];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Vector2 v = new Vector2((float)x / width, (float)y / height);
                    v -= center;
                    //asix x inverse
                    v = new Vector2(-v.x / x_scale, v.y / y_scale);
                    v += _center;
                    tex.SetPixel(x, y, imageData.DataImage.GetPixelBilinear(v.x, v.y));
                }
            tex.Apply();
            faceData.DataImage = tex;
        }
    }
}