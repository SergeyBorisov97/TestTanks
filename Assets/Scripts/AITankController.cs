using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITankController : TankControllerBase
{
	public float changeDirectionInterval = 3f;
	public bool isStuck = false; // if stuck in the corner of game field

	protected override void Control()
	{
		if(currentSpeed != 0f)
		{
			transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
		}
		if(isStuck)
			RespawnOnStuck();
	}

	protected override void Shoot()
	{
		// shooting code goes here...
	}

	private new void Start()
	{
		base.Start();
		currentSpeed = speed;
		direction = GetRandomDirection();
		LookAtDirection(direction);
		StartCoroutine(ChangeDirection());
	}

	private void Update()
	{
		if(alive)
		{
			Control();
		}
	}

	private Direction GetRandomDirection()
	{
		return (Direction)Random.Range((int)Direction.Up, (int)Direction.Right + 1);
	}

	private IEnumerator ChangeDirection()
	{
		yield return new WaitForSeconds(changeDirectionInterval);
		direction = GetRandomDirection();
		LookAtDirection(direction);
	}

	private GameObject GetClosestObjectWithTag(string tag)
	{
		GameObject[] objects;
		objects = GameObject.FindGameObjectsWithTag(tag);
		GameObject closest = null;

		float distance = Mathf.Infinity;
		Vector3 pos = transform.position;

		foreach(GameObject go in objects)
		{
			Vector3 diff = go.transform.position - pos;
			float thisDist = diff.sqrMagnitude;
			if(thisDist < distance)
			{
				closest = go;
				distance = thisDist;
			}
		}
		return closest;
	}

	private void RespawnOnStuck()
	{
		Transform respawnPoint = GetClosestObjectWithTag("Respawn").transform;
		transform.position = respawnPoint.position;
		isStuck = false;
	}

	protected override void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.GetComponent<GameField>() || collision.gameObject.GetComponent<AITankController>())
		{
			switch(direction)
			{
				case Direction.Down:
					direction = Direction.Up;
					break;
				case Direction.Right:
					direction = Direction.Left;
					break;
				case Direction.Left:
					direction = Direction.Right;
					break;
				case Direction.Up:
					direction = Direction.Down;
					break;
			}
			LookAtDirection(direction);

			if(collision.collider is EdgeCollider2D) // collider of game field
			{
				if(transform.rotation.z % 90 != 0) // the rotation isn't straight, so the tank is stuck in corner
				{
					isStuck = true;
				}
			}
		}
		if(collision.gameObject.tag == "PlayerProjectile")
		{
			alive = false;
			StartCoroutine(Die());
		}
	}
}