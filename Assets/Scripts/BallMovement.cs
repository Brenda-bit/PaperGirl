using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject Player;
    private bool isBall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * 7f, rb.velocity.y);

        if (Input.GetKeyDown("space"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 8f);
        }
        hidePlayer();
    }
    private void hidePlayer()
    {
        if (Input.GetKeyDown("C") && !isBall)
        {
            isBall = true;
            Player.SetActive(false);
            //transform.position = Player.transform.position;
        }
        else if (Input.GetKeyDown("C") && isBall)
        {
            isBall = false;
            Player.SetActive(true);
        }
    }
}
