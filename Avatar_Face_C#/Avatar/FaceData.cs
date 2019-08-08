using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Avatar
{

    internal sealed class FaceData : IFaceData
    {
        public Dictionary<string, Transform> FaceBones { get; private set; }
        public Dictionary<string, BoneInfo> OrginBones { get { return orginBones; } set { orginBones = value; } }
        private Dictionary<string, BoneInfo> orginBones = new Dictionary<string, BoneInfo>();
        public Dictionary<string, BoneInfo> DataBones { get { return dataBones; } set { dataBones = value; if (DataBoneChange != null) DataBoneChange(this); } }
        private Dictionary<string, BoneInfo> dataBones = new Dictionary<string, BoneInfo>();
        public Texture2D DataImage { get { return dataImage; } set { dataImage = value; if (DataImageChange != null) DataImageChange(this); } }
        public Texture2D dataImage;
        public Mesh FaceMesh { get; private set; }
        public event Action<IFaceData> DataBoneChange;
        public event Action<IFaceData> DataImageChange;
        private float scale = 1;
        private Vector3 offset = new Vector3();
        private SkinnedMeshRenderer faceSkinnedMesh;

        public FaceData(Transform faceParent, SkinnedMeshRenderer face)
        {
            InitEvent();
            InitMesh(face);
            InitBones(faceParent);
        }
        public FaceData(Transform faceParent, SkinnedMeshRenderer face, Dictionary<string, BoneInfo> bone, bool needScale, Texture2D tex)
        {
            InitEvent();
            InitMesh(face);
            InitBones(faceParent);
            DataBones = needScale ? ReScaleBones(bone) : bone;
            DataImage = tex;
        }
        private void InitEvent()
        {
            DataBoneChange += (face) =>
            {
                RecalculateBone();
                faceSkinnedMesh.BakeMesh(FaceMesh);
                FaceMesh.RecalculateBounds();
                RecalculateUV();
            };
        }
        private void InitMesh(SkinnedMeshRenderer face)
        {
            FaceMesh = new Mesh();
            faceSkinnedMesh = face;
            faceSkinnedMesh.BakeMesh(FaceMesh);
            FaceMesh.RecalculateBounds();
        }
        private void InitBones(Transform faceParent)
        {

            FaceBones = new Dictionary<string, Transform>();
            OrginBones = new Dictionary<string, BoneInfo>();
            DataBones = new Dictionary<string, BoneInfo>();
            Transform[] ts = faceParent.GetComponentsInChildren<Transform>();
            foreach (var t in ts)
            {
                BoneInfo bone = new BoneInfo()
                {
                    position = t.localPosition
                };
                if (AvatarTools.FaceBonesMap.Contains(t.name))
                {
                    if (FaceBones.ContainsKey(t.name))
                    {
                        FaceBones[t.name] = t;
                        OrginBones[t.name] = bone;
                    }
                    else
                    {
                        FaceBones.Add(t.name, t);
                        OrginBones.Add(t.name, bone);
                    }
                }
            }
            Transform left = FaceBones[AvatarTools.FaceLeft];
            Transform right = FaceBones[AvatarTools.FaceRight];
            offset = (left.localPosition + right.localPosition) * 0.5f;
            scale = Vector3.Distance(left.localPosition, right.localPosition);
        }
        public void RecalculateBone(float t = 1)
        {
            foreach (var bone in DataBones)
            {
                if (FaceBones.ContainsKey(bone.Key))
                {
                    Vector3 v0 = OrginBones[bone.Key].position;
                    Vector2 point = bone.Value.position;
                    Vector2 v1 = Vector2.Lerp(v0, point, t);
                    Vector3 v2 = FaceBones[bone.Key].localPosition;
                    FaceBones[bone.Key].localPosition = new Vector3(v1.x, v1.y, v2.z);
                }
            }
        }
        public void RecalculateUV(int uv = 1)
        {
            List<Vector2> uvs = new List<Vector2>();
            for (int i = 0; i < FaceMesh.vertexCount; i++)
            {
                Vector3 pos = FaceMesh.vertices[i] - FaceMesh.bounds.min;
                //z axis dir up
                uvs.Add(new Vector2(pos.x / FaceMesh.bounds.size.x, pos.z / FaceMesh.bounds.size.z));
            }
            faceSkinnedMesh.sharedMesh.SetUVs(1, uvs);
        }
        private Dictionary<string, BoneInfo> ReScaleBones(Dictionary<string, BoneInfo> bones)
        {
            Dictionary<string, BoneInfo> newBones = new Dictionary<string, BoneInfo>();
            foreach (var bone in bones)
                newBones.Add(bone.Key, new BoneInfo() { position = bone.Value.position * scale + offset });
            return newBones;
        }
    }
}