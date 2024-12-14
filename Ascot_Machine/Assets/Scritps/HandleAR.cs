using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using  EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class HandleAR : MonoBehaviour
{
    public delegate void ObjectPlaced();
    public event ObjectPlaced OnObjectPlaced; 

    [SerializeField]
    private ModelExplosion go;

    [Header("UI Managers")]
    [SerializeField] private ARUIManager aRUIManager;

    private ARRaycastManager arRaycastManager;
    private ARPlaneManager arPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public ModelExplosion ExplosionModel
    {get{return go;}}

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(Finger finger)
    {
        // To avoid Multitouch
        if (finger.index != 0) return;

        if (this.arRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            ARRaycastHit firstHit = hits[0];
            Pose pose = firstHit.pose;

            if (go.gameObject.activeInHierarchy)
                return;

            //GameObject go = Instantiate(prefabToInstantiate, pose.position, pose.rotation);
            go.gameObject.SetActive(true);
            go.transform.position = pose.position;
            go.transform.rotation = pose.rotation;

            go.DoSomething(pose);

            arPlaneManager.enabled = false;
            foreach(ARPlane trackable in arPlaneManager.trackables)
            {
                trackable.enabled = false;
            }
            
            /*
            // Enable the machine at this position.
            if (arPlaneManager.GetPlane(firstHit.trackableId).alignment == PlaneAlignment.HorizontalUp)
            {
                // For handling the rotation of the machine.
                Vector3 position = go.transform.position;
                Vector3 cameraPosition = Camera.main.transform.position;
                Vector3 direction = cameraPosition - position;
                Vector3 targetRotationEuler = Quaternion.LookRotation(direction).eulerAngles;
                Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, go.transform.up.normalized);
                Quaternion targetRotation = Quaternion.Euler(scaledEuler);
                go.transform.rotation = go.transform.rotation * targetRotation;
            }
            */
            OnObjectPlaced?.Invoke();
        }
    }   
}
