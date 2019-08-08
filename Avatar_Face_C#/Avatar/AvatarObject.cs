using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Avatar
{
    /// <summary>
    /// 为模型添加Avatar功能,模型必须严格符合Avatar规范
    /// </summary>
    public class AvatarObject : MonoBehaviour
    {
        /// <summary>
        /// 角色脸部模型
        /// </summary>
        public SkinnedMeshRenderer Face { get { if (!face) face = transform.FindChild("Face").GetComponent<SkinnedMeshRenderer>(); return face; } private set { face = value; } }
        /// <summary>
        /// 角色头发模型
        /// </summary>
        public SkinnedMeshRenderer Hair { get { if (!hair) hair = transform.FindChild("Hair").GetComponent<SkinnedMeshRenderer>(); return hair; } private set { hair = value; } }
        /// <summary>
        /// 角色上身身体模型
        /// </summary>
        public SkinnedMeshRenderer UpperBody { get { if (!upperBody) upperBody = transform.FindChild("UpperBody").GetComponent<SkinnedMeshRenderer>(); return upperBody; } private set { upperBody = value; } }
        /// <summary>
        /// 角色上身布料模型
        /// </summary>
        public SkinnedMeshRenderer UpperCloth { get { if (!upperCloth) upperCloth = transform.FindChild("UpperCloth").GetComponent<SkinnedMeshRenderer>(); return upperCloth; } private set { upperCloth = value; } }
        /// <summary>
        /// 角色下身身体模型
        /// </summary>
        public SkinnedMeshRenderer LowerBody { get { if (!lowerBody) lowerBody = transform.FindChild("LowerBody").GetComponent<SkinnedMeshRenderer>(); return lowerBody; } private set { lowerBody = value; } }
        /// <summary>
        /// 角色下身布料模型
        /// </summary>
        public SkinnedMeshRenderer LowerCloth { get { if (!lowerCloth) lowerCloth = transform.FindChild("LowerCloth").GetComponent<SkinnedMeshRenderer>(); return lowerCloth; } private set { lowerCloth = value; } }
        /// <summary>
        /// 角色鞋子模型
        /// </summary>
        public SkinnedMeshRenderer Shoes { get { if (!shoes) shoes = transform.FindChild("Shoes").GetComponent<SkinnedMeshRenderer>(); return shoes; } private set { shoes = value; } }
        private SkinnedMeshRenderer face, hair, upperBody, upperCloth, lowerBody, lowerCloth, shoes;
        /// <summary>
        /// 角色骨骼信息
        /// </summary>
        public Dictionary<string, Transform> Bones
        {
            get
            {
                if (bones == null)
                {
                    bones = new Dictionary<string, Transform>();
                    Transform[] trans = transform.FindChild("Bip001").GetComponentsInChildren<Transform>();
                    foreach (Transform tran in trans)
                        bones.Add(tran.name, tran);
                }
                return bones;
            }
        }
        private Dictionary<string, Transform> bones;
        /// <summary>
        /// 性别
        /// </summary>
        public GenderType Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        internal IFaceData FaceData { get { if (faceData == null) faceData = new FaceData(transform, Face); return faceData; } private set { faceData = value; } }
        private IFaceData faceData;
        /// <summary>
        /// 角色的变化时间区间
        /// </summary>
        public float ChangeTime { get { return changeTime; } set { changeTime = value; } }
        [SerializeField]
        private float changeTime = 1;
        /// <summary>
        /// 获取结果回调委托
        /// </summary>
        public delegate void ResultCallBack(bool isDone);
        /// <summary>
        /// 获取结果回调事件
        /// </summary>
        public event ResultCallBack LoadResultEvent;
        /// <summary>
        /// 读取贴图到Face++，然后根据数据变化
        /// </summary>
        /// <param name="tex"></param>
        public void LoadFaceTex(Texture2D tex)
        {
            AvatarDebug.Log("start load texture:" + "width:" + tex.width + ",height:" + tex.height);
            StartCoroutine(AvatarTools.PostTex(tex, JsonManager));
        }
        private void JsonManager(bool isDone, string json, Texture2D tex)
        {
            if (LoadResultEvent != null)
                LoadResultEvent(isDone);
            if (!isDone)
                return;
            IJsonFaceData jsonData = new JsonFaceData(AvatarTools.ReadFaceJson(json));
            Gender = jsonData.Gender;
            Age = jsonData.Age;
            FaceData = new FaceData(transform, Face);
            Dictionary<string, BoneInfo> dataBone = new JsonFaceDataBone(jsonData);
            IFaceImageData imageData = new JsonFaceDataImage(jsonData, tex);
            FaceData = new FaceData(transform, Face, dataBone, true, null);
            AvatarTools.FaceTexMapper(imageData, FaceData);
            SetFaceImage(FaceData.DataImage);
            AvatarDebug.Log("LoadFaceTex done!");
            StartCoroutine(BoneChange(changeTime));
        }
        /// <summary>
        /// 读取一个Avatar数据
        /// </summary>
        /// <param name="data">Avatar数据</param>
        public void SetAvatarData(AvatarData data)
        {
            Texture2D tex = new Texture2D(0, 0);
            if (data.FaceImageData != null)
                tex.LoadImage(data.FaceImageData);
            else
                tex = null;
            FaceData = new FaceData(transform, Face, data.FaceBonesData, false, tex);
            SetFaceImage(FaceData.DataImage);
            Hair = data.Hair.GetSkinnedMesh(Bones, Hair);
            UpperBody = data.UpperBody.GetSkinnedMesh(Bones, UpperBody);
            UpperCloth = data.UpperCloth.GetSkinnedMesh(Bones, UpperCloth);
            LowerBody = data.LowerBody.GetSkinnedMesh(Bones, LowerBody);
            LowerCloth = data.LowerCloth.GetSkinnedMesh(Bones, LowerCloth);
            Shoes = data.Shoes.GetSkinnedMesh(Bones, Shoes);
            StartCoroutine(BoneChange(changeTime));
            AvatarDebug.Log("SetAvatarData done!");
        }
        /// <summary>
        /// 获取该Object的AvatarData，每次调用都会new一个对象
        /// </summary>
        /// <returns>返回一个新的AvatarData</returns>
        public AvatarData GetAvatarData()
        {
            AvatarData _avatar = new AvatarData(this);
            return _avatar;
        }
        /// <summary>
        /// 更换skinnedMesh
        /// </summary>
        /// <param name="origin">被更换的目标</param>
        /// <param name="target">更换目标</param>
        public void LoadSkinnedMesh(SkinnedMeshRenderer origin, SkinnedMeshRenderer target)
        {
            if (origin && target)
            {
                Transform[] bones = new Transform[target.bones.Length];
                for (int i = 0; i < bones.Length; i++)
                    bones[i] = Bones[target.bones[i].name];
                origin.sharedMesh = target.sharedMesh;
                origin.sharedMaterial = target.sharedMaterial;
                origin.bones = bones;
                origin.rootBone = Bones[target.rootBone.name];
            }
            else
            {
                AvatarDebug.Log("origin & target SkinnedMeshRenderer is null!", LogType.Error);
            }
        }
        void SetFaceImage(Texture2D tex)
        {
            Face.sharedMaterial.SetTexture("_SecondTex", tex);
        }
        IEnumerator BoneChange(float time)
        {
            float _time = 0;
            while (_time < time)
            {
                _time += Time.deltaTime;
                FaceData.RecalculateBone(_time / time);
                yield return new WaitForEndOfFrame();
            }
        }

#if UNITY_EDITOR
        public Texture2D tex;
        public SkinnedMeshRenderer[] HairMesh;
        public SkinnedMeshRenderer[] UpperMesh;
        public SkinnedMeshRenderer[] LowerMesh;
        private int hairIndex = 0, upperIndex = 0, lowerIndex = 0;
        void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 300, 100), "发送"))
            {
                LoadFaceTex(tex);
            }
            if (GUI.Button(new Rect(0, 150, 300, 100), "头发"))
            {
                hairIndex = (hairIndex + 1) % HairMesh.Length;
                LoadSkinnedMesh(Hair, HairMesh[hairIndex]);
            }
            if (GUI.Button(new Rect(0, 300, 300, 100), "上衣"))
            {
                upperIndex = (upperIndex + 1) % UpperMesh.Length;
                LoadSkinnedMesh(UpperCloth, UpperMesh[upperIndex]);
            }
            if (GUI.Button(new Rect(0, 450, 300, 100), "裤子"))
            {
                lowerIndex = (lowerIndex + 1) % LowerMesh.Length;
                LoadSkinnedMesh(LowerCloth, LowerMesh[lowerIndex]);
            }
            if (GUI.Button(new Rect(0, 600, 300, 100), "保存"))
            {
                AvatarTools.SaveAvatarData(GetAvatarData());
            }
            if (GUI.Button(new Rect(0, 750, 300, 100), "加载"))
            {
                SetAvatarData(AvatarTools.ReadAvtarDatas()[0]);
            }
        }
#endif
    }
}