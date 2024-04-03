using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    [SerializeField] SenseComponent[] senses;

    LinkedList<PerceptionStimuli> currentlyPerceivedStimulis = new LinkedList<PerceptionStimuli>();

    PerceptionStimuli targetStimuli;
    public delegate void OnTargetChanged(GameObject target, bool HaveTarget);
    public event OnTargetChanged onTargetChanged;

    // Start is called before the first frame update
    void Start()
    {
        foreach(SenseComponent sense in senses)
        {
            sense.onPerceptionUpdated += NewSenseDetected;
        }
    }


    public void NewSenseDetected(PerceptionStimuli stimuli, bool successfullySensed)
    {
      var nodeFound = currentlyPerceivedStimulis.Find(stimuli);
      if (successfullySensed)
        {
            if (nodeFound != null)
            {
                currentlyPerceivedStimulis.AddAfter(nodeFound, stimuli); // Add the stimuli next to the that node because it's the same stimuli
            }
            else
            {
                currentlyPerceivedStimulis.AddLast(stimuli);
            }
        }
       else // Lost track of that stimuli's sense
       {
           currentlyPerceivedStimulis.Remove(nodeFound);
       }
       if(currentlyPerceivedStimulis.Count != 0) // The enemy have track of at least one stimuli(player)
       {
           PerceptionStimuli highestStimuli = currentlyPerceivedStimulis.First.Value;
           if(targetStimuli == null || targetStimuli != highestStimuli)
           {                
               targetStimuli = highestStimuli; // Updated the enemy's target
               onTargetChanged?.Invoke(targetStimuli.gameObject, true);
           }
       }
       else // The enemy does not have track of any stimuli's
       {
           if(targetStimuli != null)
           {
               onTargetChanged?.Invoke(targetStimuli.gameObject, false);
               targetStimuli = null;
           }
       }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
