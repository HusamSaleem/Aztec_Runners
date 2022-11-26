using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float trailDistance = 4f;
    public float heightOffset = 3.0f;
    public float cameraDelay = 0.05f;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 followPos = target.position - target.forward * trailDistance;

        followPos.y += heightOffset;
        transform.position += (followPos - transform.position) * cameraDelay;

        transform.LookAt(target.transform);
    }
}
