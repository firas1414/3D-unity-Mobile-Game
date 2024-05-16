using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardDecorator : Decorator
{
    public enum RunCondition
    {
        KeyExists,
        keyNotExists
    }

    public enum NotifyRule
    {
        RunConditionChange,
        KeyValueChange
    }

    public enum NotifyAbort
    {
        none,
        self,
        lower,
        both
    }

    BehaviorTree tree;
    string key;
    object value;

    RunCondition runCondition;
    NotifyRule notifyRule;
    NotifyAbort notifyAbort;

    public BlackboardDecorator(BehaviorTree tree,
        BTNode child,
        string key,
        RunCondition runCondition,
        NotifyRule notifyRule,
        NotifyAbort notifyAbort) : base(child)
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
        if (blackboard == null) // This is just in case
            return NodeResult.Failure;

        blackboard.onBlackboardValueChange -= CheckNotify;
        blackboard.onBlackboardValueChange += CheckNotify;

        if (CheckRunCondition())
        {
            return NodeResult.Inprogress;
        }
        else
        {
            return NodeResult.Failure;
        }
    }


    // CHECK WETHER THE RunCondition IS MET OR NOT
    private bool CheckRunCondition()
    {
        bool exists = tree.Blackboard.GetBlackboardData(key, out value); // IF KEY EXISTS -> FALSE, OTHERWISE TRUE
        switch(runCondition)
        {
            case RunCondition.KeyExists: // THE exist variable already have the bool for the key existance soo if all we have to do is return exist
                return exists;
            case RunCondition.keyNotExists: // HERE, WE HAVE TO RETURN THE OPPOSITE OF THE exists RESULT
                return !exists;
        }

        return false;
    }

    private void CheckNotify(string key, object val) // THIS WILL BE EXECUTED EACH TIME A CHANGE HAPPENS TO THE BLACKBOARDDATA
    {
        if (this.key != key) return; // CHECK IF THE CHANGE MATTERS TO US OR NOT

        if(notifyRule == NotifyRule.RunConditionChange)
        {
            bool prevExists = value != null; // IF RETURN TRUE -> KEY EXISTS, IF FALSE -> KEY DOES NOT EXISTS
            bool currentExists = val != null; // IF RETURN TRUE -> KEY EXISTS, IF FALSE -> KEY DOES NOT EXISTS

            if(prevExists != currentExists) // IF THERE IS A DIFFERENCE IN THE EXSISTANCE STATE, NOTIFY()
            {
                Notify();
            }
        }
        else if(notifyRule == NotifyRule.KeyValueChange)
        {
            if(value != val)
            {
                Notify();
            }
        }
    }

    private void Notify()
    {
        switch (notifyAbort)
        {
            case NotifyAbort.none:
                break;
            case NotifyAbort.self:
                AbortSelf();
                break;
            case NotifyAbort.lower:
                AbortLower();
                break;
            case NotifyAbort.both:
                AbortBoth();
                break;
        }
    }

    private void AbortBoth()
    {
        Abort();
        AbortLower();
    }

    private void AbortLower()
    {
        tree.AbortLowerThan(GetPriority());
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
