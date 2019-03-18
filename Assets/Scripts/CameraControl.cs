using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    void Start ()
    {

    }
    public float speed = 25f;

    void Update()
    {
        Vector3 translate = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        this.transform.Translate(translate * speed * Time.deltaTime, Space.World);

    }

}