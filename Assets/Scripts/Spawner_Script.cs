using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Script : MonoBehaviour {

	[HideInInspector] public List<GameObject> enemyList;
	public int maxEnemies;
	public GameObject background;
	public float spawnRate;
	public float startDelay;
	public GameObject enemyPrefab;
	public GameObject[] bounds;
	GameObject enemyClone;
	GameObject trailClone;
	public GameObject player;
	private GameObject trail;
	public int minSmallerEnemies = 0;

	void Start () {
		
		InvokeRepeating ("spawnEnemy", startDelay, spawnRate);
		enemyList = new List<GameObject> ();


	}
	
	// Update is called once per frame
	void Update () {




	}
	void spawnEnemy(){
		float scale = 0f;
		if (enemyList.Count < maxEnemies && !Game_Controller.instance.mainMenuActive) {
			enemyClone = Instantiate (enemyPrefab, getRandomV3 (), Quaternion.identity) as GameObject;
			if (GetIsEnoughSmallerEnemyInList () == false) {
				scale = player.transform.localScale.x * .75f;
				//Debug.Log (scale);
			} else if (GetIsLargerEnemyInList () == false) {
				scale = player.transform.localScale.x * 3f;
				//Debug.Log (scale);
			}
			else{
				bool appropriateScale = false;
				while (appropriateScale == false) {
					scale = Random.Range (0.5f, 6.0f);
					//if the enemy is too close to the player's size, get new scale value. Size is appropriate if the scale is smaller than the player or larger than player's scale + 1.
					if (scale < player.transform.localScale.x || scale > player.transform.localScale.x + 0.75f) {
						appropriateScale = true;
					} else
						appropriateScale = false;
				}
			}
			enemyClone.transform.localScale = new Vector3 (scale, scale, scale);
			trail = enemyClone.gameObject.transform.GetChild (0).gameObject;
			ParticleSystem.ShapeModule trailShape = trail.GetComponent<ParticleSystem>().shape; 

			trailShape.radius = (enemyClone.gameObject.transform.localScale.x + 1)/27f;
			if (trailShape.radius > 0.68f)
				trailShape.radius = 0.68f;
			enemyList.Add (enemyClone);
		}

	}
	/// <summary>
	/// Returns true if a Vector3 is visible by the main camera.
	/// </summary>
	/// <returns><c>true</c>, if on screen was pointed, <c>false</c> otherwise.</returns>
	/// <param name="v">V.</param>
	bool pointOnScreen(Vector3 v){
		
		if (Camera.main.WorldToViewportPoint (v).x >= 0.0 && Camera.main.WorldToViewportPoint (v).x <= 1.0 && Camera.main.WorldToViewportPoint (v).y >= 0.0 && Camera.main.WorldToViewportPoint (v).y <= 1.0)
			return true;

		return false;
	
	}

	/// <summary>
	/// Returns a random V3 outside the view of the main camera.
	/// </summary>
	/// <returns>The random v3.</returns>
	Vector3 getRandomV3(){

		//Safe zone is the x and y coordinates of the space inside the background and bounds
		float safeZoneX = (background.gameObject.transform.localScale.x * 3.0f);
		float safeZoneY = (background.gameObject.transform.localScale.y * 3.0f);

		Vector3 rv = new Vector3 (Random.Range(-1*safeZoneX, safeZoneX), Random.Range(-1*safeZoneY, safeZoneY), 0);

		while (pointOnScreen(rv)) {
			rv = new Vector3 (Random.Range(-1*safeZoneX, safeZoneX), Random.Range(-1*safeZoneY, safeZoneY), 0);
		}

		return rv;
	}

	bool GetIsEnoughSmallerEnemyInList(){
		bool result = false;
		int count = 0;
		//search the list, if the player is larger than any enemyClone, then there is a smaller enemy in the list.
		foreach(GameObject enemyClone in enemyList){
			if (player.transform.localScale.x > enemyClone.transform.localScale.x){
				count++;
				if(count >= minSmallerEnemies){
					result = true;
				}
			}
		}
		return result;
	}

	bool GetIsLargerEnemyInList(){
		bool result = false;
		//search the list, if the player is larger than any enemyClone, then there is a smaller enemy in the list.
		foreach(GameObject enemyClone in enemyList){
			if (player.transform.localScale.x < enemyClone.transform.localScale.x)
				result = true;
		}
		return result;
	}
}