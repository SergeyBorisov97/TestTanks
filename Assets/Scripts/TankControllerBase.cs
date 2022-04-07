using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction { Up, Down, Left, Right }

public enum Corner { TopLeft, TopRight, BottomLeft, BottomRight }

public abstract class TankControllerBase : MonoBehaviour
{
	public Direction direction;
	public float speed = 5f;
	public float currentSpeed = 0f;
	public bool alive = true;

	public Projectile projectile;

	protected Projectile projectilePrefab;

	/// <summary>
	/// Position of a projectile (end of tank's rifle) when firing starts.
	/// </summary>
	public Transform projectilePosition;

	/// <summary>
	/// Position of a projectile relative to its initial position. Used for detaching the projectile from tank's hierarchy after launch.
	/// </summary>
	protected Vector3 relativeProjectilePos = Vector3.zero;

	/// <summary>
	/// Control your tank.
	/// </summary>
	protected abstract void Control();

	/// <summary>
	/// Shoot!
	/// </summary>
	protected abstract void Shoot();

	protected abstract void OnCollisionEnter2D(Collision2D collision);

	protected virtual IEnumerator Die()
	{
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(1f);

		//Vector2 spawnPos = GetRandomCorner();

		Bounds fieldBounds = FindObjectOfType<GameField>().GetComponent<EdgeCollider2D>().bounds;

		Vector2 spawnPos = new Vector2();
		do
		{
			spawnPos = GetRandomCorner();
		}
		while(!fieldBounds.Contains(spawnPos)); // until the spawn point will be inside game field (when no other objects are in this position)

		transform.position = spawnPos;
		//Debug.Log("Respawn");
		GetComponent<SpriteRenderer>().enabled = true;
		alive = true;
	}

	/// <summary>
	/// Returns a random corner position for the tank respawn.
	/// </summary>
	/// <returns></returns>
	protected virtual Vector2 GetRandomCorner()
	{
		Corner randomCorner = (Corner)Random.Range((int)Corner.TopLeft, (int)Corner.BottomRight + 1);

		GameField gameField = FindObjectOfType<GameField>();
		Bounds fieldBounds = gameField.GetComponent<EdgeCollider2D>().bounds;
		Vector3 fieldExtents = fieldBounds.extents;
		Bounds tankBounds = GetComponent<PolygonCollider2D>().bounds;
		Vector3 tankExtents = tankBounds.extents;

		Vector2 position = new Vector2();
		switch(randomCorner)
		{
			case Corner.TopLeft:
				position = new Vector2(-fieldExtents.x + tankExtents.x, fieldExtents.y - tankExtents.y);
				break;
			case Corner.TopRight:
				position = new Vector2(fieldExtents.x - tankExtents.x, fieldExtents.y - tankExtents.y);
				break;
			case Corner.BottomLeft:
				position = new Vector2(-fieldExtents.x + tankExtents.x, fieldExtents.y + tankExtents.y);
				break;
			case Corner.BottomRight:
				position = new Vector2(fieldExtents.x - tankExtents.x, fieldExtents.y + tankExtents.y);
				break;
		}
		return position;
	}

	public void Start()
	{
		direction = Direction.Up;
		projectilePrefab = Resources.Load<Projectile>("Prefabs/Projectile");
	}

	public void LookAtDirection(Direction direction)
	{
		switch(direction)
		{
			case Direction.Down:
				transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.down);
				break;
			case Direction.Up:
				transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
				break;
			case Direction.Left:
				transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.left);
				break;
			case Direction.Right:
				transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
				break;
		}
	}
}