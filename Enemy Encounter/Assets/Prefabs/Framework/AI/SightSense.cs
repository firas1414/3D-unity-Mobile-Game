using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSense : SenseComponent
{
    [SerializeField] private float sightDistance = 5f;
    [SerializeField] private float sightHalfAngle = 5f;
    [SerializeField] private float eyeHeight = 1f;


	protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        float distance = Vector3.Distance(stimuli.transform.position, transform.position);
        if(distance > sightDistance)
        {
            return false;
        }
        Vector3 forwardDir = transform.forward; // Enemy forward direction
        Vector3 stimuliDir = (stimuli.transform.position - transform.position).normalized; // Player's forward direction
        if(Vector3.Angle(forwardDir, stimuliDir) > sightHalfAngle) // Check if player is the enemy's sight angle or not
        {
            return false;
        }
        if(Physics.Raycast(transform.position + Vector3.up * eyeHeight, stimuliDir, out RaycastHit hitInfo, sightDistance)) // Chck wether the player is standing behind an object
        {
            if(hitInfo.collider.gameObject != stimuli.gameObject)
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
