using System.Collections;
using UnityEngine;

public class AxeArc : Arc
{
    public override IEnumerator TravelArc(Vector3 destination, float duration)
    {
        // Grab the current gameobject's position
        Vector3 startPosition = transform.position;

        float percentComplete = 0.0f;

        // check that percentComplete is less than 100%
        while(percentComplete < 1.0f)
        {
            // Time elapsed since the last frame, divided by the total desired duration = a percentage of the duration
            percentComplete += Time.deltaTime / duration;

            // To make the object move smoothly between two points at a constant speed, use Linear Interpolation
            // No matter where the AmmoObject is fired from, it will take the same amount of time to get there
            // Determines the distance to travel per frame
            // Returns a point between start and end based on the percentage
            transform.position = Vector3.Lerp(startPosition, destination, percentComplete);

            yield return null;
        }
    }

    public IEnumerator ReturnArc(Vector3 destination, float duration)
    {
        // Grab the current gameobject's position
        Vector3 startPosition = transform.position;
        float percentComplete = 0.0f;

        while(percentComplete < 1.0f)
        {
            // Time elapsed since the last frame, divided by the total desired duration = a percentage of the duration
            percentComplete += Time.deltaTime / duration;

            // To make the object move smoothly between two points at a constant speed, use Linear Interpolation
            // No matter where the AmmoObject is fired from, it will take the same amount of time to get there
            // Determines the distance to travel per frame
            // Returns a point between start and end based on the percentage
            transform.position = Vector3.Lerp(startPosition, destination, percentComplete);
            yield return null;
        }
    }
}