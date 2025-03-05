using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules;

/// <summary> Static class containing extension methods for colors </summary>
public static class OtherExtensionMethods {

  /// <summary> Inverts the direction </summary>
  public static Direction Invert(this Direction direction) {
    switch (direction) {
      case Direction.left: return Direction.right;
      case Direction.up: return Direction.down;
      case Direction.right: return Direction.left;
      case Direction.down: return Direction.up;
    }
    return direction;
  }

  /// <summary> Checks if horizontal </summary>
  public static bool IsHorizontal(this Direction direction) {
    switch (direction) {
      case Direction.left: return true;
      case Direction.up: return false;
      case Direction.right: return true;
      case Direction.down: return false;
    }
    return false;
  }

  /// <summary> Checks if vertical </summary>
  public static bool IsVertical(this Direction direction) {
    switch (direction) {
      case Direction.left: return false;
      case Direction.up: return true;
      case Direction.right: return false;
      case Direction.down: return true;
    }
    return false;
  }


}
