using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Compositor : BTNode
{
    LinkedList<BTNode> children = new LinkedList<BTNode>(); // The compositor childrens
    LinkedListNode<BTNode> currentChild = null; // a reference to the current child node being processed.

    public void AddChild(BTNode newChild) // Add a child to the children list
    {
        children.AddLast(newChild);
    }

    protected override NodeResult Execute()
    {
        if(children.Count == 0) // This is in case we didnt add any child to the children list(which is not gonna happen)
        {
            return NodeResult.Success;
        }

        currentChild = children.First; // at the first execution, the currentchild will the first child in the children list
        return NodeResult.Inprogress;
    }

    protected BTNode GetCurrentChild()
    {
        return currentChild.Value;
    }

    protected bool Next()
    {
        if(currentChild != children.Last)
        {
            currentChild = currentChild.Next;
            return true;
        }
        return false;
    }

    protected override void End()
    {
        if (currentChild == null)
            return;

        currentChild.Value.Abort();
        currentChild = null;
    }

    public override void SortPriority(ref int priorityConter)
    {
        base.SortPriority(ref priorityConter);

        foreach(var child in children)
        {
            child.SortPriority(ref priorityConter);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        foreach (var child in children)
        {
            child.Initialize();
        }
    }

    public override BTNode Get() // THIS FUNCTION RETURN THE CURRENT-ACTIVE CHILD IN THE CHILDRENS LIST
    {
        if(currentChild == null)
        {
            if(children.Count!=0)
            {
                return children.First.Value.Get(); // WERE DOING A GET, BECAUSE THIS CHILD COULD ALSO BE A COMPOSITOR
            }
            else
            {
                return this;
            }
        }

        return currentChild.Value.Get();
    }
}
