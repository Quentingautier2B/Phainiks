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
    float zoomLerpValue;
    bool zoomBool;
    PostProcessVolume m_Volume;
    DepthOfField m_DOF;
    static float sliderValue;

    [Header("Cheat")]
    [SerializeField] bool cheatAllow;

    public bool CaTourne = false;
    public bool LeftLerp, RightLerp;
    public bool CaZoom = false;
    public bool ZoomLerp, DezoomLerp;
    public float SpeedOfLerp = 1;

    public Slider ZoomSlider;


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
        zoomLerpValue += Time.deltaTime;
        camTransform.localPosition = new Vector3(camTransform.localPosition.x, camTransform.localPosition.y, Mathf.Lerp(startPos, (zoomSlider.value * -45) - 15, zoomLerpValue));
        
        m_DOF.focusDistance.value = Mathf.Lerp(DOFstart, (zoomSlider.value * 10) + 10, zoomLerpValue);
        if (zoomLerpValue >= 1)
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
        if (!CaTourne)
        {
            AngleCheck();

            if (lerp)
            {
                Lerp(angleLerp);
            }
            if (cheatAllow)
            {

                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    OnLeftButtonClick();
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    OnRightButtonClick();
                }     
            }

            //if (cheatAllow)
            //{
            //    if (!lerp)
            //    {
            //        var angle = transform.eulerAngles.y;
            //        var Rotate = Mathf.RoundToInt(((angle - 45) % 360) / 90);
            //        transform.eulerAngles.y

            //    }
            //}
            
        }

        if (cheatAllow)
        {

            if (!CaZoom)
            { 
                if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    if (CaTourne && LeftLerp == true)
                    {
                        CaTourne = false;
                        LeftLerp = false;
                    }
                    else
                    {
                        RightLerp = false;
                        LeftLerp = true;
                        CaTourne = true;
                    }
                
                }
                else if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    if (CaTourne && RightLerp == true)
                    {
                        CaTourne = false;
                        RightLerp = false;
                    }
                    else
                    {
                        LeftLerp = false;
                        RightLerp = true;
                        CaTourne = true;
                    }
                }
            }

            if (CaTourne || CaZoom)
            {
                if(Input.GetKeyDown(KeyCode.KeypadPlus))
                {
                    SpeedOfLerp += 0.5f;
                }
                else if (Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    if (SpeedOfLerp > 0.5)
                    {
                        SpeedOfLerp -= 0.5f;
                    }
                }
            }

            if(!CaTourne)
            {
                if(Input.GetKeyDown(KeyCode.Keypad8))
                {
                    if (CaZoom && ZoomLerp == true)
                    {
                        CaTourne = false;
                        ZoomLerp = false;
                    }
                    else
                    {
                        DezoomLerp = false;
                        ZoomLerp = true;
                        CaZoom = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    if (CaZoom && DezoomLerp == true)
                    {
                        CaTourne = false;
                        DezoomLerp = false;
                    }
                    else
                    {
                        ZoomLerp = false;
                        DezoomLerp = true;
                        CaZoom = true;
                    }
                }
            }
            
            if (LeftLerp)
            {
                CamLerp(1);
            }
            else if (RightLerp)
            {
                CamLerp(-1);
            }
            else if (ZoomLerp) 
            {
                zoomSlider.value += 0.1f * SpeedOfLerp * Time.deltaTime;
                if (ZoomSlider.value == 1) ZoomLerp = false; CaZoom = false;
            }
            else if (DezoomLerp)
            {
                zoomSlider.value -= 0.1f * SpeedOfLerp * Time.deltaTime;
                if (ZoomSlider.value == 0) DezoomLerp = false; CaZoom = false;
            }

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

    void CamLerp(float X)
    {

        target = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + X, transform.localEulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * SpeedOfLerp);

    }

}
