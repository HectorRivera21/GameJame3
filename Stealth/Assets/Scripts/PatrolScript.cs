using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolScript : MonoBehaviour
{
    public Transform[] waypoints;  // Array of waypoints
    public int targetPoint = 0;    // Index of the current target waypoint
    public float speed = 2.0f;     // Movement speed
    private bool movingForward = true;  // Direction flag
    private bool isRotating = false;    // Flag to check if rotating

    public float rotationSpeed = 180f;  // Speed of rotation (degrees per second)
    public float rotationDuration = 1f; // How long the 360 rotation should take

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        if (waypoints.Length == 0 || isRotating)
            return; // Don't move if no waypoints are set or during rotation

        // Move towards the target waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetPoint].position, speed * Time.deltaTime);

        // Check if the object has reached the target waypoint
        if (transform.position == waypoints[targetPoint].position)
        {
            if (movingForward)
            {
                targetPoint++;
                if (targetPoint >= waypoints.Length)  // Reached last waypoint
                {
                    targetPoint = waypoints.Length - 1;  // Stay at the last point
                    movingForward = false;  // Reverse direction
                    StartCoroutine(RotateObject());  // Trigger 360-degree rotation
                }
            }
            else
            {
                targetPoint--;
                if (targetPoint < 0)  // Reached first waypoint
                {
                    targetPoint = 0;  // Stay at the first point
                    movingForward = true;  // Change direction to forward
                    StartCoroutine(RotateObject());  // Trigger 360-degree rotation
                }
            }
        }
    }

    // Coroutine to rotate the object 360 degrees
    IEnumerator RotateObject()
    {
        isRotating = true;  // Prevent movement while rotating

        float elapsedTime = 0f;
        float startRotation = transform.eulerAngles.y;  // Get the current Y rotation
        float endRotation = startRotation + 180f;  // Target 360-degree rotation

        while (elapsedTime < rotationDuration)
        {
            float currentRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);  // Smoothly interpolate
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentRotation, transform.eulerAngles.z);  // Apply rotation
            elapsedTime += Time.deltaTime;
            yield return null;  // Wait until next frame
        }

        // Ensure it finishes exactly at 360 degrees
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotation % 360f, transform.eulerAngles.z);

        isRotating = false;  // Resume movement after rotation
    }
}
