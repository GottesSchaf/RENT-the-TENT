using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] float drag = 0.9f;

    Rigidbody rb;

	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));

        if(input.magnitude > 1)
        {
            input.Normalize();
        }

        rb.velocity *= drag;
        rb.velocity = input * speed;

	}

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), "Player Velocity: " + rb.velocity);
    }
}
