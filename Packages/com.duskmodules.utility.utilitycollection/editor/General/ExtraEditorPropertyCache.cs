#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System;
using System.Collections.Generic;

// Never an end-point, and thus not in Editor namespace. Can be used by other modules.
namespace DuskModules {

  /// <summary> Cache of extra editor property data </summary>
  public class ExtraEditorPropertyCache {
    /// <summary> Original position of GUI call </summary>
    public Rect originalPosition;
    /// <summary> Total position of GUI call </summary>
    public Rect totalPosition;
    /// <summary> Current working content position </summary>
    public Rect contentPosition;
    /// <summary> Total width of property area </summary>
    public float propertyWidth;
    /// <summary> Remaining width of property area </summary>
    public float remainingWidth => propertyWidth - totalTakenSpace;

    /// <summary> How much space has been taken totally </summary>
    public float totalTakenSpace;
    /// <summary> How much space has been taken by previous set </summary>
    public float takenSpace;

    /// <summary> All draw stack items </summary>
    public List<DrawStackItem> drawStack;

    /// <summary> Content position for lined properties </summary>
    public Rect linedContentPosition;

    // Constructor
    public ExtraEditorPropertyCache() {
      drawStack = new List<DrawStackItem>();
    }
  }

  /// <summary> A single draw stack item for use in draw stacks </summary>
  public class DrawStackItem {

    public SerializedProperty property;
    public GUIContent content;
    public float width;

    public DrawStackItem(SerializedProperty property, GUIContent content, float width) {
      this.property = property;
      this.content = content;
      this.width = width;
    }

  }
}
#endif