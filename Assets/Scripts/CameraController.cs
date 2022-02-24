using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public PlayerTargeting player;

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

        if(player == null)
        {
            PlayerTargeting script = FindObjectOfType<PlayerTargeting>();


        }
    }

   
    void Update()
    {


        bool isAiming =  (player && player.target && player.playerWantsToAim) ;
        

        
        // 1. Ease Position
        if (player)
        {
            transform.position = AnimMath.Ease(transform.position, player.transform.position + targetOffset, 0.01f);
        }

        // 2. Set Rig Rotation
        if (isAiming)
        {
            

            Quaternion temp = Quaternion.Euler(pitch, yaw, 0);

            transform.rotation = AnimMath.Ease(transform.rotation, yaw, 0 , .001f);
        }
        else
        {

            float mx = Input.GetAxis("MouseX");
            float my = Input.GetAxis("MouseY");


            yaw += mx * mouseSensitivityX;
            pitch += my * mouseSensitivityY;

            

            pitch = Mathf.Clamp(pitch, -10, 89);
            transform.rotation = AnimMath.Ease(transform.rotation, Quaternion.Euler(pitch, yaw, 0), .001f);

        }

        

        //3. Dolly Camera in and out



        
        
           dollyDis += Input.mouseScrollDelta.y * mouseSensitivityScroll;
            dollyDis = Mathf.Clamp(dollyDis, 3, 20);


        // Ease toward dolly position
        float tempZ = isAiming ? 2 : dollyDis;
        cam.transform.localPosition = AnimMath.Ease(
            cam.transform.localPosition,
            new Vector3(0, 0, -tempZ),
            .02f);

        //4. Rotate the camera object

        if(isAiming)
        {
            Vector3 vToAimTarget = player.target.transform.position - cam.transform.position;
            cam.transform.rotation = AnimMath.Ease(cam.transform.rotation, Quaternion.LookRotation(vToAimTarget), .001f);
        }
        else
        {
            cam.transform.localRotation = AnimMath.Ease(cam.transform.localRotation, Quaternion.identity, .001f);
        }


    }

    private void OnDrawGizmos()
    {

        if(!cam) cam = GetComponentInChildren<Camera>();
        if (cam) return;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.DrawLine(transform.position, cam.transform.position);
    }
}
