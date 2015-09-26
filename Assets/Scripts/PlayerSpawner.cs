using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;

	public float spawnInterval = 10f;

	private float lastTimeSpawned;

	// Use this for initialization
	void Start () {
		lastTimeSpawned = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.realtimeSinceStartup > lastTimeSpawned + spawnInterval) {
			GameObject playerUnit = GameObject.Instantiate(playerPrefab);
			this.lastTimeSpawned = Time.realtimeSinceStartup;
		}
	}
}
