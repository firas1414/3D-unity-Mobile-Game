using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    BTNode Root; // Our behavior tree have a root node
    Blackboard blackboard = new Blackboard(); // Our behavior tree have a blackboard


    public Blackboard Blackboard // Of course our behavior tree also has to let us have access to the blackboard at any time we want, that's the purpose of this method'
    {
        get { return blackboard; }
    }

    // Start is called before the first frame update
    void Start()
    {
        ConstructTree(out Root); // Building the behavior tree
        SortTree();
    }

    private void SortTree()
    {
        int priorityCounter = 0;
        Root.SortPriority(ref priorityCounter);
    }
    
    protected abstract void ConstructTree(out BTNode rootNode);
    // Update is called once per frame
    void Update()
    {
        Root.UpdateNode(); // This means that the node will be always updated
        /*
        what's going to be updated exactly is the state of each node in the behavior tree.
        This could involve evaluating conditions, executing actions, or transitioning between nodes based on the current state of the game or the AI.
        */
    }
}
