using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    [SerializeField] SenseComp[] senses; // STORES ALL THE SENSES
    [Header("Audio")]
    [SerializeField] AudioClip DetectionAudio;
    [SerializeField] float volume = 1f;
    LinkedList<PerceptionStimuli> currentlyPerceivedStimulis = new LinkedList<PerceptionStimuli>(); // STORES ALL THE CURRENT PERCEIVABLE STIMULI

    PerceptionStimuli targetStimuli; // REPRESENTES THE FUTURE PERCEIVALE TARGET(in our case, the player)

    public delegate void OnPerceptionTagetChanged(GameObject target, bool sensed);

    public event OnPerceptionTagetChanged onPerceptionTargetChanged;

    private void Awake() // THIS EXECUTED AUTOMATICALLY ONLY ONCE, WHEN THE GAME STARTS, its almost like the Start function
    {
        foreach (SenseComp sense in senses)
        {
            sense.onPerceptionUpdated += SenseUpdated;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void SenseUpdated(PerceptionStimuli stimuli, bool succsessfulySensed) // THIS METHOD IS CALLED EACH TIME A CERTAIN SENSE GETS UPDATED
    {
        var nodeFound = currentlyPerceivedStimulis.Find(stimuli);
        if (succsessfulySensed)
        {
            if (nodeFound != null)
            {
                currentlyPerceivedStimulis.AddAfter(nodeFound, stimuli); // MAKES SURE TO ADD THAT NODE EXACTLY NEXT TO THE STIMULI(for example if there is a plyer 1, add player1 next to player1 and not next to player 2)
            }
            else
            {
                currentlyPerceivedStimulis.AddLast(stimuli);
            }
        }
        else
        {
            currentlyPerceivedStimulis.Remove(nodeFound);
        }

        if (currentlyPerceivedStimulis.Count != 0)
        {
            PerceptionStimuli highestStimuli = currentlyPerceivedStimulis.First.Value;
            if (targetStimuli == null || targetStimuli!=highestStimuli)
            {
                targetStimuli = highestStimuli;
                onPerceptionTargetChanged?.Invoke(targetStimuli.gameObject, true);
                Vector3 audioPos = transform.position;
                GameplayStatics.PlayAudioAtLoc(DetectionAudio,audioPos, volume);
            }
        }
        else
        {
            if(targetStimuli!=null)
            {

                onPerceptionTargetChanged?.Invoke(targetStimuli.gameObject, false);
                targetStimuli = null;
            }
        }
    }

    internal void AssignPercievedStimui(PerceptionStimuli targetStimuli)
    {
        if(senses.Length != 0) // THE ENEMY HAS AT LEAST ONE SENSE
        {
            senses[0].AssignPerceivedStimuli(targetStimuli);
        }
    }
}
