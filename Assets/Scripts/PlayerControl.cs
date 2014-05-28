using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public float speed = 1.0f;
	public HeathSystem HS;
	Animator anim;
	float startTime = Time.time;
	Vector3 direction;
	float playerY = 0.0f;
	float playerX = 0.0f;

	void Start() {
		anim = GetComponent<Animator> ();
		direction = transform.position;
		playerX = transform.position.x;
		playerY = transform.position.y;
	}


	void Update()
	{
		playerX = transform.position.x;
		playerY = transform.position.y;

		ClickToMove();
	}

	//Handle continuous-movement and back-and-forth glitch
	void OnCollisionStay2D(Collision2D col) {
		if (col.gameObject.tag == "Wall"){
			direction = transform.position;
		}
		else if (col.gameObject.tag == "Enemy" && Time.time % HS.hitDelay == 0) {
			HS.health -= 10;
			Debug.Log(HS.health);
		}
	}
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Wall"){
			direction = transform.position;
		}
		else if (col.gameObject.tag == "Enemy" && Time.time % HS.hitDelay == 0) {
			HS.health -= 10;
			Debug.Log("Health = " + HS.health);
		}
	}

	void ClickToMove() 
	{
		//0 for left, 1 for right, and 2 for middle click
		if (Input.GetMouseButton(1)) {
			anim.SetBool ("isUpLeft", false);
			anim.SetBool("isDownLeft", false);
			anim.SetBool ("isLeft", false);
			anim.SetBool ("isDown", false);
			anim.SetBool ("isUp", false);
			anim.SetBool ("hasMoved", false);
			//Where did the player click on the map?
			direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			direction.z = transform.position.z;
		}

		//move player to position
		transform.position = Vector3.MoveTowards (transform.position, direction, speed * Time.deltaTime);

		if (Vector3.Distance(transform.position, direction) > 0) {
			float slope = 0.0f;
			Vector3 heading = direction - transform.position;

			Debug.DrawRay (transform.position, heading, Color.red);

			slope = Mathf.Round((direction.y - transform.position.y)/(direction.x - transform.position.x));
			//play up animation
			if (Mathf.Abs(slope) > 3.0f && direction.y > playerY) {
				anim.SetBool("isUp", true);
				anim.SetBool ("hasMoved", false);
			}
			//then play up-right animation
			else if ((slope <= 3.0f && slope >= 0.3f) && (direction.y > playerY && direction.x > playerX)) {
				anim.SetBool("isUpLeft", true);
				anim.SetBool ("hasMoved", false);
				transform.eulerAngles = new Vector2(0, 0); //this sets the rotation of the gameobject
			}
			//then play right animation
			else if ((slope < 0.3f && slope > -0.3f) && direction.x > playerX) {
				anim.SetBool("isLeft", true);
				anim.SetBool ("hasMoved", false);
				transform.eulerAngles = new Vector2(0, 0); //this sets the rotation of the gameobject
			}
			//play down-right animation
			else if ((slope >= -3.0f && slope <= -0.3f) && (direction.y < playerY && direction.x > playerX)) {
				anim.SetBool("isDownLeft", true);
				anim.SetBool ("hasMoved", false);
				transform.eulerAngles = new Vector2(0, 0); //this sets the rotation of the gameobject
			}
			//play down animation
			else if (Mathf.Abs(slope) > 3.0f && direction.y < playerY) {
				anim.SetBool("isDown", true);
				anim.SetBool ("hasMoved", false);
				transform.eulerAngles = new Vector2(0, 0); //this sets the rotation of the gameobject
			}
			//play down-left animation
			else if ((slope <= 3.0f && slope >= 0.3f) && (direction.y < playerY && direction.x < playerX)) {
				anim.SetBool("isDownLeft", true);
				anim.SetBool ("hasMoved", false);
				transform.eulerAngles = new Vector2(0, 180); //this sets the rotation of the gameobject
			}
			//play left animation
			else if ((slope <= 0.3f && slope > -0.3f) && direction.x < playerX) {
				anim.SetBool("isLeft", true);
				anim.SetBool ("hasMoved", false);
				transform.eulerAngles = new Vector2(0, 180); //this sets the rotation of the gameobject
			}
			//then play up-left animation
			else if ((slope >= -3.0f && slope <= -0.3f) && (direction.y > playerY && direction.x < playerX)) {
				anim.SetBool("isUpLeft", true);
				anim.SetBool ("hasMoved", false);
				transform.eulerAngles = new Vector2(0, 180); //this sets the rotation of the gameobject
			}
		}

		//Not moving
		else {
			anim.SetBool ("isUpLeft", false);
			anim.SetBool("isDownLeft", false);
			anim.SetBool ("isLeft", false);
			anim.SetBool ("isDown", false);
			anim.SetBool ("isUp", false);
			anim.SetBool ("hasMoved", false);
		}
	}


	//for WASD movement
	void Movement() //function that stores all the movement
	{
		anim.SetBool ("isUpLeft", false);
		anim.SetBool("isDownLeft", false);
		anim.SetBool ("isLeft", false);
		anim.SetBool ("isDown", false);
		anim.SetBool ("isUp", false);
		anim.SetBool ("hasMoved", false);
		anim.SetFloat ("time", (Time.time - startTime));
		anim.SetFloat("speed", Mathf.Abs(Input.GetAxis("Horizontal")));

		//RIGHT
		if(Input.GetAxisRaw("Horizontal") > 0)
		{
			if (anim.GetBool("isUp")) 
			{
				anim.SetBool("isUpLeft", true);
				anim.SetBool("isUp", false);
			}
			else if (anim.GetBool("isDown")) 
			{
				anim.SetBool("isDownLeft", true);
				anim.SetBool("isDown", false);
			}
			else 
			{
				anim.SetBool ("isLeft", true);
			}
			anim.SetBool ("hasMoved", true);
			startTime = Time.time;
			transform.Translate(Vector3.right * speed * Time.deltaTime); 
			transform.eulerAngles = new Vector2(0, 0); //this sets the rotation of the gameobject
		}

		//UP
		if(Input.GetAxisRaw("Vertical") > 0)
		{
			if (anim.GetBool("isLeft")) 
			{
				anim.SetBool("isUpLeft", true);
				anim.SetBool("isUp", false);
			}
			else 
			{
				anim.SetBool ("isUp", true);
			}
			anim.SetBool ("hasMoved", true);
			startTime = Time.time;
			transform.Translate(Vector3.up * speed * Time.deltaTime); 
		}

		//DOWN
		if(Input.GetAxisRaw("Vertical") < 0)
		{
			if (anim.GetBool("isLeft")) 
			{
				anim.SetBool("isDownLeft", true);
				anim.SetBool("isDown", false);
			}
			else 
			{
				anim.SetBool ("isDown", true);
			}
			anim.SetBool ("hasMoved", true);
			startTime = Time.time;
			transform.Translate(Vector3.down * speed * Time.deltaTime); 
		}
		if(Input.GetAxisRaw("Horizontal") < 0)
		{
			if (anim.GetBool("isUp")) 
			{
				anim.SetBool("isUpLeft", true);
				anim.SetBool("isUp", false);
			}
			else if (anim.GetBool("isDown")) 
			{
				anim.SetBool("isDownLeft", true);
				anim.SetBool("isDown", false);
			}
			else 
			{
				anim.SetBool ("isLeft", true);
			}
			anim.SetBool("hasMoved", true);
			startTime = Time.time;
			transform.Translate(Vector3.right * speed * Time.deltaTime);
			transform.eulerAngles = new Vector2(0, 180); //this sets the rotation of the gameobject
		}

		if (Input.GetAxisRaw("Horizontal") != 0 && (anim.GetBool("isUpLeft") || anim.GetBool("isDownLeft"))) {
			anim.SetBool ("isLeft", false);

		}
	}
}
