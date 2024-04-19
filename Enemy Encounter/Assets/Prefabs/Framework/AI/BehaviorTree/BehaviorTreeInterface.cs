using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BehaviorTreeInterface
{
	public void RotationTowards(GameObject target, bool verticalAim = false);
	
}
