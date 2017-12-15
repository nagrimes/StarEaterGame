using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_escape : State<AI> {
	private static State_escape instance;

	private State_escape()
	{
		if (instance != null) {
			return;
		}

		instance = this;
	}

	public static State_escape Instance
	{
		get 
		{
			if (instance == null) {
				instance = new State_escape ();
			}
			return instance;
		}

	}
	//Do upon entry of state
	//Meat of the code should go in EnterState for now
	public override void EnterState(AI owner)
	{
		
	}

	//Do before exiting state
	public override void ExitState(AI owner)
	{

	}

	//Update to a new state
	public override void UpdateState(AI owner)
	{
		
		if (owner.enemy == null) {
			owner.stateMachine.ChangeState (State_idle.Instance);
		}
		if (owner.enemy != null) {
			escape(owner);
		}

	}

	public void escape(AI owner){
		float timeToTarget = .5f;
		float maxSpeed = 15.75f * (1 / owner.transform.localScale.x);
		Rigidbody rb = owner.GetComponent<Rigidbody> ();
		Vector3 offSet = owner.enemy.position - owner.transform.position;
		Vector3 target = owner.transform.position - offSet;
		Vector3 towards =  target - owner.transform.position;

		towards /= timeToTarget;
		if (towards.magnitude > maxSpeed) {
			towards.Normalize ();
			towards *= maxSpeed;
		}

			rb.velocity = towards;
		
	}

}
