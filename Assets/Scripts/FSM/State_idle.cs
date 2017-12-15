using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_idle : State<AI> {
	private static State_idle instance;


	private State_idle()
	{
		if(instance != null)
			return;

		instance = this;
	}

	public static State_idle Instance
	{
		get 
		{
			if (instance == null) {
				new State_idle ();
			}
			return instance;
		}

	}

	//Do upon entry of state
	//Meat of the code should go in EnterState for now
	public override void EnterState(AI owner)
	{
		owner.stateTimer = Time.time;
		owner.seconds = 0;
	}

	//Do before exiting state
	public override void ExitState(AI owner)
	{

	}

	//Update to a new state
	public override void UpdateState(AI owner)
	{
		//After 5 seconds AI will switch to wandering state
		if (Time.time > owner.stateTimer + 1) {
			owner.stateTimer = Time.time;
			owner.seconds++;
		}

		if (owner.prey != null) {
			owner.stateMachine.ChangeState (State_chase.Instance);
		}
		if (owner.enemy != null) {
			owner.stateMachine.ChangeState (State_escape.Instance);
		} 
		if(owner.seconds == 5){
			owner.stateMachine.ChangeState (State_wander.Instance);
		}

	}



		
		
}
