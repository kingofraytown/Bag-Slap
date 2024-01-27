using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum PlayerStates
    {
        Idle,
        Walking,
        Slapping,
        Hurt,
        Dead
    }

    public PlayerStates currentState = PlayerStates.Idle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
