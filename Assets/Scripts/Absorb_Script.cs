using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb_Script : MonoBehaviour
{

	public GameObject background;
	public ParticleSystem trail;
	//public GameObject gameController;
		
	void OnCollisionEnter (Collision col)
	{
		if (this.gameObject.tag == "Enemy" && col.gameObject.tag == "Player") {
			Consume (col);
		}
		else if (this.gameObject.tag == "Player"){
			Consume (col);
		}
	}

	IEnumerator sizeInc (Transform source, float bigger, float smaller)
	{
		//minimum of 1, maximum of 2. Usually falls between 1.2 and 1.5
		float deltaScale = (smaller/bigger) + 1;
		float rateOfChange = .1f;
		Vector3 target = source.localScale + new Vector3 (deltaScale, deltaScale, deltaScale);

		//This information is only used if the player grows, so it's in an if statement to prevent enemies from trying to access a null gameobject
		Vector3 backgroundTarget = Vector3.zero;
		if (this.gameObject.tag == "Player") {
			backgroundTarget = background.transform.localScale + new Vector3 (deltaScale + 0.5f, deltaScale + 0.5f, deltaScale + 0.5f);
			this.gameObject.GetComponent<Player_Controller> ().isScaling = true;

			//maybe replace this with some math later
			if(deltaScale < 1.1f)
				this.gameObject.GetComponent<Player_Controller> ().updateEnergy (-20f); //using negative values because energy formula is energy -= updateEnergy(value).
			else if(deltaScale < 1.2f)
				this.gameObject.GetComponent<Player_Controller> ().updateEnergy (-40f);
			else if(deltaScale < 1.3f)
				this.gameObject.GetComponent<Player_Controller> ().updateEnergy (-60f);
			else if(deltaScale < 1.4f)
				this.gameObject.GetComponent<Player_Controller> ().updateEnergy (-80f);
			else
				this.gameObject.GetComponent<Player_Controller> ().updateEnergy (-100f);


		}
		
		//this.gameObject.GetComponent<Light> ().range += deltaScale*2;
		this.gameObject.GetComponent<Light> ().range *= this.gameObject.transform.localScale.x*2;
		//print (this.gameObject.transform.localScale.x);

		while (source.localScale.x < target.x) {
			source.localScale = Vector3.Lerp (source.localScale, target, rateOfChange);

			//Only scales background when the player grows
			if (this.gameObject.tag == "Player") {
				
				background.transform.localScale = Vector3.Lerp (background.transform.localScale, backgroundTarget, rateOfChange);
				Game_Controller.instance.planeSize = backgroundTarget;



			}


			this.gameObject.GetComponent<Light> ().range = this.gameObject.transform.localScale.x*2;

			if (this.gameObject.tag == "Player") {
				this.gameObject.GetComponent<Player_Controller> ().isScaling = false;
				ParticleSystem.ShapeModule trailShape = trail.shape; 
				trailShape.radius = (this.gameObject.transform.localScale.x + 1)/27f;
				if (trailShape.radius > 0.68f)
					trailShape.radius = 0.68f;

			}

			//print (this.gameObject.transform.localScale.x);
			yield return null;
		}
	}

	void Consume(Collision col){
		if (col.gameObject.tag == "Bounds" && this.gameObject.tag == "Player")
			print ("Hit bounds");

		if (col.gameObject.tag != "Bounds") {
			float thisSize = this.gameObject.GetComponent<Entity> ().size;
			float colSize = col.gameObject.GetComponent<Entity> ().size;

			if (thisSize > colSize) {
				col.gameObject.SetActive (false);
				col.gameObject.GetComponent<Entity> ().removeSelfFromList ();
				StartCoroutine (this.sizeInc (this.transform, thisSize, colSize));


				//plays the consume sound only if it was the player that got bigger
				if (this.gameObject.tag == "Player") {
					this.gameObject.GetComponent<Player_Controller> ().audio.Play ();
				} else {
					if(Game_Controller.instance.incTimer)
						Game_Controller.instance.GameOver ();
				}



			} else {
				//What do if same size?
			}
		}
	}
}
