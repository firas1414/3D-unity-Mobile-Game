using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator : BTNode
{
    BTNode child; // This is the node that we're applying a decorator to

    protected BTNode GetChild()
    {
        return child;
    }

    public Decorator(BTNode child)
    {
        this.child = child;
    }

    public override void SortPriority(ref int priorityConter)
    {
        base.SortPriority(ref priorityConter);
        child.SortPriority(ref priorityConter);
    }

    public override void Initialize()
    {
        base.Initialize();
        child.Initialize();
    }
    public override BTNode Get()
    {
        return child.Get();
    }
}
