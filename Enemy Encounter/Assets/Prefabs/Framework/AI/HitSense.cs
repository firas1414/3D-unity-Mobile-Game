using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSense : SenseComponent
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

    private void TookDamage(float current_Health, float amount, float maxHealth, GameObject Hitter)
    {
        PerceptionStimuli stimuli = Hitter.GetComponent<PerceptionStimuli>();
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
    
    IEnumerator ForgetStimuli(PerceptionStimuli stimuli) // if this coroutine eneded, the enemy lost track(lost sense completelty)
    {
        yield return new WaitForSeconds(HitMemory); // This line will last 3 seconds, after that the next lines will be executed
        HitRecord.Remove(stimuli);
    }
}
