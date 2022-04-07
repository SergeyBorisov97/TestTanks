using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankController : TankControllerBase
{
	public int enemiesDestroyed = 0;

	private void OnGUI()
	{
		Event e = Event.current;
		if(e.type == EventType.KeyUp && e.keyCode != KeyCode.Mouse0)
		{
			currentSpeed = 0f;
		}
	}

	protected override void Control()
	{
		#region Checking keys
		if(Input.GetKey(KeyCode.A)) // move left
		{
			currentSpeed = speed;
			direction = Direction.Left;
		}
		if(Input.GetKey(KeyCode.S)) // move down
		{
			currentSpeed = speed;
			direction = Direction.Down;
		}
		if(Input.GetKey(KeyCode.D)) // move right
		{
			currentSpeed = speed;
			direction = Direction.Right;
		}
		if(Input.GetKey(KeyCode.W)) // move up
		{
			currentSpeed = speed;
			direction = Direction.Up;
		}
		#endregion

		LookAtDirection(direction);

		if(Input.GetMouseButtonDown(0)) // left mouse button
		{
			if(Time.time - Time.deltaTime > 1f)
				Shoot();
		}

		if(currentSpeed != 0f)
		{
			transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
		}
	}

	protected override void Shoot()
	{
		projectile = Instantiate(projectilePrefab);
		projectile.tag = "PlayerProjectile";
		projectile.owner = transform;
		projectile.transform.SetParent(transform, false);
		projectile.transform.localPosition = projectilePosition.localPosition;

		projectile.transform.SetParent(null);
		projectile.transform.position = projectile.transform.TransformPoint(projectilePosition.localPosition);
	}

	public void EnemyHit()
	{
		enemiesDestroyed++;
		Debug.Log("Enemy hit");
	}

	protected override void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.GetComponent<AITankController>()) // collided with AI tank
		{
			alive = false;
			StartCoroutine(Die());
		}
	}

	void Update()
    {
		Control();
	}
}