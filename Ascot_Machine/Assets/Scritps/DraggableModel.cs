using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DraggableModel : MonoBehaviour
{
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private Material doneColor;
    [SerializeField] private MeshRenderer[] modelTransforms;

    private void Awake()
    {
        modelTransforms = GetComponentsInChildren<MeshRenderer>();
    }

    private void Start()
    {
        foreach (MeshRenderer mesh in modelTransforms)
        {
            if (mesh.transform.name.Contains("Imported1"))
            {
                mesh.transform.gameObject.SetActive(false);
                continue;
            }

            mesh.AddComponent<BoxCollider>();
            
            //Material[] matCollection = mesh.materials;

            mesh.material = transparentMaterial;

            //matCollection[0].DOColor(Color.grey, 0.25f);
            //matCollection[0].DOFade(0.5f, 0.25f);
        }
    }

    public void EnablePart(string name)
    {
        foreach (MeshRenderer mesh in modelTransforms)
        {
            if (mesh.gameObject.name == name)
            {
                print("Switch color for " + name);
                mesh.material = doneColor;
            }
        }
    }
}
