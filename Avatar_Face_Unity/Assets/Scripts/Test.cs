using UnityEngine;
using System.Collections;
using Avatar;
using System.IO;
public class Test : MonoBehaviour
{
    private AvatarObject obj;
    void Start()
    {
        AvatarTools.API_Key = "AfmJWHbD9khpycyklMR5GSR1j4CSg5Yt";
        AvatarTools.API_Secret = "3Vng4tiwU6zP7Zqetjq0DRdTISQ4CBAO";
        obj = FindObjectOfType<AvatarObject>();
        obj.LoadResultEvent += (done) => Debug.Log(done ? "加载完成" : "加载失败");
        AvatarDebug.Enable = true;
        AvatarDebug.LogEvent += (log, stack, type) => Debug.Log(log);
    }
    void OnGUI()
    {
        GUI.skin.button.fontSize = 60;
        if (GUI.Button(new Rect(Screen.width / 2f - 200, Screen.height / 2f + 800, 400, 200), "加载"))
        {
            if (obj.tex != null)
            {
                obj.LoadFaceTex(obj.tex);
            }
        }
    }
}
