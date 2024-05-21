using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SenseComp : MonoBehaviour
{
    [SerializeField] float forgettingTime = 3f;
    static List<PerceptionStimuli> registeredStimulis = new List<PerceptionStimuli>(); // THIS LIST STORES ANY REGISTERED STIMULIS (which is in our case the player)
    List<PerceptionStimuli> PerceivableStimulis = new List<PerceptionStimuli> (); // STORES ALL THE CURRENT PERCEIVABLE STIMULIS

    Dictionary<PerceptionStimuli, Coroutine> ForgettingRoutines = new Dictionary<PerceptionStimuli, Coroutine>(); // EACH STIMULI HAS A COROUTINE

    public delegate void OnPerceptionUpdated(PerceptionStimuli stimuli, bool succsessfulySensed);

    public event OnPerceptionUpdated onPerceptionUpdated;

    static public void RegisterStimuli(PerceptionStimuli stimuli)
    {
        if (registeredStimulis.Contains(stimuli))
            return;
        
        registeredStimulis.Add(stimuli);
    }

    static public void UnRegisterStimuli(PerceptionStimuli stimuli)
    {
        registeredStimulis.Remove(stimuli);
    }

    protected abstract bool IsStimuliSensable(PerceptionStimuli stimuli);

    // Update is called once per frame
    void Update() // THIS WILL EXECUTED ONCE PER FRAME, so either 60 times per second or 30 times per second(depending on the fps)
    {
        foreach(var stimuli in registeredStimulis) // IN OUR CASE WE ONLY HAVE ONE STIMULI, but we just want to make this flexible
        {
            if(IsStimuliSensable(stimuli)) // IF THE PLAYER IS SENSED BY THIS SENSE
            {
                if(!PerceivableStimulis.Contains(stimuli)) // check if we already sensing him
                {
                    PerceivableStimulis.Add(stimuli); 
                    if(ForgettingRoutines.TryGetValue(stimuli, out Coroutine routine)) // IF THERE IS A COUROTINE RUNNING
                    {
                        StopCoroutine(routine); // STOP IT
                        ForgettingRoutines.Remove(stimuli); // REMOVE STIMULI FROM FORGETTING ROUTINES BECAUSE WE ARE NOW SENSING HIM
                        // WE DIDNT INVOKE A FUNCTION BECAUSE THE STIMULI IS ALREADY SET TO TRUE
                    }
                    else
                    {
                        onPerceptionUpdated?.Invoke(stimuli, true); // IF WE SPOTTED THE PLAYER, TRIGGER AN EVENT
                    }
                }
            }
            else
            {
                if(PerceivableStimulis.Contains(stimuli))
                {
                    PerceivableStimulis.Remove(stimuli);
                    ForgettingRoutines.Add(stimuli, StartCoroutine(ForgetStimuli(stimuli)));
                }
            }
        }
    }

    internal void AssignPerceivedStimuli(PerceptionStimuli targetStimuli)
    {
        PerceivableStimulis.Add(targetStimuli);
        onPerceptionUpdated?.Invoke(targetStimuli, true);


        if(ForgettingRoutines.TryGetValue(targetStimuli, out Coroutine forgetCoroutine))
        {
            StopCoroutine(forgetCoroutine);
            ForgettingRoutines.Remove(targetStimuli);
        }
    }

    IEnumerator ForgetStimuli(PerceptionStimuli stimuli)
    {
        yield return new WaitForSeconds(forgettingTime);
        ForgettingRoutines.Remove(stimuli);
        onPerceptionUpdated?.Invoke(stimuli, false);
    }

    protected virtual void DrawDebug()
    {

    }

    private void OnDrawGizmos()
    {
        DrawDebug();
    }
}
