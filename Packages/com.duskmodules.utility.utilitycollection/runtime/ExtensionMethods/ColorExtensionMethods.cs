using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Static class containing extension methods for colors </summary>
public static class ColorExtensionMethods {

	/// <summary> Adjusts the a value of the color </summary>
	/// <param name="c"> What color to adjust </param>
	/// <param name="a"> What a to set </param>
	/// <returns> The adjusted color </returns>
	public static Color WithR(this Color c, float r) {
		c.r = r;
		return c;
	}

	/// <summary> Adjusts the g value of the color </summary>
	/// <param name="c"> What color to adjust </param>
	/// <param name="g"> What g to set </param>
	/// <returns> The adjusted color </returns>
	public static Color WithG(this Color c, float g) {
		c.g = g;
		return c;
	}

	/// <summary> Adjusts the b value of the color </summary>
	/// <param name="c"> What color to adjust </param>
	/// <param name="b"> What b to set </param>
	/// <returns> The adjusted color </returns>
	public static Color WithB(this Color c, float b) {
		c.b = b;
		return c;
	}

	/// <summary> Adjusts the a value of the color </summary>
	/// <param name="c"> What color to adjust </param>
	/// <param name="a"> What a to set </param>
	/// <returns> The adjusted color </returns>
	public static Color WithA(this Color c, float a) {
		c.a = a;
		return c;
	}
}