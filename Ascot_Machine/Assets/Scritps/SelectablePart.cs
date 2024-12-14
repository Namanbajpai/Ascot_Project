using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;

public class SelectablePart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 mousePos;
    private Vector3 _originalPos;
    private Vector3 m_originalScale;
    private Vector3 m_targetScale;
    private MeshRenderer m_meshRenderer;
    private Material[] m_material;
    private BoxCollider m_collider;
    private Color m_caheColor;

    private ModelExplosion m_explosion;
    private DraggableModel m_draggableModel;
     

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void Awake()
    {
        m_collider = GetComponent<BoxCollider>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_material = m_meshRenderer.materials;
        m_caheColor = m_material[0].color;
        m_originalScale = transform.localScale;
        m_targetScale = new Vector3(m_originalScale.x + 0.25f, m_originalScale.y + 0.25f, m_originalScale.z + 0.25f);    
    }
    void Start()
    {
        // Find the Canvas child object
        Transform canvasTransform = transform.Find("Canvas");
       
        // Check if the Canvas exists
        if (canvasTransform != null)
        {
            // Activate the Canvas
            canvasTransform.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Canvas not found as a child of " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        this.transform.AddComponent<BoxCollider>();
    }

    public void Initalize(ModelExplosion modelExplosion, DraggableModel drag)
    {
        m_explosion = modelExplosion;
        m_draggableModel = drag;
    }

    public void SavePosition()
    {
        print("Saving position");
        _originalPos = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        /* Transform detailedCanvas=transform.Find("CanvasDetailed");
           if (detailedCanvas != null)
        {
            // Activate the Canvas
            detailedCanvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Canvas not found as a child of " + gameObject.name);
        }*/
        //transform.DOScale(m_targetScale, 1f);
        m_material[0].DOColor(Color.green, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
         //Transform detailedCanvas=transform.Find("CanvasDetailed");
        // detailedCanvas.gameObject.SetActive(false);
        //transform.DOScale(m_originalScale, 1f);
        m_material[0].DOColor(m_caheColor, 1f);
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mousePos = Input.mousePosition - GetMousePos();
        m_explosion.SelectedPart = this;
        m_explosion.DisableOtherColliders(m_meshRenderer);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_explosion.SelectedPart = null;

        if (m_collider != null) 
            m_collider.enabled = false;


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.transform.name);

            if (hit.transform.gameObject.GetComponentInParent<DraggableModel>() != null)
            {
                m_draggableModel.EnablePart(this.gameObject.name);
                this.gameObject.SetActive(false);
                //this.transform.position = _originalPos;

                print($"If condition");
            }
            else
            {
                this.transform.position = _originalPos;
                print("Else condition");
            }

            Debug.Log($"hit {hit.transform.name}");
        }

        m_explosion.EnableAllColliders();
        //this.gameObject.layer = 0;
    }
}
