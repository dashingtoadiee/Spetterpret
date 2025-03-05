using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Static class containing extension methods for angular calculations </summary>
public static class ActionExtensionMethods {
    
  /// <summary> Adds an action to this action. </summary>
  public static Action Add(this Action action, params Action[] others) {
    for (int i = 0; i < others.Length; i++) {
      action += others[i];
		}
    return action;
  }

  /// <summary> Adds an action to this action. </summary>
  public static Action<T> Add<T>(this Action<T> action, params Action<T>[] others) {
    for (int i = 0; i < others.Length; i++) {
      action += others[i];
    }
    return action;
  }

  /// <summary> Adds an action to this action. </summary>
  public static Action<T1, T2> Add<T1, T2>(this Action<T1, T2> action, params Action<T1, T2>[] others) {
    for (int i = 0; i < others.Length; i++) {
      action += others[i];
    }
    return action;
  }

	/// <summary> Checks whether the action is contained within this action </summary>
	public static bool Contains(this Action action, Action other) {
		Delegate[] invocations = action.GetInvocationList();
		for (int i = 0; i < invocations.Length; i++) {
			if (invocations[i].Method == other.Method)
				return true;
		}
		return false;
	}

	/// <summary> Checks whether the action is contained within this action </summary>
	public static bool Contains<T>(this Action<T> action, Action<T> other) {
		Delegate[] invocations = action.GetInvocationList();
		for (int i = 0; i < invocations.Length; i++) {
			if (invocations[i].Method == other.Method)
				return true;
		}
		return false;
	}

	/// <summary> Checks whether the action is contained within this action </summary>
	public static bool Contains<T1, T2>(this Action<T1, T2> action, Action<T1, T2> other) {
		Delegate[] invocations = action.GetInvocationList();
		for (int i = 0; i < invocations.Length; i++) {
			if (invocations[i].Method == other.Method)
				return true;
		}
		return false;
	}


}