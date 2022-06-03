using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
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
    PostProcessVolume m_Volume;
    DepthOfField m_DOF;
    static float sliderValue;


    private void Awake()
    {
        m_Volume = Camera.main.GetComponent<PostProcessVolume>();
        m_Volume.profile.TryGetSettings<DepthOfField>(out m_DOF);
        camBehavior = transform.Find("Main Camera").GetComponent<Camera>();
        playerPos = FindObjectOfType<Player>().transform;
        camTransform = transform.Find("Main Camera");
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
        
        flagLerp = true;
        zoomBool = false;
    }

    private void Start()
    {
/*        if (!FindObjectOfType<SceneChange>().Hub)
        {*/
            transform.position = playerPos.position;
            zoomSlider.value = sliderValue;
            m_DOF.focusDistance.value = (sliderValue * 10) + 10;
            camTransform.localPosition = new Vector3(camTransform.localPosition.x, camTransform.localPosition.y, (sliderValue * -45) - 15);     
    }


    public void onZoomValueChanged()
    {
        sliderValue = zoomSlider.value;
        if (!zoomBool && Time.timeSinceLevelLoad > 2)
        {
            zoomBool = true;
            StartCoroutine(valueChanged(camTransform.localPosition.z, m_DOF.focusDistance.value));
        }
        else
        {
            //zoomBool = false;
            //zoomLerp = 0;
            StopCoroutine(valueChanged(camTransform.localPosition.z, m_DOF.focusDistance.value));
            StartCoroutine(valueChanged(camTransform.localPosition.z, m_DOF.focusDistance.value));
            //m_DOF.focusDistance.value = (sliderValue * 10) + 10;
            //camTransform.localPosition = new Vector3(camTransform.localPosition.x, camTransform.localPosition.y, (sliderValue * -45) - 15);
        }
    }



    IEnumerator valueChanged(float startPos, float DOFstart)
    {
        zoomLerp += Time.deltaTime;
        camTransform.localPosition = new Vector3(camTransform.localPosition.x, camTransform.localPosition.y, Mathf.Lerp(startPos, (zoomSlider.value * -45) - 15, zoomLerp));
        
        m_DOF.focusDistance.value = Mathf.Lerp(DOFstart, (zoomSlider.value * 10) + 10, zoomLerp);
        if (zoomLerp >= 1)
        {
            //zoomLerp = 0;
            zoomBool = false;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(valueChanged(startPos, DOFstart));
        }
    }


    private void Update()
    {
        AngleCheck();
/*        RaycastHit[] hits = Physics.SphereCastAll(camTransform.position, .2f, playerPos.position - camTransform.position, Vector3.Distance(camTransform.position, playerPos.position), LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
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
        }*/

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

    private void LateUpdate()
    {
        // cam suit le joueur
        if(Time.timeSinceLevelLoad > .5)
            transform.position = Vector3.SmoothDamp(transform.position, playerPos.position, ref velocity, smoothTime);
        else
        {
            transform.position = playerPos.position;
        }

        
        
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

}
