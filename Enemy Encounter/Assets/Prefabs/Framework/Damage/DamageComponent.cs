using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageComponent : MonoBehaviour, ITeamInterface
{
    [SerializeField] protected bool DamageFriendly;
    [SerializeField] protected bool DamageEnemy;
    [SerializeField] protected bool DamageNeutral;
    ITeamInterface teamInterface;
    public int GetTeamID()
    {
        if(teamInterface != null)
            return teamInterface.GetTeamID();
        return -1;
    }

    public void SetTeamInterfaceSrc(ITeamInterface teamInterface)
    {
        this.teamInterface = teamInterface; // This makes the DamageComponent use the ITeamInterface of the Object that has a script called either Player or enemy_name script attached to it
    }

    public bool ShouldDamage(GameObject other)
    {
        if (teamInterface == null)
            return false;

        ETeamRelation relation = teamInterface.GetRelationTowards(other); // This will let us know if the game object we're seing is wether : neutral, friendly or enemy
        if (DamageFriendly && relation == ETeamRelation.Friendly) // If i should damage the friendly, and the gameobject im seing is friendly, then i should damage that gamobject
            return true;

        if (DamageEnemy && relation == ETeamRelation.Enemy) // same principle
            return true;

        if (DamageNeutral && relation == ETeamRelation.Neutral)
            return true;

        return false;
    }
   
}
