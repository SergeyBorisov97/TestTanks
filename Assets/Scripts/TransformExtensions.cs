using UnityEngine;

public static class TransformExtensions
{
	/// <summary>
	/// Rotates the 2D transform so its forward vector points at 2D target.
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="eye">The object's axis that must face the target.</param>
	/// <param name="target">Target 2D vector.</param>
    public static void LookAt2D(this Transform transform, Vector2 eye, Vector2 target)
	{
		Vector2 look = target - (Vector2)transform.position;
		float angle = Vector2.Angle(eye, look);
		Vector2 right = Vector3.Cross(Vector3.forward, look);

		int direction = 1;

		if(Vector2.Angle(right, eye) < 90f)
			direction = -1;

		transform.rotation *= Quaternion.AngleAxis(angle * direction, Vector3.forward);
	}

	/// <summary>
	/// Rotates the 2D transform so its forward vector points at 2D target.
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="eye">The object's axis that must face the target.</param>
	/// <param name="target">Target Transform.</param>
	public static void LookAt2D(this Transform transform, Vector2 eye, Transform target)
	{
		transform.LookAt2D(eye, target.position);
	}

	/// <summary>
	/// Rotates the 2D transform so its forward vector points at 2D target.
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="eye">The object's axis that must face the target.</param>
	/// <param name="target">Target GameObject.</param>
	public static void LookAt2D(this Transform transform, Vector2 eye, GameObject target)
	{
		transform.LookAt2D(eye, target.transform.position);
	}
}