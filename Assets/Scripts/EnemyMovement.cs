using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	public Transform player;
	public float speed = 3.0f;
	public float hitRange = 4.0f;
	public float range = 4.0f;
	Vector3 direction;
	bool hunting;
	// Use this for initialization
	void Start () {
		hunting = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		Kill ();

	}

	void Kill() {
		if (Input.GetMouseButton(1)) {
			Vector3 hit;
			hit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			hit.z = transform.position.z;
			// We found him
			if (Vector3.Distance(hit, transform.position) < hitRange && Vector3.Distance(transform.position, player.position) < range)
			{
				hunting = true;
			}
			else if (Vector3.Distance(transform.position, player.position) > range) {
				hunting = false;
			}
		}

		if (hunting) {
			//Find player
			direction = player.position;
			
			//Kill him
			transform.position = Vector3.MoveTowards (transform.position, direction, speed * Time.deltaTime);
		}
		else {
			direction = transform.position;
		}
	}
}
