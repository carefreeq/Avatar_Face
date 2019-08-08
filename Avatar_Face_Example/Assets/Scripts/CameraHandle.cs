using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Camera))]
public class CameraHandle : MonoBehaviour
{
    public Transform target;
    public float distance = 0.6f;
    public float speed = 5f;
    private float y = 0f;
    void Start()
    {
        y = transform.eulerAngles.y;
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
#else
        if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
#endif
        {
            distance += Input.GetAxis("Mouse Y") * speed * 0.001f;
            distance = distance < 0f ? 0f : distance;
            y += Input.GetAxis("Mouse X") * speed * 0.1f;
            transform.rotation = Quaternion.Euler(0, y, 0);
            transform.position = transform.rotation * new Vector3(0f, transform.position.y, -distance) + target.position;
        }
    }
}
