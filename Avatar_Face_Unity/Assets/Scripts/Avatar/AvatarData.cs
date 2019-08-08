using UnityEngine;
using System.Collections.Generic;
using System;

namespace Avatar
{
    /// <summary>
    /// AvatarData 数据信息类
    /// </summary>
    [Serializable]
    public class AvatarData
    {
        /// <summary>
        /// 识别ID
        /// </summary>
        public string GUID { get; private set; }
        /// <summary>
        /// 角色姓名
        /// </summary>
        public string AvatarName { get; set; }
        /// <summary>
        /// 角色性别
        /// </summary>
        public GenderType Gender { get; set; }
        /// <summary>
        /// 角色的年龄
        /// </summary>
        public int Age { get; set; }

        internal Dictionary<string, BoneInfo> FaceBonesData { get; set; }
        internal byte[] FaceImageData { get; set; }
        internal SkinnedInfo Hair { get; set; }
        internal SkinnedInfo UpperBody { get; set; }
        internal SkinnedInfo UpperCloth { get; set; }
        internal SkinnedInfo LowerBody { get; set; }
        internal SkinnedInfo LowerCloth { get; set; }
        internal SkinnedInfo Shoes { get; set; }
        internal AvatarData()
        {
            DateTime time = DateTime.Now;
            GUID = "avatar" + time.Day + time.Hour + time.Minute + time.Second;
            AvatarName = "小酷";
            FaceBonesData = new Dictionary<string, BoneInfo>();
            FaceImageData = new byte[0];
            Gender = 0;
            Age = 0;
        }
        internal AvatarData(AvatarObject avatar)
        {
            DateTime time = DateTime.Now;
            GUID = "avatar" + time.Day + time.Hour + time.Minute + time.Second;
            AvatarName = "小酷";
            FaceBonesData = avatar.FaceData.DataBones;
            if (avatar.FaceData.DataImage)
                FaceImageData = avatar.FaceData.DataImage.EncodeToPNG();
            Gender = avatar.Gender;
            Age = avatar.Age;
            Hair = new SkinnedInfo(avatar.Hair);
            UpperBody = new SkinnedInfo(avatar.UpperBody);
            UpperCloth = new SkinnedInfo(avatar.UpperCloth);
            LowerBody = new SkinnedInfo(avatar.LowerBody);
            LowerCloth = new SkinnedInfo(avatar.LowerCloth);
            Shoes = new SkinnedInfo(avatar.Shoes);
        }
    }
}