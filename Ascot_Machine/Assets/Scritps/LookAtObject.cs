using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] private bool debug;
    [SerializeField] private float movementSpeed;

    private MeshRenderer _meshRender;
    private Camera _mainCamera;
    private CinemachineBrain _cinemachineBrain;
    private Vector3 _cachePosition;
    private Vector3 _cacheRotation;

    private void Awake()
    {
        _meshRender = GetComponent<MeshRenderer>();
        _cachePosition = this.transform.position;
        _cacheRotation = this.transform.rotation.eulerAngles;
        _mainCamera = Camera.main;
        _cinemachineBrain = _mainCamera.GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        if (debug)
            _meshRender.enabled = true;
        else
            _meshRender.enabled = false;

    }

    private void Update()
    {
        ActivateThisLookAt(true);
    }

    private void MoveCamera()
    {
        Vector3 inputDir = new Vector3(0f, 0f, 0f);

        if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.UpArrow))) inputDir.z = +0.5f;
        if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.DownArrow))) inputDir.z = -0.5f;
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow))) inputDir.x = -0.5f;
        if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow))) inputDir.x = +0.5f;
        if (Input.GetKey(KeyCode.Q)) inputDir.y = +0.5f;
        if (Input.GetKey(KeyCode.E)) inputDir.y = -0.5f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.up * inputDir.y + transform.right * inputDir.x;
        //float moveSpeed = 50f;
        transform.position += moveDir * movementSpeed * Time.deltaTime;
    }

    private void RotateCamera()
    {
        float yawnCamera = _mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, yawnCamera, transform.rotation.z), 5 * Time.deltaTime);
    }

    public void ActivateThisLookAt(bool status)
    {
        if (!_cinemachineBrain.IsBlending)
        {
            if (status)
            {
                MoveCamera();
                RotateCamera();
            }
        }
    }
}
