using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour
{
    public GameObject target;
    private Texture2D tex;
    private string log;
    private Avatar.AvatarObject avatar;
    private string avatarInfo;
    private int camId = 0;


    //换装模型SkinnedMesh
    public SkinnedMeshRenderer[] hairs;
    public SkinnedMeshRenderer[] upperCloth;
    public SkinnedMeshRenderer[] lowerCloth;
    private int hairIndex, upperClothIndex, lowerClothIndex;
    void Awake()
    {
        //注册face++ key
        Avatar.AvatarTools.API_Key = "Ga75S46sZa0SQaZ8c38DcMsETf78jLFs";
        Avatar.AvatarTools.API_Secret = "y87yThUmXVAvRotv0wdCXWKh9dMCeJ2i";
    }
    void Start()
    {
        //为特定模型添加AvatarObject
        avatar = target.AddComponent<Avatar.AvatarObject>();
        //设置脸变化时间
        avatar.ChangeTime = 2f;
        //注册脸部加载结果事件
        avatar.LoadResultEvent += (done) => avatarInfo = "加载" + (done ? "成功" : "失败");
        //打开AvatarLog日志
        Avatar.AvatarDebug.Enable = true;
        //删除AvatarLog旧日志
        Avatar.AvatarDebug.Delete();
        //注册AvatarLog事件
        Avatar.AvatarDebug.LogEvent += (log, stack, type) => this.log = log;
        //初始化
        tex = new Texture2D(0, 0);
    }
    void OnGUI()
    {
        //调用Avatar摄像机
        Avatar.CameraTexture.OnGUI();

        //绘制按钮和Log
        GUI.skin.button.fontSize = 60;
        GUI.skin.label.fontSize = 20;
        GUI.Label(new Rect(20, Screen.height / 2f + 600, Screen.width - 40, 200), log);
        GUI.Label(new Rect(20, Screen.height / 2f + 800, Screen.width - 40, 200), avatarInfo);

        if (tex != null)
        {
            if (GUI.Button(new Rect(Screen.width / 2f - 200, Screen.height / 2f + 800, 400, 200), "打开"))
            {
                tex = null;
                //打开摄像机
                Avatar.CameraTexture.Open(camId);
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width - 300, 0, 300, 150), "切换"))
            {
                //更改摄像机设备ID
                camId = (camId + 1) % 2;
                Avatar.CameraTexture.Close();
                Avatar.CameraTexture.Open(camId);
            }
            if (GUI.Button(new Rect(Screen.width / 2f - 200, Screen.height / 2f + 800, 400, 200), "拍摄"))
            {
                //获取摄像机图片
                tex = Avatar.CameraTexture.GetTexture(0.4f);
                //关闭摄像机
                Avatar.CameraTexture.Close();
                //avatar读取图片
                avatar.LoadFaceTex(tex);
            }
        }

        //绘制截图的图片
        if (tex != null)
            GUI.DrawTexture(new Rect(0, 0, tex.width, tex.height), tex);


        //换装
        if (tex != null)
        {
            if (GUI.Button(new Rect(10, 10, 200, 150), "头发"))
            {
                hairIndex = (hairIndex + 1) % hairs.Length;
                avatar.LoadSkinnedMesh(avatar.Hair, hairs[hairIndex]);
            }
            if (GUI.Button(new Rect(10, 170, 200, 150), "上衣"))
            {
                upperClothIndex = (upperClothIndex + 1) % upperCloth.Length;
                avatar.LoadSkinnedMesh(avatar.UpperCloth, upperCloth[upperClothIndex]);
            }
            if (GUI.Button(new Rect(10, 330, 200, 150), "裤子"))
            {
                lowerClothIndex = (lowerClothIndex + 1) % lowerCloth.Length;
                avatar.LoadSkinnedMesh(avatar.LowerCloth, lowerCloth[lowerClothIndex]);
            }
        }

        //读取存储
        if (tex != null)
        {
            if (GUI.Button(new Rect(Screen.width - 210, 10, 200, 150), "保存"))
            {
                //保存Avatar数据到本地
                Avatar.AvatarTools.SaveAvatarData(avatar.GetAvatarData());
            }
            if (GUI.Button(new Rect(Screen.width - 210, 170, 200, 150), "读取0"))
            {
                //读取本机所有Avatar数据
                Avatar.AvatarData[] datas = Avatar.AvatarTools.ReadAvtarDatas();
                //加载0号数据
                if (datas.Length > 0)
                    avatar.SetAvatarData(datas[0]);
            }
            if (GUI.Button(new Rect(Screen.width - 210, 330, 200, 150), "删除0"))
            {
                //读取本机所有Avatar数据
                Avatar.AvatarData[] datas = Avatar.AvatarTools.ReadAvtarDatas();
                //删除0号数据
                if (datas.Length > 0)
                    Avatar.AvatarTools.DeleteAvatarDatas(datas[0]);
            }
        }
    }
    public void DeleteData()
    {
        //读取本机所有Avatar数据
        Avatar.AvatarData[] datas = Avatar.AvatarTools.ReadAvtarDatas();
        //删除0号数据
        if (datas.Length > 0)
            Avatar.AvatarTools.DeleteAvatarDatas(datas[0]);
    }
}
