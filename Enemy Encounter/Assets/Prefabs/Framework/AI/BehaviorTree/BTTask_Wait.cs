using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_Wait : BTNode {
    float WaitTime = 2f;
    float timeElapsed = 0f;
    
    // Constructor
    public BTTask_Wait(float waitTime) {
        this.WaitTime = waitTime;
    }
    
    protected override NodeResult Execute() {
        if (WaitTime <= 0) {
            return NodeResult.Success;
        }
        Debug.Log($"Wait started with duration: {WaitTime}");
        timeElapsed = 0f;
        return NodeResult.InProgress;
    }

    protected override NodeResult Update() {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= WaitTime) {
            //Debug.Log($"Wait time finished for {timeElapsed}");
            return NodeResult.Success;
        }
        return NodeResult.InProgress;
    }
}
