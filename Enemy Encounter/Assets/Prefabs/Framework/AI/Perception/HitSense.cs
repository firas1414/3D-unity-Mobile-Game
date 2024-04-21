using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSense : SenseComp
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] float HitMemory = 2f;

    Dictionary<PerceptionStimuli, Coroutine> HitRecord = new Dictionary<PerceptionStimuli, Coroutine>();
    protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        return HitRecord.ContainsKey(stimuli);
    }

    // Start is called before the first frame update
    void Start()
    {
        healthComponent.onTakeDamage += TookDamage;
    }

    private void TookDamage(float health, float delta, float maxHealth, GameObject Instigator)
    {
        PerceptionStimuli stimuli = Instigator.GetComponent<PerceptionStimuli>();
        if(stimuli != null)
        {
            Coroutine newForgettingCoroutine = StartCoroutine(ForgetStimuli(stimuli));
            if(HitRecord.TryGetValue(stimuli, out Coroutine onGoingCoroutine))
            {
                StopCoroutine(onGoingCoroutine);
                HitRecord[stimuli] = newForgettingCoroutine;
            }
            else
            {
                HitRecord.Add(stimuli, newForgettingCoroutine);
            }
        }
    }

    IEnumerator ForgetStimuli(PerceptionStimuli Stimuli)
    {
        yield return new WaitForSeconds(HitMemory);
        HitRecord.Remove(Stimuli);
    }
}
