using System;

namespace DuskModules {

  /// <summary> Action called during a state change of an object. If it happens to be called before a listener is listening
  /// then the listener's action is called upon subscribe, allowing them to respond to the current state. </summary>
  public class StateAction {
    public Action action { get; protected set; }
    public int invokeCount { get; protected set; }

    public void Invoke() {
      invokeCount++;
      action?.Invoke();
    }

    public static StateAction operator +(StateAction setupAction, Action action) {
      setupAction.action += action;
      if (setupAction.invokeCount >= 1)
        action?.Invoke();
      return setupAction;
    }

    public static StateAction operator -(StateAction setupAction, Action action) {
      setupAction.action -= action;
      return setupAction;
    }
  }

  /// <summary> Action called during a state change of an object. If it happens to be called before a listener is listening
  /// then the listener's action is called upon subscribe, allowing them to respond to the current state. </summary>
  public class StateAction<T> {
    public Action<T> action { get; protected set; }
    public int invokeCount { get; protected set; }
    public T lastParam { get; protected set; }

    public void Invoke(T param) {
      invokeCount++;
      lastParam = param;
      action?.Invoke(param);
    }

    public static StateAction<T> operator +(StateAction<T> setupAction, Action<T> action) {
      setupAction.action += action;
      if (setupAction.invokeCount >= 1)
        action?.Invoke(setupAction.lastParam);
      return setupAction;
    }

    public static StateAction<T> operator -(StateAction<T> setupAction, Action<T> action) {
      setupAction.action -= action;
      return setupAction;
    }
  }

  /// <summary> Action called during a state change of an object. If it happens to be called before a listener is listening
  /// then the listener's action is called upon subscribe, allowing them to respond to the current state. </summary>
  public class StateAction<T1, T2> {
    public Action<T1, T2> action { get; protected set; }
    public int invokeCount { get; protected set; }
    public T1 lastParam1 { get; protected set; }
    public T2 lastParam2 { get; protected set; }

    public void Invoke(T1 param1, T2 param2) {
      invokeCount++;
      lastParam1 = param1;
      lastParam2 = param2;
      action?.Invoke(param1, param2);
    }

    public static StateAction<T1, T2> operator +(StateAction<T1, T2> setupAction, Action<T1, T2> action) {
      setupAction.action += action;
      if (setupAction.invokeCount >= 1)
        action?.Invoke(setupAction.lastParam1, setupAction.lastParam2);
      return setupAction;
    }

    public static StateAction<T1, T2> operator -(StateAction<T1, T2> setupAction, Action<T1, T2> action) {
      setupAction.action -= action;
      return setupAction;
    }
  }

  /// <summary> Action called during a state change of an object. If it happens to be called before a listener is listening
  /// then the listener's action is called upon subscribe, allowing them to respond to the current state. </summary>
  public class StateAction<T1, T2, T3> {
    public Action<T1, T2, T3> action { get; protected set; }
    public int invokeCount { get; protected set; }
    public T1 lastParam1 { get; protected set; }
    public T2 lastParam2 { get; protected set; }
    public T3 lastParam3 { get; protected set; }

    public void Invoke(T1 param1, T2 param2, T3 param3) {
      invokeCount++;
      lastParam1 = param1;
      lastParam2 = param2;
      lastParam3 = param3;
      action?.Invoke(param1, param2, param3);
    }

    public static StateAction<T1, T2, T3> operator +(StateAction<T1, T2, T3> setupAction, Action<T1, T2, T3> action) {
      setupAction.action += action;
      if (setupAction.invokeCount >= 1)
        action?.Invoke(setupAction.lastParam1, setupAction.lastParam2, setupAction.lastParam3);
      return setupAction;
    }

    public static StateAction<T1, T2, T3> operator -(StateAction<T1, T2, T3> setupAction, Action<T1, T2, T3> action) {
      setupAction.action -= action;
      return setupAction;
    }
  }

  /// <summary> Action called during a state change of an object. If it happens to be called before a listener is listening
  /// then the listener's action is called upon subscribe, allowing them to respond to the current state. </summary>
  public class StateAction<T1, T2, T3, T4> {
    public Action<T1, T2, T3, T4> action { get; protected set; }
    public int invokeCount { get; protected set; }
    public T1 lastParam1 { get; protected set; }
    public T2 lastParam2 { get; protected set; }
    public T3 lastParam3 { get; protected set; }
    public T4 lastParam4 { get; protected set; }

    public void Invoke(T1 param1, T2 param2, T3 param3, T4 param4) {
      invokeCount++;
      lastParam1 = param1;
      lastParam2 = param2;
      lastParam3 = param3;
      lastParam4 = param4;
      action?.Invoke(param1, param2, param3, param4);
    }

    public static StateAction<T1, T2, T3, T4> operator +(StateAction<T1, T2, T3, T4> setupAction, Action<T1, T2, T3, T4> action) {
      setupAction.action += action;
      if (setupAction.invokeCount >= 1)
        action?.Invoke(setupAction.lastParam1, setupAction.lastParam2, setupAction.lastParam3, setupAction.lastParam4);
      return setupAction;
    }

    public static StateAction<T1, T2, T3, T4> operator -(StateAction<T1, T2, T3, T4> setupAction, Action<T1, T2, T3, T4> action) {
      setupAction.action -= action;
      return setupAction;
    }
  }

}