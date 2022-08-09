using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GoInDirection : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public Vector2 startMarker;
    public Vector2 point;

    private bool Moving = false;

    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    public float startTime;

    // Total distance between the markers.
    public float journeyLength;

    private void OnEnable()
    {
        

        startMarker = transform.position;
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector2.Distance(startMarker, point);

        Moving = true;
    }

    // Follows the target position like with a spring
    void Update()
    {
        if (!Moving)
        {
            return;
        }

        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector2.Lerp(startMarker, point, fracJourney);
    }
}
