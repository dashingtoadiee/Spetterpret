
namespace DuskModules {

  /// <summary> Simple directional enum </summary>
  public enum Direction {
    none, left, up, right, down, any
  }

  /// <summary> Simple on/off enum </summary>
  public enum OnOff {
    on, off
  }

  /// <summary> Type of time to update on </summary>
  public enum TimeType {
    deltaTime,
    interfaceDeltaTime
  }

  /// <summary> Months in english </summary>
  public enum Month {
    none,
    Januari,
    Februari,
    March,
    April,
    May,
    June,
    Juli,
    Augustus,
    September,
    Oktober,
    November,
    December
  }

  /// <summary> Axis </summary>
  public enum Axis {
    none,
    horizontal,
    vertical
  }

  /// <summary> Axis 3D space </summary>
  public enum Axis3D {
    none,
    x,
    y,
    z
  }

  /// <summary> Interactions </summary>
  public enum InteractionType {
    pressed,
    released
  }

  /// <summary> Comparison relation </summary>
  public enum ComparisonRelation {
    equal,
    notEqual,
    greater,
    lesser,
    greaterOrEqual,
    lesserOrEqual
  }

}