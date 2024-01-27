using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public float speed;
    [SerializeField] Camera camera;
    [SerializeField] float slapTime;
    float slapTimer;
    public enum PlayerStates
    {
        Idle,
        Walking,
        Slapping,
        Hurt,
        Dead
    }

    public PlayerStates currentState = PlayerStates.Idle;
    Vector2 _movement;
    Vector2 _mousePos;
    Animator animator;

    Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == PlayerStates.Slapping)
        {
            slapTimer -= Time.deltaTime;

            if(slapTimer <= 0f)
            {
                currentState = PlayerStates.Idle;
            }
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.AddForce(_movement * speed);
        Vector2 faceingDir = _mousePos - _rigidbody2D.position;
        //float angle = Mathf.Atan2(faceingDir.y, faceingDir.x) * Mathf.Rad2Deg;
        //_rigidbody2D.MoveRotation(angle);


    }

    public void OnMove(InputAction.CallbackContext cc)
    {
        _movement = cc.ReadValue<Vector2>();
    }

    public void OnMousePos(InputAction.CallbackContext cc)
    {
        _mousePos = camera.ScreenToWorldPoint(cc.ReadValue<Vector2>());
        transform.up = _mousePos - new Vector2(transform.position.x, transform.position.y);
    }

    public void OnSlap()
    {
        //trigger slap
        if (currentState != PlayerStates.Slapping)
        {
            animator.SetTrigger("Slap");
            currentState = PlayerStates.Slapping;
            slapTimer = slapTime;
        }
    }
}
