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
    public Transform target;
    bool flag;
    #endregion

    private void Awake()
    {
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
            if (flag)
            {
                flag = false;
                LockedCamMode();
            }
        }
        else if (!camMode)
        {
            FreeHandleCamMode();
            flag = true;
        }

        ZoomCam();
        RotateCam();
    }

    void LockedCamMode()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(playerPos.position.x - 3.5f, playerPos.position.y + 5f, playerPos.position.z - 4f), ref velocity, smoothTime);
        
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
            transform.RotateAround(target.position, target.up, Time.deltaTime * -camRotateSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(target.position, target.up, Time.deltaTime * camRotateSpeed);
        }      
    }
}
