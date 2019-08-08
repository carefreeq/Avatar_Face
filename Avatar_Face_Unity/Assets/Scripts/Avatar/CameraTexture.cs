using UnityEngine;
namespace Avatar
{
    /// <summary>
    /// 摄像机控制类
    /// </summary>
    public static class CameraTexture
    {
        private static WebCamTexture camTex;
        private static ScreenOrientation orientation;
        internal static int CamAngle { set { if (value > 180) { value = value % 360 - 360; } else { while (value < -180) value = value + 360; } camAngle = value; } get { return camAngle; } }
        private static int camAngle = 0;
        /// <summary>
        /// 打开摄像机拍摄功能
        /// </summary>
        /// <param name="id">设备ID</param>
        public static void Open(int id = 0)
        {
            AvatarDebug.Log("camTex opening ---");
            if (id < WebCamTexture.devices.Length)
            {
                Application.RequestUserAuthorization(UserAuthorization.WebCam);
                if (Application.HasUserAuthorization(UserAuthorization.WebCam))
                {
                    camTex = new WebCamTexture(WebCamTexture.devices[id].name, Screen.height, Screen.width, 24);
                    orientation = Screen.orientation;
                    Screen.orientation = ScreenOrientation.Portrait;
                    camTex.Play();
                    CamAngle = camTex.videoRotationAngle;
                    AvatarDebug.Log("camTex open done!");
                    return;
                }
            }
            AvatarDebug.Log("camTex open fail!", LogType.Error);
        }
        /// <summary>
        /// 将摄像机图像渲染到屏幕，请在OnGUI函数中调用
        /// </summary>
        public static void OnGUI()
        {
            if (camTex != null)
            {
                float half_x = Screen.width / 2f;
                float half_y = Screen.height / 2f;
                GUIUtility.RotateAroundPivot(CamAngle, new Vector2(half_x, half_y));
                GUIUtility.ScaleAroundPivot(new Vector2(Mathf.Sign(CamAngle), 1), new Vector2(half_x, half_y));
                GUI.DrawTexture(new Rect(half_x - half_y, half_y - half_x, Screen.height, Screen.width), camTex);
                GUIUtility.ScaleAroundPivot(new Vector2(Mathf.Sign(CamAngle), 1), new Vector2(half_x, half_y));
                GUIUtility.RotateAroundPivot(-CamAngle, new Vector2(half_x, half_y));
            }
        }
        /// <summary>
        /// 关闭摄像机拍摄
        /// </summary>
        public static void Close()
        {
            if (camTex != null)
            {
                camTex.Stop();
                Screen.orientation = orientation;
                camTex = null;
                AvatarDebug.Log("camTex close done!");
            }
            else
            {
                AvatarDebug.Log("close camTex? camTex is null!", LogType.Warning);
            }
        }
        /// <summary>
        /// 截取当前摄像机在屏幕中的信息
        /// </summary>
        /// <param name="scale">比例值</param>
        /// <returns>摄像机图片</returns>
        public static Texture2D GetTexture(float scale = 1f)
        {
            if (camTex != null)
            {
                AvatarDebug.Log("camTex getting ---");
                Texture2D tex = new Texture2D(Screen.width, Screen.height);
                tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                tex.Apply();
                int width = (int)(Screen.width * scale);
                int height = (int)(Screen.height * scale);
                Texture2D _tex = new Texture2D(width, height);
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        _tex.SetPixel(x, y, tex.GetPixelBilinear((float)x / width, (float)y / height));
                _tex.Apply();
                AvatarDebug.Log("camTex get done!");
                return _tex;
            }
            else
            {
                AvatarDebug.Log("get camTex? camTex is null!", LogType.Warning);
                return null;
            }
        }
    }
}