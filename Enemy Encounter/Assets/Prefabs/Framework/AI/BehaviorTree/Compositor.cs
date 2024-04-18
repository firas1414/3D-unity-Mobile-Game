using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Compositor : BTNode
{
    //list of the tasks(nodes)
    LinkedList<BTNode> children = new LinkedList<BTNode>();
    //represent one task (item of the list)
    LinkedListNode<BTNode> currentChild = null;

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
        return currentChild.Value ;
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
        //if(currentChild == null)
        {
            return;
        }
        currentChild.Value.Abort();
        //currentChild=null;
    }

    public override void SortPriority(ref int priorityCounter)
	{
		base.SortPriority(ref priorityCounter);
        foreach(var child in children)
        {
            child.SortPriority(ref priorityCounter);
        }
		
	}
}
