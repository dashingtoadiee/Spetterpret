using System;
using System.Collections.Generic;

/// <summary> Static class containing some static functions for list shuffling & selection </summary>
public static class ListExtensionMethods {

	/// <summary> Shuffle the list in place using the Fisher-Yates method. </summary>
	/// <typeparam name="T"> Type of List </typeparam>
	/// <param name="list"> List to shuffle </param>
	public static void Shuffle<T>(this IList<T> list) {
		Random rng = new Random();
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	/// <summary> Return a random item from the list. Sampling with replacement.  </summary>
	/// <typeparam name="T"> Type of List </typeparam>
	/// <param name="list"> List to get random item from </param>
	/// <returns> The randomly selected item </returns>
	public static T GetRandom<T>(this IList<T> list) {
		if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	/// <summary> Removes a random item from the list, returning that item. Sampling without replacement. </summary>
	/// <typeparam name="T"> Type of List </typeparam>
	/// <param name="list"> List to remove random from </param>
	/// <returns> The randomly selected item </returns>
	public static T RemoveRandom<T>(this IList<T> list) {
		if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
		int index = UnityEngine.Random.Range(0, list.Count);
		T item = list[index];
		list.RemoveAt(index);
		return item;
	}

	/// <summary> Return a collection of random items from a list. </summary>
	/// <typeparam name="T"> Type of list. </typeparam>
	/// <param name="list"> List to get random items from. </param>
	/// <param name="size"> Size of how many the new list should contain </param>
	/// <param name="duplicates"> Whether duplicate entries are allowed. </param>
	/// <returns> A new list with randomly selected entries </returns>
	public static List<T> GetRandomCollection<T>(this IList<T> list, int size = -1, bool duplicates = false) {
		IList<T> pool = (duplicates) ? list : new List<T>(list);
		List<T> newList = new List<T>();

		if (size < 0 || size > pool.Count) size = pool.Count;
		for (int i = 0; i < size; i++) {
			if (duplicates)
				newList.Add(pool.GetRandom());
			else
				newList.Add(pool.RemoveRandom());
		}

		return newList;
	}

	/// <summary> Returns a collection of random items from a list. The items taken are removed from the original list. </summary>
	/// <typeparam name="T"> Type of list. </typeparam>
	/// <param name="list"> List to get random items from. </param>
	/// <param name="size"> Size of how many the new list should contain </param>
	/// <returns> A new list with randomly selected entries </returns>
	public static List<T> RemoveRandomCollection<T>(this IList<T> list, int size = -1) {
		List<T> newList = new List<T>();

		if (size < 0 || size > list.Count) size = list.Count;
		for (int i = 0; i < size; i++) {
			newList.Add(list.RemoveRandom());
		}

		return newList;
	}
}