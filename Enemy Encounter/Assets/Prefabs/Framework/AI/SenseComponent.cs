using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenseComponent : MonoBehaviour
{
    static List<PerceptionStimuli> registeredStimulis = new List<PerceptionStimuli>(); // Static means that the list is the same for the whole class, and every instance will have that same list
    List<PerceptionStimuli> PerceivableStimulis = new List<PerceptionStimuli>();

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
                if(!PerceivableStimulis.Contains(stimuli)){ // The object just sensed
                    PerceivableStimulis.Add(stimuli);
                    Debug.Log($"I sensed{stimuli.gameObject}");
                }
            }
            else{
                if(PerceivableStimulis.Contains(stimuli)){
                    PerceivableStimulis.Remove(stimuli); // The object lost sense
                    Debug.Log($"I lost sense{stimuli.gameObject}");
                }
            }
        }
    }
    protected virtual void DrawDebug()
    {

    }
    private void OnDrawGizmos()
    {
        DrawDebug();
    }
}
