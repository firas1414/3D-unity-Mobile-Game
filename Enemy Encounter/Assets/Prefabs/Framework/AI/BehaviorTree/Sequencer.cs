using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Go from right to left will be successful if every child is executed successfully (one node fails the entire sequence fails)
public class Sequencer : Compositor
{
    protected override NodeResult Update() // This will return not the state of the node, but the state of the whole Sequencer
    {
        NodeResult result = GetCurrentChild().UpdateNode();
        if (result == NodeResult.Failure)
        {
            return NodeResult.Failure; // If any child fails, the sequence fails
        }
        if (result == NodeResult.Success)
        {
            if (Next())
            {
                return NodeResult.InProgress; // If there are more children, continue with the sequence
            }
            else
            {
                return NodeResult.Success; // If all children succeed, the sequence succeeds
            }
        }

        // If the child is still in progress, the sequence is still in progress
        return NodeResult.InProgress;
    }

}


//BT is for logic what to do 
//We need something to store the knowldge (what i know?) (to drive the behavior of the AI character)
// based on what i know i do
