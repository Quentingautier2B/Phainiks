using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [HideInInspector] public int rotateMode;
    bool lerp;
    int angleLerp;
    bool flagLerp;
    float interpolateAmount;
    Quaternion target;
    //[SerializeField] bool camMode;
    //[SerializeField] float camMoveSpeed;  
    //[SerializeField] float camRotateSpeed;

    #endregion
    public Slider zoomSlider;
    float zoomLerp;
    bool zoomBool;
    private void Awake()
    {
        //target = FindObjectOfType<Target>().transform;
        //camMode = true;
        camBehavior = transform.Find("Main Camera").GetComponent<Camera>();
        playerPos = FindObjectOfType<Player>().transform;
        camTransform = transform.Find("Main Camera");
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
        flagLerp = true;
        zoomBool = false;
    }

    private void Start()
    {
        transform.position = playerPos.position;
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            camBehavior.orthographicSize = 7;
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            camBehavior.orthographicSize = 3.8f;
    }


    public void onZoomValueChanged()
    {
        if (!zoomBool)
        {
            zoomBool = true;
            StartCoroutine(valueChanged(camTransform.localPosition.z, zoomSlider.value));
        }
    }



    IEnumerator valueChanged(float startPos, float endPos)
    {
        zoomLerp += Time.deltaTime;
        camTransform.localPosition = new Vector3(camTransform.localPosition.x, camTransform.localPosition.y, Mathf.Lerp(startPos, zoomSlider.value, zoomLerp));

        if(zoomLerp >= 1)
        {
            zoomLerp = 0;
            zoomBool = false;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(valueChanged(startPos, endPos));
        }
    }


    private void Update()
    {/*
        if (Input.GetKey(KeyCode.P))
        {

                var c = camTransform.localPosition;
                c.z = -60;
                camTransform.localPosition = Vector3.SmoothDamp(camTransform.localPosition, c, ref velocity, smoothTime);
            
        }
        else if (Input.GetKey(KeyCode.O) )
        {
            var c = camTransform.localPosition;
            c.z = -15;
            camTransform.localPosition = Vector3.SmoothDamp(camTransform.localPosition, c, ref velocity, smoothTime);
        }
*/
        ForPicture();
        AngleCheck();
        RaycastHit[] hits = Physics.SphereCastAll(camTransform.position, .2f, playerPos.position - camTransform.position, Vector3.Distance(camTransform.position, playerPos.position), LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
        if ( hits.Length >= 1)
        {
            foreach (RaycastHit h in hits)
            {
                if (h.collider.gameObject.GetComponent<GridTiles>() != null && h.collider.gameObject.GetComponent<GridTiles>().open)
                {
                        h.collider.GetComponent<GridTiles>().hitByCam = true;
                        h.collider.GetComponent<GridTiles>().numberFrameHit += 1;

                }

            }
        }

        if (lerp == true)
        {
            Lerp(angleLerp);
        }

    }

    public void OnLeftButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Menuing/RotateCam");
        angleLerp = 90;
        lerp = true;
    }

    public void OnRightButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Menuing/RotateCam");
        angleLerp = -90;
        lerp = true;
    }

    /*private void OnDrawGizmos()
    {
        Debug.DrawRay(camTransform.position, playerPos.position - camTransform.position, Color.red);
    }
*/
    private void LateUpdate()
    {
        // cam suit le joueur
        if(Time.timeSinceLevelLoad > .5)
            transform.position = Vector3.SmoothDamp(transform.position, playerPos.position, ref velocity, smoothTime);
        else
        {
            transform.position = playerPos.position;
        }

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

/*    private void OnGUI()
    {

        if (GUI.Button(new Rect(110, 440, 150, 150), "left"))
        {
            //transform.Rotate(0, 90, 0, Space.World);
            angleLerp = 90;
            lerp = true;
            
        }

        if (GUI.Button(new Rect(1650, 440, 150, 150), "right"))
        {
            //transform.Rotate(0, -90, 0, Space.World);
            angleLerp = -90;
            lerp = true;
        }
    }*/

    void Lerp(int angle)
    {

        if (flagLerp)
        {
            target = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + angle, transform.localEulerAngles.z);
            flagLerp = false;
        }
        interpolateAmount += Time.deltaTime*2;
        transform.rotation = Quaternion.Lerp(transform.rotation, target, interpolateAmount);
        if (interpolateAmount > 0.95)
        {
            transform.rotation = target;
            lerp = false; 
            flagLerp = true;
            interpolateAmount = 0;
        }
    }
    void AngleCheck()
    {
        rotateMode = Mathf.RoundToInt(((transform.localEulerAngles.y - 45) % 360) / 90);
    }

    void ForPicture()
    {
        //if ()
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
