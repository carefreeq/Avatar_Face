using UnityEngine;
using System.Collections;
namespace Avatar
{
    /// <summary>
    /// 表情对象，待完成
    /// </summary>
    public class FaceBlend : MonoBehaviour
    {
        public SkinnedMeshRenderer face;
        public SkinnedMeshRenderer lash;
        public float Blink
        {
            get { return blink; }
            set
            {
                if (face)
                    face.SetBlendShapeWeight(0, value);
                if (lash)
                    lash.SetBlendShapeWeight(0, value);
                blink = value;
            }
        }
        private float blink = 0;

        public float Smile
        {
            get { return smile; }
            set
            {
                if (face)
                    face.SetBlendShapeWeight(1, value);
                smile = value;
            }
        }
        private float smile = 0;
        public float Talk
        {
            get { return talk; }
            set
            {
                if (face)
                    face.SetBlendShapeWeight(2, value);
                talk = value;
            }
        }
        private float talk = 0;
        void Start()
        {
            //StartCoroutine(UpdateBlink(0.1f, 1f));
        }
        IEnumerator UpdateBlink(float time, float span)
        {
            while (true)
            {
                float _t = 0;
                while ((_t += Time.deltaTime) < time)
                {
                    Blink = _t / time * 100f;
                    yield return new WaitForEndOfFrame();
                }
                Blink = 100f;
                while ((_t -= Time.deltaTime) > 0)
                {
                    Blink = _t / time * 100f;
                    yield return new WaitForEndOfFrame();
                }
                Blink = 0f;
                yield return new WaitForSeconds(span);
            }
        }
    }
}