using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    // soon the code able to get min and max values of scene boundaries
    [SerializeField]
    private Vector3 minValues, maxValues;

    [SerializeField] 
    private Transform playerTarget;
    private void Update()
    {
        // player position
        Vector3 targetPosition = playerTarget.position + offset;
        // verify targetposition is out of bound or not
        // limit it to min and max values
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z)
            );

        transform.position = Vector3.SmoothDamp(transform.position, boundPosition, ref velocity, smoothTime);
    }
}
