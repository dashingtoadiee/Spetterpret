using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;

/// <summary> Static class containing static utility string methods </summary>
public static class StringExtensionMethods {

	/// <summary> Cut off the string after maxLength if it exceeds it. </summary>
	/// <param name="value"> the string value </param>
	/// <param name="maxLength"> The maximum length allowed for the string </param>
	/// <returns> The truncated string value </returns>
	public static string Truncate(this string value, int maxLength) {
		if (string.IsNullOrEmpty(value)) return value;
		return value.Length <= maxLength ? value : value.Substring(0, maxLength);
	}

}