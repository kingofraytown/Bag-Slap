using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Goal : MonoBehaviour
{

    public GameObject player;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            //activate the results screen
        }
    }
}
