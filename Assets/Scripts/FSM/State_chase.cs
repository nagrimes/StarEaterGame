using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_chase : State<AI> {
	private static State_chase instance;


	private State_chase()
	{
		if(instance != null)
			return;

		instance = this;
	}

	public static State_chase Instance
	{
		get 
		{
			if (instance == null) {
				new State_chase ();
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
		if (owner.prey == null) {
			owner.stateMachine.ChangeState (State_idle.Instance);
		}

		if (owner.prey != null) {
			chase (owner);
		}

	}
	public void chase(AI owner){
		float timeToTarget = .4f;
		float maxSpeed = 15.75f * (1 / owner.transform.localScale.x);
		Rigidbody rb = owner.GetComponent<Rigidbody> ();
		Vector3 towards =  owner.prey.position - owner.transform.position;

		towards /= timeToTarget;
		if (towards.magnitude > maxSpeed) {
			towards.Normalize ();
			towards *= maxSpeed;
		}
			
			rb.velocity = towards;

	}
}