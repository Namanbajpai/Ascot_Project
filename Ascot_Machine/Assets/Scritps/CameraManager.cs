using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private ModelExplosion explosionHandler;

    public static int MAX_CINE_PRIORITY;


    private const int DEFAULT_CAMERA_INDEX = 0;
    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";
    private const float POINTER_Z = 0.3f;

    [SerializeField] private CinemachineBrain cineBrain; // Used only for bledning.
    [SerializeField] private CinemachineFreeLook activeFreeLook;

    private int UILayer;
    private CinemachineFreeLookZoom _zoomHandler;
    private float _scaleValue;

    private void Awake()
    {
        _zoomHandler = GetComponent<CinemachineFreeLookZoom>();
    }

    void Start()
    {

        UILayer = LayerMask.NameToLayer("UI");
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        //if(IsPointerOverUIElement()) return;
        
        if (activeFreeLook != null)
        {
            CameraFreeLookHandle(activeFreeLook);
            //ScaleValue();
        }
        //ScaleTagOnHover();

    }

    private void ScaleValue()
    {
        Vector3 distance = activeFreeLook.transform.position - activeFreeLook.m_Follow.transform.position;
        _scaleValue = (distance.magnitude) / 20f;
    }

    private void CameraFreeLookHandle(CinemachineFreeLook currentFreeLook)
    {
        if (Input.GetMouseButton(0) && !cineBrain.IsBlending && explosionHandler.SelectedPart == null)
        {
            // Activate cinemachine camera 
            currentFreeLook.m_YAxis.m_InputAxisName = MOUSE_Y;
            currentFreeLook.m_XAxis.m_InputAxisName = MOUSE_X;
        }
        else
        {
            currentFreeLook.m_YAxis.m_InputAxisName = "";
            currentFreeLook.m_XAxis.m_InputAxisName = "";

            currentFreeLook.m_XAxis.m_InputAxisValue = 0;
            currentFreeLook.m_YAxis.m_InputAxisValue = 0;
        }
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
 
 
    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }
 
 
    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
