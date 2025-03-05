using UnityEngine;

/// <summary> Static class containing extension methods for numbers </summary>
public static class UnityNumericalExtensionMethods {

	/// <summary> Remaps a value between valueRangeMin and valueRangeMax to newRangeMin and newRangeMax </summary>
	/// <param name="value"> Value to remap </param>
	/// <param name="valueRangeMin"> Original range minimum </param>
	/// <param name="valueRangeMax"> Original range maximum </param>
	/// <param name="newRangeMin"> New range minimum </param>
	/// <param name="newRangeMax"> New range maximum </param>
	/// <returns> The remapped value between newRangeMin and newRangeMax </returns>
	public static float LinearRemap(this float value, float valueRangeMin, float valueRangeMax, float newRangeMin, float newRangeMax) {
		return (value - valueRangeMin) / (valueRangeMax - valueRangeMin) * (newRangeMax - newRangeMin) + newRangeMin;
	}

	/// <summary> Randomly selects value or -value depending on negativeProbability value </summary>
	/// <param name="value"> The value to randomize </param>
	/// <param name="negativeProbability"> Probability of it being negative </param>
	/// <returns> The randomized value </returns>
	public static int WithRandomSign(this int value, float negativeProbability = 0.5f) {
		return Random.value < negativeProbability ? -value : value;
	}

	/// <summary> Randomly selects value or -value depending on negativeProbability value </summary>
	/// <param name="value"> The value to randomize </param>
	/// <param name="negativeProbability"> Probability of it being negative </param>
	/// <returns> The randomized value </returns>
	public static float WithRandomSign(this float value, float negativeProbability = 0.5f) {
		return Random.value < negativeProbability ? -value : value;
	}

	/// <summary> Creates a string of the given time (in seconds). </summary>
	/// <param name="time"> What time in seconds </param>
	/// <param name="twoDigitMinutes"> If true, the minute part of the string always has at least 2 digits </param>
	/// <returns> The string of the time </returns>
	public static string TimeString(this float time, bool twoDigitMinutes = true) {
		int min = (int)(time / 60);
		int sec = (int)(time - (min * 60));

		string minStr = min + "";
		if (twoDigitMinutes && min < 10) minStr = "0" + minStr;
		string secStr = sec + "";
		if (sec < 10) secStr = "0" + secStr;

		return minStr + ":" + secStr;
	}
}