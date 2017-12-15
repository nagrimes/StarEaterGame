using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Controller : MonoBehaviour
{
	/*
		Energy 10% max deter per sec.
		Consume increases relative to size of enemy.
		

	*/

	public float minSpeed, maxSpeed, boostSpeed;
	public float speed;
	private Rigidbody rb;
	private Transform tr;
	[HideInInspector]public AudioSource audio;
	private float oldSize;
	public float energy;
	public float boostEnergyMultiplier;
	public float maxEnergy;
	private bool boosting;
	public float energyLostPerSec;
	public ParticleSystem trail;
	public Color defaultColor = new Color(1f, 1f, .66f);
	public Color boostColor = new Color(1f, .5f, 0f);
	public Color energyBarColor = new Color(1f, .5f, 0f);
	public Color energyBarBoostColor = new Color(1f, 0f, 0f);
	private const float FRAME_RATE = 60f;
	[HideInInspector]public bool isScaling;
	public ParticleSystem deathParticlePrefab;


	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		tr = GetComponent<Transform> ();
		audio = GetComponent<AudioSource> ();
		oldSize = 0.0f;
		boosting = false;
		energy = maxEnergy;
		isScaling = false;

	}
	/// <summary>
	/// Updates the energy. Pass it a value for the percentage to update it by, positive or negative.
	/// </summary>
	/// <param name="change">Change.</param>

	public void updateEnergy(float change){

		energy -= change;
		energy = Mathf.Clamp(energy, 0, maxEnergy);
		Game_Controller.instance.UpdateEnergyUI (energy);
		//print (energy);
	}


	void FixedUpdate ()
	{

		if (energy < 1) {
			this.gameObject.SetActive (false);

			//this.transform.localRotation = Quaternion.Euler (0,0,0);

			//Color c = GetComponent<Renderer> ().material.color;
			//GetComponent<Renderer> ().material.color = new Color (c.r, c.b, c.g, c.a - (2.5f - Time.deltaTime));


			ParticleSystem deathParticles = Instantiate (deathParticlePrefab, new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity) as ParticleSystem;
			//ParticleSystem.MainModule deathMain
			Game_Controller.instance.GameOver ();
		}
		switch((int)transform.localScale.x){
		case 1:
			Game_Controller.instance.UpdateMilestoneUI (0);
			break;
		case 4:
			Game_Controller.instance.UpdateMilestoneUI (1);
			break;
		case 8:
			Game_Controller.instance.UpdateMilestoneUI (2);
			break;
		case 12:
			Game_Controller.instance.UpdateMilestoneUI (3);
			break;
		case 16:
			Game_Controller.instance.UpdateMilestoneUI (4);
			Game_Controller.instance.Win ();
			break;
		}
		
		float moveHor = Input.GetAxis ("Horizontal");
		float moveVer = Input.GetAxis ("Vertical");


		Vector3 movement = new Vector3 (moveHor, moveVer, 0.0f);




		Game_Controller.instance.ChangeEnergyBarColor (energyBarColor);
		speed = (70 - GetComponent<Entity> ().size);
		if (speed > maxSpeed)
			speed = maxSpeed;
		else if (speed < minSpeed)
			speed = minSpeed;
		if (!Game_Controller.instance.mainMenuActive && Game_Controller.instance.incTimer)
			updateEnergy (energyLostPerSec / FRAME_RATE); //Read this as energyLost per second, system is set to 60fps, do not alter the FRAME_RATE value. Adjust energyLostPerSec in the inspector.


		if(boosting){
			speed += boostSpeed;
			Game_Controller.instance.ChangeEnergyBarColor (energyBarBoostColor);
			updateEnergy (energyLostPerSec*boostEnergyMultiplier / FRAME_RATE); //Read this as energyLost per second, system is set to 60fps, do not alter the FRAME_RATE value. Adjust energyLostPerSec in the inspector.
		}

		//print (tr.position.x);
		if (!Game_Controller.instance.mainMenuActive)
			tr.Translate (movement * speed / 400); 
		float boundsFactor = Game_Controller.instance.planeSize.x*10/2f*0.7f;
		if (tr.position.x < -1*boundsFactor || tr.position.x > boundsFactor || tr.position.y < -1*boundsFactor || tr.position.y > boundsFactor)
			tr.Translate(-1*movement * speed / 400);



	}

	void RunMovement(){




	}

	void Update ()
	{
		if (!Game_Controller.instance.mainMenuActive) {
			if (Input.GetKeyDown (KeyCode.Space)) {

				//print("boosting");
				boosting = true;
				ParticleSystem.MainModule trailMain = trail.main;
				trailMain.startColor = boostColor;
				ParticleSystem.EmissionModule emission = trail.emission;
				emission.rateOverTime = 500;


			}

			if (Input.GetKeyUp (KeyCode.Space)) {

				//print("stop boosting");
				boosting = false;
				ParticleSystem.MainModule trailMain = trail.main;
				trailMain.startColor = defaultColor;
				ParticleSystem.EmissionModule emission = trail.emission;
				emission.rateOverTime = 50;
			}
		}
			


	}



}
