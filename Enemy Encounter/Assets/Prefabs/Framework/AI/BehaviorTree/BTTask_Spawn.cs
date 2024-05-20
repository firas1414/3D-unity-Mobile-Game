using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// THIS CLASS IS A LEAF NODE, 
public class BTTask_Spawn : BTNode
{
    SpawnComponent spawnComponent;
    public BTTask_Spawn(BehaviorTree tree)
    {
        spawnComponent = tree.GetComponent<SpawnComponent>();
    }
    protected override NodeResult Execute()
    {
        if(spawnComponent == null || !spawnComponent.StartSpawn()) // THIS PROBABLY WILL NEVER HAPPEN, BUT WERE CHECKING JUST IN CASE, WE HERE ACTIVATED THE START SPAWN FUNCTION
        {
            return NodeResult.Failure;
        }

        return NodeResult.Success;
    }
}
