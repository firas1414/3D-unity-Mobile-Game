using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Fire")]
public class FireAbility : Ability
{
    [Header("Fire")]
    [SerializeField] Scaner ScanerPrefab;
    [SerializeField] float fireRadius;
    [SerializeField] float fireDuration;
    [SerializeField] float damageDuration = 3f;
    [SerializeField] float fireDamage = 20f;
    
    [SerializeField] GameObject ScanVFX;
    [SerializeField] GameObject DamageVFX;

    public override void ActivateAbility()
    {
        if (!CommitAbility()) return; // TESTS IF THE PLAYER HAVE ENOUGH STAMINA
        Scaner fireScaner = Instantiate(ScanerPrefab, AbilityComp.transform);
        // AbilityComp.transform is the position and orientation where the new Scaner object will be instantiated, 
        // which in our case is the same position as the player's since the AbilityComponent Class is attached to the player GameObject
        fireScaner.SetScanRange(fireRadius);
        fireScaner.SetScanDuration(fireDuration);
        fireScaner.AddChildAttached(Instantiate(ScanVFX).transform); // SETS THE VFX'S POSITION THE SAME AS THE PIVOT POSITION WHICH IS DEFINED IN THE Scanner Class(ScanerPivot), IN SUMMARY SET VFX POSITION TO THE PLAYER'S POSITION
        fireScaner.onScanDetectionUpdated += DetectionUpdate;
        fireScaner.StartScan();
    }

    private void DetectionUpdate(GameObject newDetection)
    {
        ITeamInterface detectedTeamInterface = newDetection.GetComponent<ITeamInterface>(); // GET THE ITeamInterface OF THE DETECTED OBJECT 
        if(detectedTeamInterface == null || detectedTeamInterface.GetRelationTowards(AbilityComp.gameObject) != ETeamRelation.Enemy) // IF THAT OBJECT IS NOT AN ENEMY TO THE PLAYER, DO NOTHING
        {
            return;
        }

        HealthComponent enemyHealthComp = newDetection.GetComponent<HealthComponent>(); // GET THE HealthComponent OF THE DETECTED OBJECT
        if(enemyHealthComp == null)
        {
            return;
        }

        AbilityComp.StartCoroutine(ApplyDamageTo(enemyHealthComp));
    }

    private IEnumerator ApplyDamageTo(HealthComponent enemyHealthComp)
    {
        GameObject damageVFX = Instantiate(DamageVFX, enemyHealthComp.transform);
        float damageRate = fireDamage / damageDuration;
        float startTime = 0;
        while(startTime < damageDuration && enemyHealthComp != null)
        {
            startTime += Time.deltaTime;
            enemyHealthComp.changeHealth(-damageRate * Time.deltaTime, AbilityComp.gameObject);
            yield return new WaitForEndOfFrame();
        }

        if (damageVFX != null)
            Destroy(damageVFX);
    }
}
