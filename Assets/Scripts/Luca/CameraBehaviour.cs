using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {


    [SerializeField] Transform toFollow;
    Vector3 displacement;

	void Start () {
        displacement = transform.position - toFollow.position;	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = toFollow.position + displacement;
	}
}
