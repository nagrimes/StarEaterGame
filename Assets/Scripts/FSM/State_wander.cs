using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_wander : State<AI> {
	private static State_wander instance;

	private State_wander()
	{
		if(instance != null)
			return;

		instance = this;
	}

	public static State_wander Instance
	{
		get 
		{
			if (instance == null) {
				new State_wander ();
			}
			return instance;
		}

	}

	//Do upon entry of state
	//Meat of the code should go in EnterState for now
	public override void EnterState(AI owner)
	{
		
		owner.setWanderTo (owner);

	}

	//Do before exiting state
	public override void ExitState(AI owner)
	{

	}

	//Update to a new state
	public override void UpdateState(AI owner)
	{
		//Debug.Log (owner.wanderTo);
		if (owner.prey != null) {
			owner.stateMachine.ChangeState (State_chase.Instance);
		}

		if (owner.enemy != null) {
			owner.stateMachine.ChangeState (State_escape.Instance);
		}

		if ((owner.wanderTo - owner.transform.position).magnitude < 8 ) {
			owner.setWanderTo (owner);
		}
		wander (owner);

	}

	//Movement
	public void wander(AI owner){
		float timeToTarget = 3f;
		float maxSpeed = 5f * (1 / owner.transform.localScale.x);
		Rigidbody rb = owner.GetComponent<Rigidbody> ();
		Vector3 towards =  owner.wanderTo - owner.transform.position;

		towards /= timeToTarget;
		if (towards.magnitude > maxSpeed) {
			towards.Normalize ();
			towards *= maxSpeed;
		}
	
			rb.velocity = towards;
		
	}
		

}