using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Compositor
{
    protected override NodeResult Update()
    {
        NodeResult result = GetCuurentChild().UpdateNode();

        // We need just one child to be successful (task approved)
        if (result == NodeResult.Success)
        {
            return NodeResult.Success;
        }

        if (result == NodeResult.Failure)
        {
            // Check if we have more children
            if (Next())
            {
                return NodeResult.InProgress;
            }
            else
            {
                return NodeResult.Failure; // Return Failure if no more children
            }
        }

        // Default behavior if none of the above conditions are met
        return NodeResult.InProgress;
    }
}
