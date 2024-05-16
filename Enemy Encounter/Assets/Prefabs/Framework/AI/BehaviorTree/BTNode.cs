using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A node representes a specific action

public enum NodeResult
{
    Success,
    Failure,
    Inprogress
}

public abstract class BTNode
{
    public NodeResult UpdateNode() // This function is executed always(non stop)
    {
        //one off thing
        if(!started) // Which means that the section below will be only executed one time, and that is only the first time.
        {
            started = true; // This will let the AI know that the node already started
            NodeResult execResult = Execute();
            if(execResult != NodeResult.Inprogress)
            {
                EndNode();
                return execResult;
            }
        }

        //time based
        NodeResult updateResult = Update();
        if(updateResult != NodeResult.Inprogress)
        {
            EndNode();
        }
        return updateResult;
    }

    //override in child class
    protected virtual NodeResult Execute() // Gonna be overriden
    {
        //one off thing
        return NodeResult.Success;
    }

    protected virtual NodeResult Update() // Gonna be overriden
    {
        //time based
        return NodeResult.Success;
    }

    protected virtual void End() // Gonna be overriden
    {
        //reset and clean up
    }

    private void EndNode()
    {
        started = false;
        End();
    }

    public void Abort()
    {
        EndNode();
    }

    bool started = false;
    int priority;

    public int GetPriority()
    {
        return priority;
    }

    public virtual void SortPriority(ref int priorityConter) // Gonna be overriden
    {
        priority = priorityConter++;
        Debug.Log($"{this} has priorty {priority}");
    }

    public virtual void Initialize(){} // Gonna be overriden

    public virtual BTNode Get() // Gonna be overriden
    {
        return this;
    }
}
