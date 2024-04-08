using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Check the status of the task (done/failed/in progress)(timer)
public enum NodeResult {
    Success,
    Failure,
    InProgress
}

public abstract class BTNode {
    // Update the node 
    public NodeResult UpdateNode() {
        // One-off thing task (just one step)
        if (!started) {
            started = true;
            NodeResult execResult = Execute();
            if (execResult != NodeResult.InProgress) {
                EndNode();
                return execResult;
            }
        }
        // Time-based tasks (multi-steps or based on conditions)
        return Update();
    }

    // Functions to override in child classes
    // Execute the one thing task
    protected virtual NodeResult Execute() {
        return NodeResult.Success;
    }

    protected virtual NodeResult Update() {
        // Time-based tasks
        return NodeResult.Success;
    }
    
    protected virtual void End(){
        //clean up (reset)
    }

    protected virtual void EndNode() {
        started = false;
        End();
    }

    private bool started = false;
}
