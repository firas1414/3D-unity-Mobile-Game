using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenseComponent : MonoBehaviour
{
    [SerializeField] float forgettingTime = 3f;

    static List<PerceptionStimuli> registeredStimulis = new List<PerceptionStimuli>(); // Static means that the list is the same for the whole class, and every instance will have that same list
    List<PerceptionStimuli> PerceivableStimulis = new List<PerceptionStimuli>();
    Dictionary<PerceptionStimuli, Coroutine> ForgettingRoutines = new Dictionary<PerceptionStimuli, Coroutine>();

    public delegate void OnPerceptionUpdated(PerceptionStimuli stimuli, bool successfulySensed);
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
            if(IsStimuliSensable(stimuli)){
                if(!PerceivableStimulis.Contains(stimuli)){ // The object just sensed the player
                    PerceivableStimulis.Add(stimuli);
                    if(ForgettingRoutines.TryGetValue(stimuli, out Coroutine routine)) // If there is already a routine going on, stop it because the enemmy sensed
                    {
                        StopCoroutine(routine);
                        ForgettingRoutines.Remove(stimuli);
                        Debug.Log($"I sensed{stimuli.gameObject}");
                    }
                    else // If there is not a routine...
                    {
                        onPerceptionUpdated?.Invoke(stimuli, true);
                        
                    }
                }
            }
            else{
                if(PerceivableStimulis.Contains(stimuli)){
                    PerceivableStimulis.Remove(stimuli); // The object lost sense
                    ForgettingRoutines.Add(stimuli, StartCoroutine(ForgetStimuli(stimuli))); // Started Forgetting routine
                    Debug.Log($"I lost sense{stimuli.gameObject}");
                }
            }
        }
    }

    IEnumerator ForgetStimuli(PerceptionStimuli stimuli)
    {
        yield return new WaitForSeconds(forgettingTime);
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
