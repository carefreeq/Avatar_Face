using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Avatar
{
    /// <summary>
    /// 性别信息
    /// </summary>
    public enum GenderType : int
    {/// <summary>
     /// 未知
     /// </summary>
     Null,
     /// <summary>
     /// 男性
     /// </summary>
     Male,
     /// <summary>
     /// 女性
     /// </summary>
     Female
    }
    internal enum Axis : int
    { X, Y, Z }
    [Serializable]
    internal struct Vec2
    {
        public float x { get; set; }
        public float y { get; set; }
        public static Vector2[] GetVectors(Vec2[] vecs)
        {
            Vector2[] _vecs = new Vector2[vecs.Length];
            for (int i = 0; i < _vecs.Length; i++)
                _vecs[i] = vecs[i];
            return _vecs;
        }
        public static Vec2[] SetVectors(Vector2[] vecs)
        {
            Vec2[] _vecs = new Vec2[vecs.Length];
            for (int i = 0; i < _vecs.Length; i++)
                _vecs[i] = vecs[i];
            return _vecs;
        }

        public static implicit operator Vec2(Vector2 v)
        {
            Vec2 p = new Vec2();
            p.x = v.x;
            p.y = v.y;
            return p;
        }
        public static implicit operator Vector2(Vec2 p)
        {
            Vector2 v = new Vector2();
            v.x = p.x;
            v.y = p.y;
            return v;
        }
        public static implicit operator Vec2(Vector3 v)
        {
            Vec2 p = new Vec2();
            p.x = v.x;
            p.y = v.y;
            return p;
        }
        public static implicit operator Vector3(Vec2 p)
        {
            Vector3 v = new Vector3();
            v.x = p.x;
            v.y = p.y;
            v.z = 0f;
            return v;
        }
        public static Vec2 operator *(Vec2 p, float d)
        {
            return d * p;
        }
        public static Vec2 operator *(float d, Vec2 p)
        {
            p.x *= d;
            p.y *= d;
            return p;
        }
    }
    [Serializable]
    internal struct Vec3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public static Vector3[] GetVectors(Vec3[] vecs)
        {
            Vector3[] _vecs = new Vector3[vecs.Length];
            for (int i = 0; i < _vecs.Length; i++)
                _vecs[i] = vecs[i];
            return _vecs;
        }
        public static Vec3[] SetVectors(Vector3[] vecs)
        {
            Vec3[] _vecs = new Vec3[vecs.Length];
            for (int i = 0; i < _vecs.Length; i++)
                _vecs[i] = vecs[i];
            return _vecs;
        }


        public static implicit operator Vec3(Vector3 v)
        {
            Vec3 p = new Vec3();
            p.x = v.x;
            p.y = v.y;
            p.z = v.z;
            return p;
        }
        public static implicit operator Vector3(Vec3 p)
        {
            Vector3 v = new Vector3();
            v.x = p.x;
            v.y = p.y;
            v.z = p.z;
            return v;
        }
        public static implicit operator Vec3(Vector2 v)
        {
            Vec3 p = new Vec3();
            p.x = v.x;
            p.y = v.y;
            p.z = 0f;
            return p;
        }
        public static implicit operator Vector2(Vec3 p)
        {
            Vector2 v = new Vector2();
            v.x = p.x;
            v.y = p.y;
            return v;
        }
        public static Vec3 operator *(Vec3 p, float d)
        {
            return d * p;
        }
        public static Vec3 operator *(float d, Vec3 p)
        {
            p.x *= d;
            p.y *= d;
            p.z *= d;
            return p;
        }
    }
    [Serializable]
    internal struct Vec4
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }
        public static Vector4[] GetVectors(Vec4[] vecs)
        {
            Vector4[] _vecs = new Vector4[vecs.Length];
            for (int i = 0; i < _vecs.Length; i++)
                _vecs[i] = vecs[i];
            return _vecs;
        }
        public static Vec4[] SetVectors(Vector4[] vecs)
        {
            Vec4[] _vecs = new Vec4[vecs.Length];
            for (int i = 0; i < _vecs.Length; i++)
                _vecs[i] = vecs[i];
            return _vecs;
        }
        public static implicit operator Vec4(Vector4 v)
        {
            Vec4 p = new Vec4();
            p.x = v.x;
            p.y = v.y;
            p.z = v.z;
            p.w = v.w;
            return p;
        }
        public static implicit operator Vector4(Vec4 p)
        {
            Vector4 v = new Vector4();
            v.x = p.x;
            v.y = p.y;
            v.z = p.z;
            v.w = p.w;
            return v;
        }
        public static implicit operator Vec4(Vector3 v)
        {
            Vec4 p = new Vec4();
            p.x = v.x;
            p.y = v.y;
            p.z = v.z;
            p.w = 0f;
            return p;
        }
        public static implicit operator Vector3(Vec4 p)
        {
            Vector3 v = new Vector3();
            v.x = p.x;
            v.y = p.y;
            v.z = p.z;
            return v;
        }
        public static Vec4 operator *(Vec4 p, float d)
        {
            return d * p;
        }
        public static Vec4 operator *(float d, Vec4 p)
        {
            p.x *= d;
            p.y *= d;
            p.z *= d;
            p.w *= d;
            return p;
        }
    }
    [Serializable]
    internal struct BoneWeight_
    {
        public int boneIndex0 { get; set; }
        public int boneIndex1 { get; set; }
        public int boneIndex2 { get; set; }
        public int boneIndex3 { get; set; }
        public float weight0 { get; set; }
        public float weight1 { get; set; }
        public float weight2 { get; set; }
        public float weight3 { get; set; }
        public static BoneWeight[] GetBoneWeights(BoneWeight_[] bones)
        {
            BoneWeight[] _bones = new BoneWeight[bones.Length];
            for (int i = 0; i < _bones.Length; i++)
                _bones[i] = bones[i];
            return _bones;
        }
        public static BoneWeight_[] SetBoneWeights(BoneWeight[] bones)
        {
            BoneWeight_[] _bones = new BoneWeight_[bones.Length];
            for (int i = 0; i < _bones.Length; i++)
                _bones[i] = bones[i];
            return _bones;
        }

        public static implicit operator BoneWeight(BoneWeight_ bone)
        {
            BoneWeight _bone = new BoneWeight();
            _bone.boneIndex0 = bone.boneIndex0;
            _bone.boneIndex1 = bone.boneIndex1;
            _bone.boneIndex2 = bone.boneIndex2;
            _bone.boneIndex3 = bone.boneIndex3;
            _bone.weight0 = bone.weight0;
            _bone.weight1 = bone.weight1;
            _bone.weight2 = bone.weight2;
            _bone.weight3 = bone.weight3;
            return _bone;
        }
        public static implicit operator BoneWeight_(BoneWeight bone)
        {
            BoneWeight_ _bone = new BoneWeight_();
            _bone.boneIndex0 = bone.boneIndex0;
            _bone.boneIndex1 = bone.boneIndex1;
            _bone.boneIndex2 = bone.boneIndex2;
            _bone.boneIndex3 = bone.boneIndex3;
            _bone.weight0 = bone.weight0;
            _bone.weight1 = bone.weight1;
            _bone.weight2 = bone.weight2;
            _bone.weight3 = bone.weight3;
            return _bone;
        }
    }
    [Serializable]
    internal struct Matrix4x4_
    {
        public float m00;
        public float m01;
        public float m02;
        public float m03;
        public float m10;
        public float m11;
        public float m12;
        public float m13;
        public float m20;
        public float m21;
        public float m22;
        public float m23;
        public float m30;
        public float m31;
        public float m32;
        public float m33;
        public static Matrix4x4[] GetMatrix4x4s(Matrix4x4_[] mats)
        {
            Matrix4x4[] _mats = new Matrix4x4[mats.Length];
            for (int i = 0; i < _mats.Length; i++)
                _mats[i] = mats[i];
            return _mats;
        }
        public static Matrix4x4_[] SetMatrix4x4s(Matrix4x4[] mats)
        {
            Matrix4x4_[] _mats = new Matrix4x4_[mats.Length];
            for (int i = 0; i < _mats.Length; i++)
                _mats[i] = mats[i];
            return _mats;
        }

        public static implicit operator Matrix4x4(Matrix4x4_ mat)
        {
            Matrix4x4 _mat = new Matrix4x4();
            _mat.m00 = mat.m00;
            _mat.m01 = mat.m01;
            _mat.m02 = mat.m02;
            _mat.m03 = mat.m03;
            _mat.m10 = mat.m10;
            _mat.m11 = mat.m11;
            _mat.m12 = mat.m12;
            _mat.m13 = mat.m13;
            _mat.m20 = mat.m20;
            _mat.m21 = mat.m21;
            _mat.m22 = mat.m22;
            _mat.m23 = mat.m23;
            _mat.m30 = mat.m30;
            _mat.m31 = mat.m31;
            _mat.m32 = mat.m32;
            _mat.m33 = mat.m33;
            return _mat;
        }
        public static implicit operator Matrix4x4_(Matrix4x4 mat)
        {
            Matrix4x4_ _mat = new Matrix4x4_();
            _mat.m00 = mat.m00;
            _mat.m01 = mat.m01;
            _mat.m02 = mat.m02;
            _mat.m03 = mat.m03;
            _mat.m10 = mat.m10;
            _mat.m11 = mat.m11;
            _mat.m12 = mat.m12;
            _mat.m13 = mat.m13;
            _mat.m20 = mat.m20;
            _mat.m21 = mat.m21;
            _mat.m22 = mat.m22;
            _mat.m23 = mat.m23;
            _mat.m30 = mat.m30;
            _mat.m31 = mat.m31;
            _mat.m32 = mat.m32;
            _mat.m33 = mat.m33;
            return _mat;
        }
    }
    [Serializable]
    internal struct Color_
    {
        public float r;
        public float g;
        public float b;
        public float a;
        public static implicit operator Color(Color_ col)
        {
            return new Color(col.r, col.g, col.b, col.a);
        }
        public static implicit operator Color_(Color col)
        {
            return new Color_() { r = col.r, g = col.g, b = col.b, a = col.a };
        }
    }
    /// <summary>
    /// 骨骼信息
    /// </summary>
    [Serializable]
    internal struct BoneInfo
    {
        public Vec3 position;
    }
    /// <summary>
    /// 蒙皮信息
    /// </summary>
    [Serializable]
    internal struct SkinnedInfo
    {
        public string name { get; set; }
        public string[] bonesName { get; set; }
        public string rootBoneName { get; set; }
        public byte[] texture { get; set; }
        public Color_ color { get; set; }

        //mesh info
        public Vec3[] vertices { get; set; }
        public Vec3[] normals { get; set; }
        public Vec4[] tangents { get; set; }
        public int[] triangles { get; set; }
        public Vec2[] uv { get; set; }
        public BoneWeight_[] boneWeights { get; set; }
        public Matrix4x4_[] bindposes { get; set; }
        public SkinnedMeshRenderer GetSkinnedMesh(Dictionary<string, Transform> bones, SkinnedMeshRenderer skin)
        {
            Mesh _mesh = new Mesh();
            _mesh.vertices = Vec3.GetVectors(vertices);
            _mesh.normals = Vec3.GetVectors(normals);
            _mesh.tangents = Vec4.GetVectors(tangents);
            _mesh.uv = Vec2.GetVectors(uv);
            _mesh.boneWeights = BoneWeight_.GetBoneWeights(boneWeights);
            _mesh.bindposes = Matrix4x4_.GetMatrix4x4s(bindposes);
            _mesh.triangles = triangles;
            _mesh.RecalculateBounds();
            skin.sharedMesh = _mesh;

            Texture2D tex = new Texture2D(0, 0);
            if (texture != null)
                tex.LoadImage(texture);
            else
                tex = null;
            skin.material.mainTexture = tex;
            skin.material.color = color;

            skin.name = name;
            skin.rootBone.name = rootBoneName;
            List<Transform> _bones = new List<Transform>();
            for (int i = 0; i < bonesName.Length; i++)
            {
                string _name = bonesName[i];
                if (bones.ContainsKey(_name))
                {
                    _bones.Add(bones[_name]);
                }
                else
                {
                    AvatarDebug.Log("bones name is null!");
                }
            }
            skin.bones = _bones.ToArray();
            return skin;
        }
        public SkinnedInfo(SkinnedMeshRenderer skin)
        {
            Mesh _mesh = skin.sharedMesh;
            vertices = Vec3.SetVectors(_mesh.vertices);
            normals = Vec3.SetVectors(_mesh.normals);
            tangents = Vec4.SetVectors(_mesh.tangents);
            uv = Vec2.SetVectors(_mesh.uv);
            boneWeights = BoneWeight_.SetBoneWeights(_mesh.boneWeights);
            bindposes = Matrix4x4_.SetMatrix4x4s(_mesh.bindposes);
            triangles = _mesh.triangles;
            _mesh.RecalculateBounds();
            skin.sharedMesh = _mesh;

            color = skin.material.color;
            name = skin.name;
            rootBoneName = skin.rootBone.name;
            bonesName = new string[skin.bones.Length];
            for (int i = 0; i < bonesName.Length; i++)
                bonesName[i] = skin.bones[i].name;
            Texture2D matTex = (Texture2D)skin.material.mainTexture;
            if (matTex)
            {
                Texture2D tex = new Texture2D(matTex.width, matTex.height);
                try
                {
                    tex.SetPixels(matTex.GetPixels());
                    tex.Apply();
                    texture = tex.EncodeToPNG();
                }
                catch
                {
                    texture = null;
                    AvatarDebug.Log("mainTexture is not read/write!");
                }
            }
            else
            {
                texture = null;
                AvatarDebug.Log("mainTexture is null!");
            }
        }
    }
    internal struct Matrix2x2
    {
        float m00, m01, m10, m11;
        public Matrix2x2(float m00, float m01, float m10, float m11)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m10 = m10;
            this.m11 = m11;
        }
        public float this[int index]
        {
            get
            {
                if (index == 0)
                    return m00;
                if (index == 1)
                    return m01;
                if (index == 2)
                    return m10;
                if (index == 3)
                    return m11;
                throw new IndexOutOfRangeException("Invalid matrix2x2 index!");
            }
            set
            {
                if (index > 3)
                    throw new IndexOutOfRangeException("Invalid matrix2x2 index!");
                if (index == 0)
                    m00 = value;
                if (index == 1)
                    m01 = value;
                if (index == 2)
                    m10 = value;
                if (index == 3)
                    m11 = value;
            }
        }
        public float this[int row, int column]
        {
            get
            {
                if (row == 0 && column == 0)
                    return m00;
                if (row == 0 && column == 1)
                    return m01;
                if (row == 1 && column == 0)
                    return m10;
                if (row == 1 && column == 1)
                    return m11;
                throw new IndexOutOfRangeException("Invalid matrix2x2 index!");
            }
            set
            {
                if (row > 1 || column > 1)
                    throw new IndexOutOfRangeException("Invalid matrix2x2 index!");
                if (row == 0 && column == 0)
                    m00 = value;
                if (row == 0 && column == 1)
                    m01 = value;
                if (row == 1 && column == 0)
                    m10 = value;
                if (row == 1 && column == 1)
                    m11 = value;
            }
        }
        public static Matrix2x2 identity
        {
            get
            {
                return new Matrix2x2() { m00 = 1, m01 = 1, m10 = 1, m11 = 1 };
            }
        }
        public static Matrix2x2 zero
        {
            get
            {
                return new Matrix2x2() { m00 = 0, m01 = 0, m10 = 0, m11 = 0 };
            }
        }
        public static Vector2 operator *(Vector2 v, Matrix2x2 mat)
        {
            return new Vector2(v.x * mat[0, 0] + v.y * mat[1, 0], v.x * mat[0, 1] + v.y * mat[1, 1]);
        }
    }

    internal interface IJsonFaceData
    {
        Dictionary<string, Vector2> JsonData { get; }
        GenderType Gender { get; }
        int Age { get; }
        Vector3 Angle { get; }
        string Glass { get; }
        string[] JsonKeys { get; }
    }
    internal interface IFaceData
    {
        Dictionary<string, Transform> FaceBones { get; }
        Dictionary<string, BoneInfo> OrginBones { get; set; }
        Dictionary<string, BoneInfo> DataBones { get; set; }
        Texture2D DataImage { get; set; }
        Mesh FaceMesh { get; }
        void RecalculateBone(float t = 1);
        void RecalculateUV(int uv = 1);
        event Action<IFaceData> DataBoneChange;
        event Action<IFaceData> DataImageChange;
    }
    internal interface IFaceImageData
    {
        Dictionary<string, Vector2> DataImageData { get; }
        Texture2D DataImage { get; }
    }
}