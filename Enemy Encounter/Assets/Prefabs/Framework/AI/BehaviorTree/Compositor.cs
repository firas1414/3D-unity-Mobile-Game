using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Compositor : BTNode
{
    //list of the tasks(nodes)
    LinkedList<BTNode> children = new LinkedList<BTNode>();
    //represent one task (item of the list)
    LinkedListNode<BTNode> currentChild = null;

    protected override NodeResult Execute(){
        //No multiTask
        if(children.Count == 0){
            return NodeResult.Success;
        }

        currentChild = children.First;
        return NodeResult.InProgress ;
    }

    protected BTNode GetCuurentChild(){
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
        currentChild=null;
    }
}
