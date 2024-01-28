using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    [SerializeField] Camera camera;
    [SerializeField] float slapTime;
    public int items;
    float slapTimer;
    public Stack<GameObject> bag = new Stack<GameObject>();
    public Vector3 itemOffset;
    public float minItemDropRadius;
    public float maxItemDropRadius;

    public enum PlayerStates
    {
        Idle,
        Walking,
        Slapping,
        Hurt,
        Dead
    }

    public PlayerStates currentState = PlayerStates.Idle;
    public TextMeshProUGUI itemText;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy_slap")
        {
            Debug.Log("player slapped");
            DropItems();
        }

        if (collision.gameObject.tag == "item")
        {
            collision.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            Debug.Log("item grab");
            collision.transform.parent = transform;

            collision.transform.localPosition = Vector3.zero + (itemOffset * bag.Count);
            bag.Push(collision.gameObject);
            itemText.text = bag.Count.ToString();
        }
    }

    private void DropItems()
    {
        //drop all items from bag 

        //get player position
        Vector3 ppos = gameObject.transform.position;

        //iterate through bag
        while (bag.Count != 0) 
        {
            //get random x and y near the player
            float rx = Random.Range(minItemDropRadius, maxItemDropRadius);
            float ry = Random.Range(minItemDropRadius, maxItemDropRadius);
            int coin = Random.Range(0, 2);
            if(coin == 0)
            {
                coin = -1;
            } else
            {
                coin = 1;
            }
            Vector3 itemDropLoc = new Vector3(ppos.x + (coin * rx), ppos.y + (coin * ry), 0f);
            //unparent item from player
            GameObject currentItem = bag.Pop();
            currentItem.transform.parent = null;
            currentItem.transform.position = itemDropLoc;
            currentItem.GetComponent<PolygonCollider2D>().enabled = true;
        }

        itemText.text = bag.Count.ToString();


    }
}
