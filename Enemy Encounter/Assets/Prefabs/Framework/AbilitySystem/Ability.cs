using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// HANDLES THE ABILITIES MECHANISM (handles the abilities part, like knowing how much stamina each ability needs to consume)
public abstract class Ability : ScriptableObject
{
    [SerializeField] Sprite AbilityIcon;
    [SerializeField] float staminaCost = 10f;
    [SerializeField] float cooldownDuration = 2f;

    [Header("Audio")]
    [SerializeField] AudioClip AbilityAudio;
    [SerializeField] float volume = 1f;

    public AbilityComponent AbilityComp
    { 
        get { return abilityComponent; }
        private set { abilityComponent = value; }
    }

    AbilityComponent abilityComponent;

    bool abilityOnCooldown = false;

    public delegate void OnCooldownStarted();
    public OnCooldownStarted onCooldownStarted;

    internal Sprite GetAbilityIcon()
    {
        return AbilityIcon;
    }

    internal void InitAbility(AbilityComponent abilityComponent)
    {
        this.abilityComponent = abilityComponent;
    }

    public abstract void ActivateAbility();


    // THIS METHOD WILL BE CALLED IN THE CHILD CLASS SINCE EACH ABILITY HAS IT'S OWN ABILITY POWER
    protected bool CommitAbility() // CHECKS WETHER ALL THE CONDITIONS NEEDED TO ACTIVATE THE ABILITY ARE MET
    {
        if (abilityOnCooldown) return false; // CHECKS IF THE ABILITY IS STILL ON COOLDOWN

        if(abilityComponent == null || !abilityComponent.TryConsumeStamina(staminaCost)) // CHECK IF THERES IS NO SUFFICIENT STAMINA
            return false;

        StartAbilityCooldown();
        GameplayStatics.PlayAudioAtPlayer(AbilityAudio, volume);
        //...

        return true;
    }

    internal float GetCooldownDuration()
    {
        return cooldownDuration;
    }

    void StartAbilityCooldown()
    {
        abilityComponent.StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        abilityOnCooldown = true;
        onCooldownStarted?.Invoke();
        yield return new WaitForSeconds(cooldownDuration);
        abilityOnCooldown = false;
    }
}
