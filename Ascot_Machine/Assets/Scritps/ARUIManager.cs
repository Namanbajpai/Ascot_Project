using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;

public class ARUIManager : MonoBehaviour
{
    [SerializeField]private HandleAR arManager;
    [SerializeField] private Button deassembleBtn;
    [SerializeField] private GameObject scanSurfacesPrefab;
    [SerializeField] private GameObject tapToPlacePrefab;

    private void OnEnable()
    {
        arManager.OnObjectPlaced += ObjectPlacedSuccessfully;
    }

    private void Start()
    {
        deassembleBtn.onClick.AddListener(() => 
        {
            arManager.ExplosionModel.DismantleMachine();
            deassembleBtn.gameObject.SetActive(false);
        });
    }

    private void OnDisable()
    {
        arManager.OnObjectPlaced -= ObjectPlacedSuccessfully;
    }

    private void ObjectPlacedSuccessfully()
    {
        deassembleBtn.gameObject.SetActive(true);
    }
}