using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenseComponent : MonoBehaviour
{
    [SerializeField] float forgettingTime = 3f;

    static List<PerceptionStimuli> registeredStimulis = new List<PerceptionStimuli>(); // Static means that the list is the same for the whole class, and every instance will have that same list
    List<PerceptionStimuli> PerceivableStimulis = new List<PerceptionStimuli>(); // Stimulis that the enemy is currently sensing
    Dictionary<PerceptionStimuli, Coroutine> ForgettingRoutines = new Dictionary<PerceptionStimuli, Coroutine>(); // (Player:Coroutine)

    public delegate void OnPerceptionUpdated(PerceptionStimuli stimuli, bool successfulySensed); // successfulySensed=true if the enemy is sensing, false if he lost track
    public event OnPerceptionUpdated onPerceptionUpdated;

    static public void RegisterStimuli(PerceptionStimuli stimuli)
    {
        if(!registeredStimulis.Contains(stimuli)){
            registeredStimulis.Add(stimuli);
        }
    }

    static public void UnRegisterStimuli(PerceptionStimuli stimuli){
        registeredStimulis.Remove(stimuli);
    }

    protected abstract bool IsStimuliSensable(PerceptionStimuli stimuli);


    // Update is called once per frame
    void Update()
    {
        foreach(var stimuli in registeredStimulis){
            if(IsStimuliSensable(stimuli)){ // The enemy is sensing the player
                if(!PerceivableStimulis.Contains(stimuli)){ // This if is used to not keep adding the stimuli again and again when the player is sensing him
                    PerceivableStimulis.Add(stimuli);
                    if(ForgettingRoutines.TryGetValue(stimuli, out Coroutine routine)) // If there is already a routine going on, stop it because the enemmy sensed again before the routine ended
                    {
                        StopCoroutine(routine);
                        ForgettingRoutines.Remove(stimuli);
                    }
                    else // If there is no routine, just sense
                    {
                        onPerceptionUpdated?.Invoke(stimuli, true);
                        
                    }
                }
            }
            else{ // The enemy lost sense but not track
                if(PerceivableStimulis.Contains(stimuli)){
                    PerceivableStimulis.Remove(stimuli); // Remove the stimuli from the list because the enemy is not sensing the Stimuli(Player), (but still have track)
                    ForgettingRoutines.Add(stimuli, StartCoroutine(ForgetStimuli(stimuli))); // Started Forgetting routine      
                }
            }
        }
    }

    IEnumerator ForgetStimuli(PerceptionStimuli stimuli) // if this coroutine eneded, the enemy lost track(lost sense completelty)
    {
        Debug.Log($"I lost sense but i still have track of {stimuli.gameObject}, and i will lose track after 3 seconds");
        yield return new WaitForSeconds(forgettingTime); // This line will last 3 seconds, after that the next lines will be executed
        ForgettingRoutines.Remove(stimuli);
        onPerceptionUpdated?.Invoke(stimuli, false);
        Debug.Log($"I lost track of {stimuli.gameObject}");
    }

    protected virtual void DrawDebug()
    {

    }
    private void OnDrawGizmos()
    {
        DrawDebug();
    }
}
