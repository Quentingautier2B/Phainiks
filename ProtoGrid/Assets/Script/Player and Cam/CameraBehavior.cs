using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    #region variables    
    Camera camBehavior;
    [SerializeField] bool camMode;
    Transform playerPos;
    [SerializeField] float camMoveSpeed;
    [SerializeField] float camZoomSpeed;
    [SerializeField] float camRotateSpeed;
    [SerializeField]float smoothTime;
    Vector3 velocity = Vector3.zero;
    Transform target;
    bool flag;
    #endregion

    private void Awake()
    {
        target = FindObjectOfType<Target>().transform;
        camMode = true;
        camBehavior = transform.Find("Main Camera").GetComponent<Camera>();
        playerPos = FindObjectOfType<Player>().transform;
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && camMode)
        {
                camMode = false;
        } 
        else if (Input.GetKeyDown(KeyCode.Space) && !camMode)
        {
                camMode = true;
        }
            
        if (camMode)
        {

            LockedCamMode();

        }
        else if (!camMode)
        {
            FreeHandleCamMode();
            
        }

        ZoomCam();
        RotateCam();
    }

    void LockedCamMode()
    {
        transform.position = Vector3.SmoothDamp(transform.position, playerPos.position, ref velocity, smoothTime);
        
        //transform.position = new Vector3(playerPos.position.x-5.5f,7.3f,playerPos.position.z-7.5f);
    }

    void FreeHandleCamMode()
    {
        var HcamMove =  Vector3.right * Input.GetAxisRaw("Horizontal") * camMoveSpeed * Time.deltaTime;
        var VcamMove =  Vector3.up * Input.GetAxisRaw("Vertical") * camMoveSpeed * Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") != 0)
           transform.Translate(HcamMove);

        if (Input.GetAxisRaw("Vertical") != 0)
            transform.Translate(VcamMove);
    }

    void ZoomCam()
    {
        camBehavior.orthographicSize = Mathf.Lerp(camBehavior.orthographicSize,(Mathf.Clamp(camBehavior.orthographicSize - Mathf.Clamp(Input.mouseScrollDelta.y, -1, 1), 1f, 10)),Mathf.Clamp(camZoomSpeed/10,0,1));
    }

    void RotateCam()
    {        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, Time.deltaTime * -camRotateSpeed, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, Time.deltaTime * camRotateSpeed,0,Space.World);
        }      
    }
}
