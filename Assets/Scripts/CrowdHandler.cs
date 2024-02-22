using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowdHandler : MonoBehaviour
{
    [Header("Crowd Handler")]
    [Space]
    [Header("Configuration")]
    [SerializeField] List<Material> materials = new List<Material>();
    [SerializeField] List<GameObject> crowds = new List<GameObject>();

    void Start()
    {
        HandleCrowdColorization();
    }

    void HandleCrowdColorization()
    {
        for (int i = 0; i < crowds.Count; i++)
        {
            int crowdCount = crowds[i].transform.childCount;
            for (int y = 0; y < crowdCount; y++)
            {
                GameObject child = crowds[i].transform.GetChild(y).gameObject;

                int randomMaterialIndex = Random.Range(0, materials.Count);
                child.GetComponent<MeshRenderer>().material = materials[randomMaterialIndex];

                int grandChildrenCount = child.transform.childCount;
                for (int x = 0; x < grandChildrenCount; x++)
                {
                    GameObject grandChild = child.transform.GetChild(x).gameObject;
                    MeshRenderer grandChildRenderer = grandChild.GetComponent<MeshRenderer>();
                    if (grandChildRenderer.materials.Length > 1)
                    {
                        Material[] materialsSet = { materials[randomMaterialIndex], materials[randomMaterialIndex], materials[randomMaterialIndex] };
                        grandChildRenderer.materials = materialsSet;
                    }
                    else
                    {
                        grandChild.GetComponent<MeshRenderer>().material = materials[randomMaterialIndex];
                    }
                }
            }
        }
    }
}
