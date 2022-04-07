using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float speed = 5f;
	public Direction direction;
	public SpriteRenderer sprite;
	public bool alive = true;

	public Transform owner;

	void Start()
	{
		sprite.enabled = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.GetComponent<AITankController>() && gameObject.tag == "PlayerProjectile") // our tank shot the enemy tank
		{
			TankControllerBase target = collision.collider.gameObject.GetComponent<TankControllerBase>();
			target.alive = false;
			Debug.Log("Player Projectile eliminated the enemy tank");

			PlayerTankController ownerController = owner.gameObject.GetComponent<PlayerTankController>();
			if(ownerController)
			{
				ownerController.SendMessage("EnemyHit");
			}
			else
				Debug.LogError("No owner controller for a projectile");
			alive = false;
		}
		if(collision.collider.gameObject.GetComponent<GameField>()) // collide with game field borders
		{
			Debug.Log("Projectile collided with a wall");
			alive = false;
		}
	}

	void Update()
	{
		if(alive)
		{
			sprite.enabled = true;
			switch(direction)
			{
				case Direction.Down:
					gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime);
					break;
				case Direction.Up:
					gameObject.transform.Translate(Vector3.up * speed * Time.deltaTime);
					break;
				case Direction.Left:
					gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
					break;
				case Direction.Right:
					gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
					break;
			}
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
}