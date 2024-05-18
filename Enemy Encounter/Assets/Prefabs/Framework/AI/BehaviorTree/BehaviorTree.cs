using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    BTNode Root;
    Blackboard blackboard = new Blackboard();
    IBehaviorTreeInterface behaviorTreeInterface;
    public Blackboard Blackboard
    {
        get { return blackboard; }
    }

    private bool bRunBehaviorTree = true;

    internal IBehaviorTreeInterface GetBehaviorTreeInterface()
    {
        return behaviorTreeInterface;
    }

    // Start is called before the first frame update
    void Start()
    {
        behaviorTreeInterface = GetComponent<IBehaviorTreeInterface>();
        ConstructTree(out Root);
        SortTree();
    }


    private void SortTree()
    {
        int priortyCounter = 0;
        Root.Initialize();
        Root.SortPriority(ref priortyCounter);
        
    }

    protected abstract void ConstructTree(out BTNode rootNode); // The enemy behavior(for example ChomperBehavior) class will override this to build the enemy's behavior tree

    // Update is called once per frame
    void Update()
    {
        if(bRunBehaviorTree)
        {
            Root.UpdateNode();
        }
    }

    public void AbortLowerThan(int priority) //  ABORTS NODES THAT HAS LESS PRIORITY THAN THE GIVEN PRIORITY
    {
        BTNode currentNode = Root.Get();
        if(currentNode.GetPriority() > priority) // IF NOW WERE IN PATROLLING AND IF IT IS LESS IMPORTANT THAN priority NODE(MADE THE OPPOSITE BECAUSE LOWER PRIORITY MEANS HIGHER PRIORITY AND VISE VERSA)
        {
            Root.Abort(); // THEN ABORT EVERYTHING, WHICH MEANS RESTART THE TREE
        }
    }

    internal void StopLogic()
    {
        bRunBehaviorTree = false;
    }
}
