using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PROJECTILE IS THE PARTICLE ISSUED FROM THE SPITTER'S MOUTH WHEN ATTACKING THE PLAYER
// USED FOR MAKING THE SPITTING MECHANISM, THIS CLASS SHOULD BE APPLIED ON THE PROJECTILE PREFAB
public class Projectile : MonoBehaviour
{
    [SerializeField] float FlightHeight;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] DamageComponent DamageComponent; // YOU CAN APPLY THE "TriggreDamager" CLASS SINCE THAT CLASS IS INHERITED FROM THE "DamageComponent" CLASS
    [SerializeField] ParticleSystem ExplosionVFX; // THIS PARTICLE IS RESPONSIBLE FOR MAKING THE EXPLOSION, IT IS SPAWNED ONCE THE FIRE HITS THE PLAYER'S COLLIDER
    ITeamInterface instigatorTeamInterface;

    public void Launch(GameObject instigator, Vector3 Destination)
    {
        instigatorTeamInterface = instigator.GetComponent<ITeamInterface>();
        if (instigatorTeamInterface != null)
        {
            DamageComponent.SetTeamInterfaceSrc(instigatorTeamInterface);
        }

        // CALCULATING THE PARTICLE MOVEMENT MECHANISM
        float gravity = Physics.gravity.magnitude;
        float halfFlightTime = Mathf.Sqrt((FlightHeight * 2.0f) / gravity);

        Vector3 DestinationVec = Destination - transform.position;
        DestinationVec.y = 0;
        float horizontalDist = DestinationVec.magnitude;
        
        float upSpeed = halfFlightTime * gravity;
        float fwdSpeed = horizontalDist /(2.0f * halfFlightTime);

        Vector3 flightVel = Vector3.up * upSpeed + DestinationVec.normalized * fwdSpeed;
        rigidBody.AddForce(flightVel, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(instigatorTeamInterface.GetRelationTowards(other.gameObject) != ETeamRelation.Friendly) // CHECKS IF THE OBJECT THAT THE PARTICLE COLLIDED WITH IS A PLAYER OR NOT, IF YES DESTROY THE PARTICLE AND MAKE AN EXMPLOSION EFFECT
        {
            Explode();
        }
    }

    private void Explode()
    {
        Vector3 pawnPos = transform.position;
        Instantiate(ExplosionVFX, pawnPos, Quaternion.identity);
        Destroy(gameObject); // DESTROY THE PARTICLE ONCE COLLIDED WITH THE PLAYER'S COLLIDER
    }
}
