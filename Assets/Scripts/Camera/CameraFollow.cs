using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    private Vector3 cameraOffset;
    private float distance;

    private void Awake()
    {
        cameraOffset = this.transform.position - target.transform.position;
        GameObject.Destroy(target.gameObject);
        target = null;
    }
    
    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        transform.position = cameraOffset + target.position;
        // ScrollView();
        RotateView();
    }

    // private void ScrollView()
    // {
    //     Debug.Log("------------------------------");
    //     distance = (target.transform.position - cameraOffset).magnitude;
    //     //向前滑动拉近 向后滑动拉远
    //     distance -= Input.GetAxis("Mouse ScrollWheel") * 50;
    //     distance = Mathf.Clamp(distance, 10, 400);
    //     cameraOffset = cameraOffset.normalized * distance;
    // }
    
    
    private void RotateView()
    {
        //鼠标右键按下可以旋转视野
        if (Input.GetMouseButton(1))

        {
            Vector3 originalPosition = transform.position;
            Quaternion originalRotation = transform.rotation;
            transform.RotateAround(target.position, target.up, 30 * Input.GetAxis("Mouse X"));
            transform.RotateAround(target.position, transform.right, -30 * Input.GetAxis("Mouse Y"));
            float x = transform.eulerAngles.x;
            
            if (x < 10 || x > 50)
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }
        }

        cameraOffset = transform.position - target.position;
    }
}