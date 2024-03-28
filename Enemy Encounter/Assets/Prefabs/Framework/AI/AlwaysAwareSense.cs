using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysAware : SenseComponent
{
    [SerializeField] private float awareDistance = 2f;
    protected override bool IsStimuliSensable(PerceptionStimuli stimuli) // Checks if a particular stimuli is within the sensing range (awareDistance) of the AlwaysAware object
    {
        return Vector3.Distance(transform.position, stimuli.transform.position) <= awareDistance;
        /*
        this line of code calculates the distance between the position of the GameObject that this script is attached to (transform.position)
        and the position of another GameObject represented by stimuli.transform.position(Player in our case).
        If this distance is less than or equal to the awareDistance variable, the method returns true, indicating that the stimulus is within the sensing range.
        Otherwise, it returns false, indicating that the stimulus is outside of the sensing range.
        */
    }

    protected override void DrawDebug() // Draws a wire sphere in the editor to represent the sensing range (awareDistance) around the object.
    {
        base.DrawDebug();
        Gizmos.DrawWireSphere(transform.position + Vector3.up, awareDistance);
    }
}
