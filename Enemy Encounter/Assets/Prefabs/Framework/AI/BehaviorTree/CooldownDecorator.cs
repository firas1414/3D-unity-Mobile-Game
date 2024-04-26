using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// THIS CLASS CHECKS IF A CERTAIN DURATION IS FINISHED OR NOT BEFORE MAKING THE SPITTER'S ATTACK TASK AGAIN, SO THIS IS A CONDITION APPLIED ON THE SPITTER'S ATTACK TASK
public class CooldownDecorator : Decorator
{
    float cooldownTime;
    float lastExecutionTime = -1;
    bool failOnCooldown;
    public CooldownDecorator(BehaviorTree tree, BTNode child, float cooldownTime, bool failOnCooldown = false) : base(child)
    {
        this.cooldownTime = cooldownTime;
        this.failOnCooldown = failOnCooldown;
    }

    protected override NodeResult Execute()
    {
        if (cooldownTime == 0) // IF THERE IS NOT COOLDOWN TIME WHICH MEANS THERE IS NO CONDITION TO EXECUTE WHICH MEANS EXECUTE THE NEXT TASK
            return NodeResult.Inprogress; // EXECUTE THE CHILD NODE

        if(lastExecutionTime == -1) // IF THIS IS THE FIRST TIME THE ATTACK IS EXECUTED
        {
            lastExecutionTime = Time.timeSinceLevelLoad;
            return NodeResult.Inprogress;
        }

        if(Time.timeSinceLevelLoad - lastExecutionTime < cooldownTime) // IF COOLDOWN IS NOT FINISHED
        {
            if(failOnCooldown)
            {
                return NodeResult.Failure;
            }
            else // IF FALSE, IT WILL SUCCEED BUT NOT EXECUTE UNTIL THE COOLDOWN ENDS WHICH MEANS IT WON'T EXECUTE THE CHILD NODE BUT IT WILL KEEP CHECKING EVERYTIME IT COMES ACROSS THAT TASK
            {
                return NodeResult.Success;
            }
        }
        // IF COOLDOWN IS FINISHED ----> PERMISSION TO MAKE THE ATTACK
        lastExecutionTime = Time.timeSinceLevelLoad; 
        return NodeResult.Inprogress; // EXECUTE THE CHILD NODE
    }

    protected override NodeResult Update() // EXECUTED IF THE CONDITION IS MET(COOLDOWN TIME FINISHED)
    {
        return GetChild().UpdateNode(); // EXECUTE THE CHILD NODE
    }
}
