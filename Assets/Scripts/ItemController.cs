using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    public enum ItemType
    {
        burgers,
        vegetable,
        cookies,
        soap,
        drinks,
        pasta,
        batteries
    }

    public ItemType itemType;
    public float weight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
