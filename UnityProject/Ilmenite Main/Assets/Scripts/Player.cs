using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameObject eyeCamera;

    new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.velocity += transform.rotation * new Vector3(0, 0, .5f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody.velocity += transform.rotation * new Vector3(0, 0, -.5f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.velocity += transform.rotation * new Vector3(-.3f, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.velocity += transform.rotation * new Vector3(.3f, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<Rigidbody>().velocity += new Vector3(0, 3, 0);
        }
    }
}
