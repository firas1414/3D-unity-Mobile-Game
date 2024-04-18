using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardDecorator : Decorator // This will be reacting to blacboard changes, and do someting about the child


{   // The condition that need to met in order for the task to start running
	public enum RunCondition 
	{
		KeyExists,
		KeyNotExists
	}


	// This is designed to notify whenever the RunCondition changes while the task is already running
	public enum NotifyRule 
	{
		RunConditionChange, // Means AI should always monitor the abscence or existence of a certain value
		KeyValueChange // Means AI should always monitor the changes made to key value


	// This is designed to tell the AI what to do after being Notified about the change(it's time tointerupt something)')
	public enum NotifyAbort
	{
		none, // Mean the AI won't interrupt any task
		self, // Abort self node(child node)->In case the enemy is sensing a target and then immidiately is loses track
		lower, // Abort lower child on the right side->When RunCondition changes from KeyExists to KeyNotExists
		both
	}

	BehaviorTree tree;
	string key;
	object value;

	RunCondition runCondition;
	NotifyRule notifyRule;
	NotifyAbort notifyAbort;


	public BlackboardDecorator(BTNode child, BehaviorTree tree, string key, RunCondition runCondition, NotifyRule notifyRule, NotifyAbort notifyAbort) : base(child)
	{
		this.tree = tree;
		this.key = key;
		this.runCondition = runCondition;
		this.notifyRule = notifyRule;
		this.notifyAbort = notifyAbort;
	}

	protected override NodeResult Execute()
	{
		Blackboard blackboard = tree.Blackboard;
		if(blackboard == null)
		{
			return NodeResult.Failure;
		}
	    blackboard.onBlackboardValueChanged -= CheckNotify; // This in case it's subscribed, and we're gonna subscribre again, so this is a solution to not fell in the erreur
	    blackboard.onBlackboardValueChanged += CheckNotify;
	    if(CheckRunCondition()) // Check if the conditon is met or not
		{
			return NodeResult.InProgress;
		}
		else
		{
			return NodeResult.Failure;
		}
	}

	private bool CheckRunCondition() // Return true if the condition is met, false if not met
	{
		bool exists = tree.Blackboard.GetBlackboardData(key, out value); // true if key exists, false if key doesen't exists
		if(runCondition == RunCondition.KeyExists)
		{
			return exists;
		}
		else if(runCondition == RunCondition.KeyNotExists)
		{
			return !exists;
		}
		return false;
	}

	private void CheckNotify(string key, object val)
	{
		if(this.key != key) // The change didn't happen to the key that concerns us
		{
			return;
		}
		if(notifyRule == NotifyRule.RunConditionChange)
		{
			bool prevExists = value != null;
			bool currentExists = val != null;
			if(prevExists != currentExists) // Value changed
			{
				Notify();
			}
		}
		else if(notifyRule == NotifyRule.KeyValueChange)
		{
			if(value != val) // Value changed
			{
				Notify();
			}
		}
	}

	private void Notify()
	{
		if(notifyAbort == none)
		{

		}
		else if(notifyAbort == self)
		{
			AbortSelf();
		}
		else if(notifyAbort == lower)
		{
			AbortLower();
		}
		else if(notifyAbort == both)
		{
			AbortBoth();
		}
	}

	private void AbortBoth()
	{
		Abort();
		AbortLower();
	}

	private void AbortLower()
	{
	}

	private void AbortSelf()
	{
		Abort();
	}

	protected override NodeResult Update()
	{
		return GetChild().UpdateNode();
	}

	protected override void End()
	{
		GetChild().Abort(); 
		base.End();
	}

}
