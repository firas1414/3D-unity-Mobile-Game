using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingComponent : MonoBehaviour
{
	[SerializeField] Transform[] patrolPoints;
	int currentPatrolPontIndex = -1;

	public bool GetNextPatrolPoint(out Vector3 point)
	{
		point = Vector3.zero;
		if(patrolPoints.Length == 0)
		{
			return false;
		}
		currentPatrolPontIndex = (currentPatrolPontIndex+1) % patrolPoints.Length;
		point = patrolPoints[currentPatrolPontIndex].position;
		return true;
	}
}
