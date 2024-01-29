using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;
    [SerializeField] float slapTime;
    public int items;
    float slapTimer;
    public float hurtTime;
    public float hurtTimer;
    //public Stack<GameObject> bag = new Stack<GameObject>();
    public List<GameObject> bag = new List<GameObject>();
    public Vector3 itemOffset;
    public float minItemDropRadius;
    public float maxItemDropRadius;
    public float lookRange;
    public float slapRange;
    public float chaseSpeed;
    public GameObject player;
    public bool bagEmpty = false;
    public float attackWaitTime;
    public float attackWaitTimer;
    PlayerController pc;

    public enum EnemyStates
    {
        Idle,
        Walking,
        Angered,
        Slapping,
        Hurt,
        Dead
    }

    public EnemyStates currentState = EnemyStates.Idle;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        pc = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == EnemyStates.Hurt)
        {
            hurtTimer -= Time.deltaTime;
            if(hurtTimer < 0f)
            {
                currentState = EnemyStates.Angered;
                animator.SetBool("hurt", true);
            }
        }

        if(currentState == EnemyStates.Angered)
        {
            Angered();
        }

        if (currentState == EnemyStates.Slapping)
        {
            slapTimer -= Time.deltaTime;

            if (slapTimer <= 0f)
            {
                currentState = EnemyStates.Idle;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player_slap")
        {
            player = collision.gameObject;
            if (!bagEmpty)
            {
                DropItems();
            }
            else {
                currentState = EnemyStates.Dead;
                animator.SetBool("hurt", false);
                animator.SetBool("dead", true);
            }
            
            
        }
    }

    private void DropItems()
    {
        //drop all items from bag 

        //get player position
        Vector3 ppos = gameObject.transform.position;

        //iterate through bag

    
        for(int i = 0; i < bag.Count;i++)
        {
            //get random x and y near the player
            float rx = Random.Range(minItemDropRadius, maxItemDropRadius);
            float ry = Random.Range(minItemDropRadius, maxItemDropRadius);
            int coin = Random.Range(0, 2);
            if (coin == 0)
            {
                coin = -1;
            }
            else
            {
                coin = 1;
            }
            Vector3 itemDropLoc = new Vector3(ppos.x + (coin * rx), ppos.y + (coin * ry), 0f);
            //unparent item from player
            GameObject currentItem = bag[i];
            currentItem.transform.parent = null;
            currentItem.transform.position = itemDropLoc;
            currentItem.GetComponent<PolygonCollider2D>().enabled = true;
        }

        bagEmpty = true;
        currentState = EnemyStates.Hurt;
        hurtTimer = hurtTime;


    }

    public void Angered()
    {
        //check for player distance
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        //if in range
        if (distance <= lookRange)
        {
            Vector3 look = transform.InverseTransformPoint(player.transform.position);
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg - 90;
            //face toward player
            transform.Rotate(0, 0, angle);
            //move toward player
            rigidbody2D.velocity = transform.up * chaseSpeed;
            //if in range slap player
            if((distance <= slapRange) && (attackWaitTimer == 0))
            {
                animator.SetTrigger("slap");
                currentState = EnemyStates.Slapping;
                slapTimer = slapTime;
                attackWaitTimer = attackWaitTime;
            }

            attackWaitTimer -= Time.deltaTime;
            if(attackWaitTimer < 0)
            {
                attackWaitTimer = 0;
            }

        }
        else
        {
            //if player is not in range return to idle state
            animator.SetBool("hurt", false);
            currentState = EnemyStates.Idle;
            attackWaitTimer = 0;
        }
    }
}
