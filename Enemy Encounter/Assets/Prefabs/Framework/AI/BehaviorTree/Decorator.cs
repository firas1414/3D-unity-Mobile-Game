using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator : BTNode
{
	BTNode child; // This is the node that we will running conditions on him, and any changed will be made to this child
	protected BTNode GetChild()
	{
		return child; 
	}
	 public Decorator(BTNode child)
	 {
		 this.child = child;
	 }

	public override void SortPriority(ref int priorityCounter)
	{
		base.SortPriority(ref priorityCounter);
		child.SortPriority(ref priorityCounter);
	}
}
