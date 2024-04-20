using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Compositor : BTNode
{
    //list of the tasks(nodes)
    LinkedList<BTNode> children = new LinkedList<BTNode>();
    //represent one task (item of the list)
    public LinkedListNode<BTNode> currentChild = null;

    // Add the childrens
    public void AddChild (BTNode newChild){
        children.AddLast(newChild);
    }
    protected override NodeResult Execute(){
        //No multiTask
        if(children.Count == 0){
            return NodeResult.Success;
        }

        currentChild = children.First; // Always start with the first children
        return NodeResult.InProgress ;
    }

    protected BTNode GetCurrentChild(){
        return currentChild.Value;
    }

    //Switch between Tasks
    protected bool Next(){
        
        if(currentChild != children.Last){
            currentChild= currentChild.Next;
            return true;
        }
        return false;
    }

    protected override void End(){
        if(currentChild == null)
        {
            return;
        }
        currentChild.Value.Abort();
        currentChild = null;
    }

    public override void SortPriority(ref int priorityCounter)
	{
		base.SortPriority(ref priorityCounter); // Assign priority to the Compositor
        foreach(var child in children) // // Assign priority to the Compositor's children
        {
            child.SortPriority(ref priorityCounter);
        }
		
	}


    public override BTNode Get()
    {
        if(currentChild == null)
        {
            if(children.Count != 0)
            {
                return children.First.Value.Get();
            }
            else
            {
                return this;
            }
        }
        return currentChild.Value.Get();
    }
}
