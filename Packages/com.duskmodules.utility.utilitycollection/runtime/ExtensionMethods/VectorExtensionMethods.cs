using UnityEngine;

/// <summary> Static class containing extension methods for vectors </summary>
public static class VectorExtensionMethods {

	/// <summary> Generates a Vector2 using x and y of a Vector 3 </summary>
	/// <param name="v"> The Vector3 to convert </param>
	/// <returns> The Vector2 with xy </returns>
	public static Vector2 XY(this Vector3 v) {
		return new Vector2(v.x, v.y);
	}

	/// <summary> Generates a Vector2 using x and z of a Vector 3 </summary>
	/// <param name="v"> The Vector3 to convert </param>
	/// <returns> The Vector2 with xz </returns>
	public static Vector2 XZ(this Vector3 v) {
		return new Vector2(v.x, v.z);
	}

	/// <summary> Adjusts the x value of the Vector3 </summary>
	/// <param name="v"> The Vector3 to adjust </param>
	/// <param name="x"> The X value to set </param>
	/// <returns> The adjusted Vector3 </returns>
	public static Vector3 WithX(this Vector3 v, float x) {
		return new Vector3(x, v.y, v.z);
	}

	/// <summary> Adjusts the y value of the Vector3 </summary>
	/// <param name="v"> The Vector3 to adjust </param>
	/// <param name="y"> The Y value to set </param>
	/// <returns> The adjusted Vector3 </returns>
	public static Vector3 WithY(this Vector3 v, float y) {
		return new Vector3(v.x, y, v.z);
	}

	/// <summary> Adjusts the z value of the Vector3 </summary>
	/// <param name="v"> The Vector3 to adjust </param>
	/// <param name="z"> The Z value to set </param>
	/// <returns> The adjusted Vector3 </returns>
	public static Vector3 WithZ(this Vector3 v, float z) {
		return new Vector3(v.x, v.y, z);
	}

	/// <summary> Adjusts the x value of the Vector2 </summary>
	/// <param name="v"> The Vector2 to adjust </param>
	/// <param name="x"> The X value to set </param>
	/// <returns> The adjusted Vector2 </returns>
	public static Vector2 WithX(this Vector2 v, float x) {
		return new Vector2(x, v.y);
	}

	/// <summary> Adjusts the y value of the Vector2 </summary>
	/// <param name="v"> The Vector2 to adjust </param>
	/// <param name="y"> The Y value to set </param>
	/// <returns> The adjusted Vector2 </returns>
	public static Vector2 WithY(this Vector2 v, float y) {
		return new Vector2(v.x, y);
	}

	/// <summary> Adds a z value to the Vector2, making it a Vector3 </summary>
	/// <param name="v"> The Vector2 to adjust </param>
	/// <param name="z"> The Z value to add </param>
	/// <returns> The new Vector3 </returns>
	public static Vector3 WithZ(this Vector2 v, float z) {
		return new Vector3(v.x, v.y, z);
	}

	/// <summary> Multiplies the vectors together </summary>
	/// <param name="a"> What vectors to multiply together </param>
	/// <returns> The multiplied vector </returns>
	public static Vector3 Multiply(this Vector3 v, params Vector3[] a) {
		for (int i = 0; i < a.Length; i++) {
			v.x *= a[i].x;
			v.y *= a[i].y;
			v.z *= a[i].z;
		}
		return v;
	}

	/// <summary> Multiplies the vectors together </summary>
	/// <param name="a"> What vectors to multiply together </param>
	/// <returns> The multiplied vector </returns>
	public static Vector2 Multiply(this Vector2 v, params Vector2[] a) {
		for (int i = 0; i < a.Length; i++) {
			v.x *= a[i].x;
			v.y *= a[i].y;
		}
		return v;
	}

	/// <summary> Calculates and returns the point nearest to the given axis direction. Basically finds the nearest point on a line to the given point. </summary>
	/// <param name="axisDirection"> The directional axis Vector3 (basically a line that passes through (0,0,0). </param>
	/// <param name="point"> The location vector3 to find nearest to </param>
	/// <param name="isNormalized"> Whether the axis is normalized. </param>
	/// <returns> The point on the line nearest to the given point </returns>
	public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false) {
		if (!isNormalized) axisDirection.Normalize();
		var d = Vector3.Dot(point, axisDirection);
		return axisDirection * d;
	}

	/// <summary> Calculates and returns the point nearest to the given line. </summary>
	/// <param name="lineDirection"> The directional Vector3 of the line. </param>
	/// <param name="pointOnLine"> Any point of the line (usually the origin). </param>
	/// <param name="point"> The point to check nearest line-point for </param>
	/// <param name="isNormalized"> Whether the line direction is normalized </param>
	/// <returns> The point on the given line nearest to the given point </returns>
	public static Vector3 NearestPointOnLine(this Vector3 lineDirection, Vector3 pointOnLine, Vector3 point, bool isNormalized = false) {
		if (!isNormalized) lineDirection.Normalize();
		var d = Vector3.Dot(point - pointOnLine, lineDirection);
		return pointOnLine + (lineDirection * d);
	}

}