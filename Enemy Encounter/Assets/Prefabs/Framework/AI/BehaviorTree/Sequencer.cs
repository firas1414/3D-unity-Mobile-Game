using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// go from right to left will be succsessful if every child is executed successfully (one node fails the entire sequence fails)
public class Sequencer : Compositor
{
     protected override NodeResult Update(){
        NodeResult result = GetCuurentChild().UpdateNode();

        if(result == NodeResult.Failure){
            return NodeResult.Failure;
        }
        if(result == NodeResult.Success){
            if(Next()){return NodeResult.InProgress;}
            else{return NodeResult.Success;}

        }
        //Inprogress
        return NodeResult.InProgress;
     }
 
}
