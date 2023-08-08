using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLight : MonoBehaviour
{
    private bool isActive = true;
    private Collider2D objectCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        Transform child = gameObject.transform.GetChild(0);
        
        Debug.Log(child);
        if (collision.CompareTag("Player") && Input.GetKey(KeyCode.C))
        {
            isActive = !isActive;
            child.gameObject.SetActive(isActive);
            Debug.Log(collision.gameObject);
        }
    }
}
