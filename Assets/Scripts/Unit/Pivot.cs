using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Furniture furniture = transform.GetChild(0).GetComponent<Furniture>();
        transform.GetChild(0).position = new Vector3(transform.position.x + furniture.length / 2.0f - 0.5f, transform.position.y + furniture.height / 2.0f
                ,gameObject.transform.position.z + furniture.width / 2.0f - 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
