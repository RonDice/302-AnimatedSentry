using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;

    public Vector3 targetOffset;

    public float mouseSensitivityX = 5;
    public float mouseSensitivityY = -5;
    public float mouseSensitivityScroll = 5;

    private Camera cam;

    private float pitch = 0;
    private float yaw = 0;
    private float dollyDis = 10;

  
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

   
    void Update()
    {

        
        // 1. Ease Position
        if (target)
        {
            transform.position = AnimMath.Ease(transform.position, target.position + targetOffset, 0.01f);
        }

        // 2. Set Rotation

        float mx = Input.GetAxis("MouseX");
        float my = Input.GetAxis("MouseY");


        yaw += mx * mouseSensitivityX;
        pitch += my * mouseSensitivityY;


        pitch = Mathf.Clamp(pitch, -89, 89);


        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        //3. Dolly Camera in and out

        dollyDis += Input.mouseScrollDelta.y * mouseSensitivityScroll;
        dollyDis = Mathf.Clamp(dollyDis, 3, 20);

        // Ease toward dolly position
        cam.transform.localPosition = AnimMath.Ease(
            cam.transform.localPosition,
            new Vector3(0, 0, -dollyDis),
            .02f);


    }

    private void OnDrawGizmos()
    {

        if(!cam) cam = GetComponentInChildren<Camera>();
        if (cam) return;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.DrawLine(transform.position, cam.transform.position);
    }
}
