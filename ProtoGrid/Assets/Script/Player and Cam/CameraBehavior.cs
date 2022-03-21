using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    #region variables    
    Camera camBehavior;
    Transform playerPos;
    [SerializeField] float camZoomSpeed;
    [SerializeField]float smoothTime;
    Vector3 velocity = Vector3.zero;
    Transform camTransform;
    bool flag = true;
    //[SerializeField] bool camMode;
    //[SerializeField] float camMoveSpeed;
    //[SerializeField] float camRotateSpeed;
    
    #endregion

    private void Awake()
    {
        //target = FindObjectOfType<Target>().transform;
        //camMode = true;
        camBehavior = transform.Find("Main Camera").GetComponent<Camera>();
        playerPos = FindObjectOfType<Player>().transform;
        camTransform = transform.Find("Main Camera");
       
    }

    private void Start()
    {
        transform.position = playerPos.position;
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            camBehavior.orthographicSize = 7;
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            camBehavior.orthographicSize = 3.8f;
    }

    private void Update()
    {
       
        RaycastHit[] hits = Physics.RaycastAll(camTransform.position, playerPos.position - camTransform.position, Vector3.Distance(camTransform.position, playerPos.position), LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
            if ( hits.Length >= 1)
        {
            foreach (RaycastHit h in hits)
            {
               h.collider.GetComponent<GridTiles>().hitByCam = true;
               h.collider.GetComponent<GridTiles>().numberFrameHit += 1;
                
            }
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(camTransform.position, playerPos.position - camTransform.position, Color.red);
    }

    private void LateUpdate()
    {
        // cam suit le joueur
        transform.position = Vector3.SmoothDamp(transform.position, playerPos.position, ref velocity, smoothTime);

        // cam dezoom et woom, ne marche pas encore en pinch
        //camBehavior.orthographicSize = Mathf.Lerp(camBehavior.orthographicSize,(Mathf.Clamp(camBehavior.orthographicSize - Mathf.Clamp(Input.mouseScrollDelta.y, -1, 1), 1f, 10)),Mathf.Clamp(camZoomSpeed/10,0,1));
        
        if(flag)
            StartCoroutine(PhoneOrientation());

    }

    IEnumerator PhoneOrientation()
    {
        flag = false;
        yield return new WaitForSeconds(.5f);
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            camBehavior.orthographicSize = 7;
        else if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            camBehavior.orthographicSize = 3.8f;
        flag = true;
    }

        #region oldCam
    /*private void Update()
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


        }
        else if (!camMode)
        {
            FreeHandleCamMode();

        }
        RotateCam();
        LockedCamMode();
        ZoomCam();
    }
    void LockedCamMode()
    {
        
        //transform.position = new Vector3(playerPos.position.x-5.5f,7.3f,playerPos.position.z-7.5f);
    }
    void ZoomCam()
    {
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
    }*/
        #endregion
}
