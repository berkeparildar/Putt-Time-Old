using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    public float jumpForce = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            var ballRb = other.gameObject.GetComponent<Rigidbody>();
            ballRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
