using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    public float jumpForce = 8f;
    public float ledgeGrabPullUpForce = 5f;
    public LayerMask ledgeLayerMask;
    private bool isJumping = false;
    private bool isClimbing = false;
    private bool isGrounded;
    public float climbSpeed = 2f;
    private GameObject[] spotArray;
    private Vector3 findSpot;
    private bool isSwitched;
    public bool isActive = true;

    // Start is called before the first frame update
    private void Start()
    {
        spotArray = GameObject.FindGameObjectsWithTag("StandPosition");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        findSpot = GetClosestSpot();
        float verticalInput = 0;
        float horizontal = 0;
        
        if (isGrounded)
        {
             verticalInput = Input.GetAxis("Vertical");
             horizontal = Input.GetAxis("Horizontal");
             rb.velocity = new Vector2(horizontal * 12f, rb.velocity.y);
            if (Input.GetKeyDown("space"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
            }

        }
        if (isClimbing)
        {
            verticalInput = Input.GetAxis("Vertical");
            ClimbLadder(verticalInput);
        }
       
        // Check for ledge grab when jumping
        if (isJumping)
        {
            CheckLedgeGrab(findSpot);
        }
    }
    private void CheckLedgeGrab(Vector3 closestSpot)
    {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, ledgeLayerMask);
            
            if (hit.collider != null)
            {
            Debug.DrawRay(transform.position, Vector2.up, Color.cyan);
                Debug.Log("Here");
                // Here, we've detected a ledge, so we need to stop vertical movement and snap the player to the ledge
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z);

                // Implement ledge grab pull-up mechanic (optional)
                if (Input.GetKeyDown("space"))
                {
                    LedgeGrabPullUp(closestSpot);
                }
            }
    }
    void LedgeGrabPullUp(Vector3 closestSpot)
    {
        this.transform.position = closestSpot;
        isGrounded = true;
    }

    void ClimbLadder(float verticalInput)
    {
        rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Aplicar força para a direita
            rb.AddForce(Vector2.right * 1f, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Aplicar força para a direita
            rb.AddForce(Vector2.left * 1f, ForceMode2D.Impulse);
        }
    }

    void StopClimbing()
    {
        rb.gravityScale = 1f;
        isClimbing = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            // Check if the player is trying to climb up or down
            float verticalInput = Input.GetAxis("Vertical");
            ClimbLadder(verticalInput);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            StopClimbing();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            isJumping = false;
        }
       
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    public Vector3 GetClosestSpot()
    {
        float closestSpot = Mathf.Infinity;
        Vector3 trans = Vector3.zero;

        foreach(GameObject spot in spotArray)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, spot.transform.position);
            if(currentDistance < closestSpot)
            {
                closestSpot = currentDistance;
                trans = spot.transform.position;
            }
        }
        return trans;
    }
    
}


