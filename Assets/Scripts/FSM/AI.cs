using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class AI : MonoBehaviour {
	[HideInInspector] public Transform enemy;
	[HideInInspector] public Transform prey;
	[HideInInspector] public Vector3 wanderTo;
	[HideInInspector] public float stateTimer;
	[HideInInspector] public int seconds = 0;
	[HideInInspector] public float alertRadius;
	[HideInInspector] public float chaseRadius;
	[HideInInspector] public MonoBehaviour myMono;
	public float alertScale = 3;
	public float chaseScale = 4;
	public GameObject plane;
	//public GameObject gameController;
	public StateMachine<AI> stateMachine { get; set; }
	//private const int layerMask = ~(1 << 7)

	private void Start()
	{
		stateMachine = new StateMachine<AI> (this);
		stateMachine.ChangeState (State_idle.Instance);
		plane = GameObject.Find("Plane");
		//gameController = GameObject.Find ("GameController");
		alertRadius = this.transform.localScale.x * 2f + alertScale;
		chaseRadius = this.transform.localScale.x * 2f + chaseScale;
	}
	private void Update()
	{
		enemy = alertSphere (this.transform.position, alertRadius);
		prey = chaseSphere (this.transform.position, chaseRadius);
		stateMachine.Update();
		//Debug.Log (stateMachine.currentState);
	
	}


	/// <summary>
	/// Gets all colliders within a certain radius of itself.
	/// </summary>
	/// <returns>Largest objects transform or null if all other objects are smaller than its scale.</returns>
	/// <param name="center">Center of alert sphere.</param>
	/// <param name="radius">Radius or how lard the alert sphere is.</param>
	public Transform alertSphere(Vector3 center, float radius) {
		Transform largest = this.transform;
		Collider[] inAlterSphere = Physics.OverlapSphere (center, radius);
		if (inAlterSphere.Length > 0) {
			foreach(Collider obj in inAlterSphere) {
				if (obj.gameObject != null && !obj.CompareTag("Bounds")) {
					//Debug.Log (obj + " " + largest.GetComponent<Entity> ().gameObject);
					if (obj.GetComponent<Entity> ().size > largest.GetComponent<Entity> ().size) {
						largest = obj.transform;
					}
				}
			}

			//If the largest thing is itself return null
			if (largest == this.transform) {
				return null;
			} else {
				//Debug.Log (largest.GetComponent<Entity> ().gameObject);
				return largest.transform;
			}
		}
		return null;
	}

	public Transform chaseSphere(Vector3 center, float radius) {
		Collider[] inAlterSphere = Physics.OverlapSphere (center, radius);
		Transform localSize = this.transform;

		if (inAlterSphere.Length > 0) {
			foreach(Collider obj in inAlterSphere) {
				if (obj.gameObject != null && !obj.CompareTag("Bounds")) {
					if (obj.CompareTag("Player") && localSize.GetComponent<Entity>().size > obj.GetComponent<Entity>().size) {
						//Debug.Log (obj.transform);
						return obj.transform;
					}
				}
			}
		}
		return null;
	}
	/*
	public void moveTowards(AI owner, float timeToTarget, float maxSpeed, Vector3 target){
		Rigidbody rb = owner.GetComponent<Rigidbody> ();
		Vector3 towards =  target - owner.transform.position;

		towards /= timeToTarget;
		if (towards.magnitude > maxSpeed) {
			towards.Normalize ();
			towards *= maxSpeed;
		}
		rb.velocity = towards;
	}
	*/

	public void setWanderTo(AI owner){
		//Plane Size
		float xbound = (owner.plane.transform.localScale.x - 4) * 10; //Hardcoded using z bc defalut plane is in XZ, but ours is rotated to be in XY
		float ybound = (owner.plane.transform.localScale.z - 4) * 10; //Hardcoded using z bc defalut plane is in XZ, but ours is rotated to be in XY
		Vector3 center = owner.plane.transform.position;

		float xPos = center.x + Random.Range (-xbound/2, xbound/2);
		float yPos = center.y + Random.Range (-ybound/2, ybound/2);

		wanderTo = new Vector3 (xPos, yPos, center.z);
	}

}
