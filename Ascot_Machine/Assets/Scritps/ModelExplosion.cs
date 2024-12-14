using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModelExplosion : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] modelTransforms;
    [SerializeField] private Material modelMat;
    [SerializeField] private float explosionValue;
    [SerializeField] private float Timer;
    [SerializeField] private DraggableModel dragableModel;
    [SerializeField] private Button dismantle;
    private Animator m_animationHandler;

    private SelectablePart selectedPart;

    public SelectablePart SelectedPart
    { get { return selectedPart; }  set { selectedPart = value; } }

    private void Awake()
    {
        modelTransforms = GetComponentsInChildren<MeshRenderer>();  
        m_animationHandler = GetComponent<Animator>();
    }

    public void DoSomething(Pose pose)
    {
        dragableModel.transform.position = pose.position;
        dragableModel.transform.rotation = pose.rotation;
    }

    private void Start()
    {
        dismantle.onClick.AddListener(delegate ()
        {
            DismantleMachine();
            dismantle.gameObject.SetActive(false);
        });
        /*
        DismantleMachine();
        */
    }

    public void DismantleMachine()
    {
        foreach (MeshRenderer mesh in modelTransforms)
        {
            if (mesh.transform.name.Contains("Imported1"))
            {
                mesh.transform.gameObject.SetActive(false);
                continue;
            }

            mesh.material = modelMat;
            SelectablePart selectable = mesh.transform.gameObject.AddComponent<SelectablePart>();
            selectable.Initalize(this, dragableModel);

            Rigidbody rb = mesh.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;

            StartCoroutine(VelocityTimer(selectable, rb));
            //rb.velocity = new Vector3(pos.x, pos.y, pos.z + 2f);
        }
        //StartCoroutine(EnableAnimationForAR());
    }

    protected IEnumerator EnableAnimationForAR()
    {
        m_animationHandler.SetBool("Enable", true);
        yield return new WaitForSeconds(2);
        dragableModel.gameObject.SetActive(true);
        m_animationHandler.enabled= false;
    }

    public Vector3 FindCenterOfTransforms(MeshRenderer[] transforms)
{
    var bound = new Bounds(transforms[0].transform.position, Vector3.zero);
    for(int i = 1; i < transforms.Length; i++)
    {
        bound.Encapsulate(transforms[i].transform.position);
    }
    print(bound.center);
    return bound.center;
}

    private void Update()
    {
        
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Input.mousePosition, Vector3.forward, Color.red);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(Input.mousePosition, Vector3.forward, 100.0F);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Renderer rend = hit.transform.GetComponent<Renderer>();

            if (rend)
            {
                // Change the material of all hit colliders
                // to use a transparent shader.
                rend.material.shader = Shader.Find("Transparent/Diffuse");
                Color tempColor = rend.material.color;
                tempColor.a = 0.3F;
                rend.material.color = tempColor;
            }
        }
        
    }

    private IEnumerator VelocityTimer(SelectablePart selectable, Rigidbody rb)
    {
        Vector3 pos = rb.transform.position;
        rb.velocity = new Vector3(pos.x, pos.y, pos.z + explosionValue);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(Timer);
        rb.velocity = Vector3.zero;
        selectable.SavePosition();
        dragableModel.gameObject.SetActive(true);
    }

    public void DisableOtherColliders(MeshRenderer currentlySelected)
    {
        foreach (MeshRenderer mesh in modelTransforms)
        {
            BoxCollider currentCollider = mesh.GetComponent<BoxCollider>();
            if (currentCollider != null)
                mesh.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void EnableAllColliders()
    {
        print("Enabling all the colliders");    
        foreach (MeshRenderer mesh in modelTransforms)
        {
            BoxCollider currentCollider = mesh.GetComponent<BoxCollider>();
            if (currentCollider != null)
                mesh.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
