using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSense : SenseComponent
{
    [SerializeField] private float sightDistance = 5f;
    [SerializeField] private float sightHalfAngle = 5f;
    [SerializeField] private float eyeHeight = 1f; // eyeHeight determines how high off the ground the raycast starts from.


	protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        float distance = Vector3.Distance(stimuli.transform.position, transform.position); // Distance between player and enemy
        if(distance > sightDistance)
        {
            return false;
        }
        Vector3 forwardDir = transform.forward; // Enemy forward direction
        Vector3 stimuliDir = (stimuli.transform.position - transform.position).normalized; // Directon from enemy to player
        /*
        Normalizing a vector means scaling it to have a length of 1 while preserving its direction.
        This is important for directional calculations because it ensures that the length of the vector does not affect the comparison of directions.
        */
        if(Vector3.Angle(forwardDir, stimuliDir) > sightHalfAngle) // Check if player is in the enemy's sight angle or not
        {
            return false;
        }
        if(Physics.Raycast(transform.position + (Vector3.up * eyeHeight), stimuliDir, out RaycastHit hitInfo, sightDistance)) // Check wether the player is standing behind an object
        {
            if(hitInfo.collider.gameObject != stimuli.gameObject) // If the raycast is not hitting the player then it's defiently hitting another object, then we can't see the player
            {
                return false;
            }
        }
        return true;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();
        Vector3 drawCenter = transform.position + Vector3.up * eyeHeight;
        Gizmos.DrawWireSphere(drawCenter, sightDistance);
        Vector3 leftLimitDir = Quaternion.AngleAxis(sightHalfAngle, Vector3.up) * transform.forward;
        Vector3 rightLimitDir = Quaternion.AngleAxis(-sightHalfAngle, Vector3.up) * transform.forward;
        Gizmos.DrawLine(drawCenter, drawCenter + leftLimitDir * sightDistance);
        Gizmos.DrawLine(drawCenter, drawCenter + rightLimitDir * sightDistance);
    }

}
