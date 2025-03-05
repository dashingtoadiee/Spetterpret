#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System;
using System.Collections.Generic;

// Never an end-point, and thus not in Editor namespace. Can be easily used by other modules.
namespace DuskModules {

  /// <summary> Utility Script for Editor related code </summary>
  public class ExtraEditorUtility : MonoBehaviour {

    // ExtraEditorUtility provides methods to more easily write custom Editor code.
    // Especially with custom properties. Call BeginProperty to add an indentation to
    // the begin x position of the GUI. All other methods remember in what indentation
    // it is, and draw the correct layout accordingly.
    // See Editor scripts in the UtilityCollection for examples.

    //================================[ GUI Workspace Variables ]================================\\
    /// <summary> Properties </summary>
    public static List<ExtraEditorPropertyCache> properties;
    /// <summary> Active property data </summary>
    public static ExtraEditorPropertyCache ap => properties[0];


    //================================[ GUI Workspace Methods ]================================\\
    // The following methods maintain and update the GUI workspace, such as remembering what indentation
    // the GUI drawing code is currently at.

    /// <summary> Begins property for easier property drawing code </summary>
    public static void BeginProperty(Rect position, GUIContent label, SerializedProperty property, bool foldOut = false, bool fullSize = false) {
      if (properties == null) properties = new List<ExtraEditorPropertyCache>();
      properties.Insert(0, new ExtraEditorPropertyCache());

      if (fullSize) {
        label.text = "";
      }

      label = EditorGUI.BeginProperty(position, label, property);
      ap.totalPosition = position;

      if (foldOut) {
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property.isExpanded, label, true);
        label.text = " ";
      }

      ap.originalPosition = position;
      ap.originalPosition.height = EditorGUIUtility.singleLineHeight;

      ap.contentPosition = EditorGUI.PrefixLabel(position, label);
      ap.contentPosition.height = EditorGUIUtility.singleLineHeight;
      ap.propertyWidth = ap.contentPosition.width;
      ap.takenSpace = 0;
      ap.totalTakenSpace = 0;

      ap.linedContentPosition = new Rect(ap.totalPosition);
      ap.linedContentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      ap.linedContentPosition.height = EditorGUIUtility.singleLineHeight;
    }

    /// <summary> Ends property. </summary>
    public static void EndProperty() {
      EditorGUI.EndProperty();
      properties.RemoveAt(0);
    }

    /// <summary> Increases the indent </summary>
    public static void IncreaseIndent() {
      EditorGUI.indentLevel++;
    }
    /// <summary> Decreases the indent </summary>
    public static void DecreaseIndent() {
      EditorGUI.indentLevel--;
    }

    /// <summary> Starts the property area </summary>
    public static int ZeroIndent() {
      int indent = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      return indent;
    }
    /// <summary> Ends the property area </summary>
    public static void ResetIndent(int indent) {
      EditorGUI.indentLevel = indent;
    }

    /// <summary> Takes up an amount of horizontal space from contentPosition. </summary>
    public static void TakeSpace(float space = 0) {
      if (space == 0) space = ap.remainingWidth;
      ap.contentPosition.x += ap.takenSpace;
      ap.contentPosition.width = space;
      ap.takenSpace = space;
      ap.totalTakenSpace += space;
    }

    /// <summary> Moves to next line </summary>
    public static void NextLine() {
      ap.contentPosition.x = ap.totalPosition.x;
      ap.contentPosition.width = ap.totalPosition.width;
      ap.contentPosition.y += EditorGUIUtility.singleLineHeight;
      ap.contentPosition.height = EditorGUIUtility.singleLineHeight;
    }


    //================================[ Drawers ]================================\\
    // The following methods all draw something within the editor GUI.

    /// <summary> Draws a GUI Layout line </summary>
    public static void LayoutLine() {
      GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
    }

    /// <summary> Draws a header label </summary>
    public static void HeaderField(string header) {
      EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
    }

    /// <summary> Adds something to the property stack </summary>
    public static void AddPropertyStack(SerializedProperty property, float width = 0) {
      AddPropertyStack(property, GUIContent.none, width);
    }

    /// <summary> Adds something to the property stack </summary>
    public static void AddPropertyStack(SerializedProperty property, GUIContent content, float width = 0) {
      ap.drawStack.Add(new DrawStackItem(property, content, width));
    }
    /// <summary> Draws the property stack </summary>
    public static void DrawPropertyStack(float labelWidth) {
      int indent = ZeroIndent();

      EditorGUIUtility.labelWidth = labelWidth;
      Rect drawPosition = new Rect(ap.contentPosition);

      float setWidth = 0;
      int regular = 0;
      for (int i = 0; i < ap.drawStack.Count; i++) {
        if (ap.drawStack[i].width > 0)
          setWidth += ap.drawStack[i].width;
        else
          regular++;
      }
      float widthMult = 1;
      if (setWidth > ap.contentPosition.width)
        widthMult = ap.contentPosition.width / setWidth;

      float remainingWidth = (ap.contentPosition.width - setWidth) / regular;

      for (int i = 0; i < ap.drawStack.Count; i++) {
        drawPosition.width = ap.drawStack[i].width > 0 ? ap.drawStack[i].width : remainingWidth;
        EditorGUI.PropertyField(drawPosition, ap.drawStack[i].property, ap.drawStack[i].content);
        drawPosition.x += drawPosition.width;
      }

      ResetIndent(indent);

      ap.drawStack = new List<DrawStackItem>();
    }

    /// <summary> Shorter version of property field, tied to this content position </summary>
    public static void PropertyField(SerializedProperty property, GUIContent content = null, bool alignZero = false) {
      if (content == null) content = GUIContent.none;

      int indent = ap.contentPosition.x > ap.totalPosition.x ? ZeroIndent() : EditorGUI.indentLevel;

      Rect useRect = new Rect(ap.contentPosition);
      if ((property.isArray || (property.hasChildren && alignZero)) && property.propertyType != SerializedPropertyType.String) {
        useRect.x = ap.originalPosition.x;
        useRect.width = ap.originalPosition.width;
      }

      EditorGUI.PropertyField(useRect, property, content, true);
      ResetIndent(indent);
    }

    /// <summary> Shorter version of property field, tied to this content position </summary>
    public static void PropertyFieldLine(SerializedProperty property, GUIContent content = null) {
      if (content == null) content = GUIContent.none;
      EditorGUI.indentLevel++;
      EditorGUI.PropertyField(ap.linedContentPosition, property, content, true);
      ap.linedContentPosition.y += EditorGUI.GetPropertyHeight(property, true) + EditorGUIUtility.standardVerticalSpacing;
      EditorGUI.indentLevel--;
    }

    /// <summary> Draws an settings-dropdown-button for a named boolean toggle. </summary>
    public static void DrawSettingsToggleDropdown(Rect contentPosition, SerializedProperty boolProperty, string trueName, string falseName, bool invert = false) {
      int indent = ZeroIndent();

      GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton);
      buttonStyle.normal.background = null;

      if (EditorGUI.DropdownButton(contentPosition, EditorGUIUtility.IconContent("Icon Dropdown"), FocusType.Passive, buttonStyle)) {
        GenericMenu settingsMenu = new GenericMenu();

        if (!invert) {
          settingsMenu.AddItem(new GUIContent(trueName), boolProperty.boolValue, delegate (object o) {
            boolProperty.boolValue = true;
            boolProperty.serializedObject.ApplyModifiedProperties();
          }, 0);

          settingsMenu.AddItem(new GUIContent(falseName), !boolProperty.boolValue, delegate (object o) {
            boolProperty.boolValue = false;
            boolProperty.serializedObject.ApplyModifiedProperties();
          }, 1);
        } else {
          settingsMenu.AddItem(new GUIContent(falseName), !boolProperty.boolValue, delegate (object o) {
            boolProperty.boolValue = false;
            boolProperty.serializedObject.ApplyModifiedProperties();
          }, 1);

          settingsMenu.AddItem(new GUIContent(trueName), boolProperty.boolValue, delegate (object o) {
            boolProperty.boolValue = true;
            boolProperty.serializedObject.ApplyModifiedProperties();
          }, 0);
        }

        settingsMenu.ShowAsContext();
      }

      ResetIndent(indent);
    }

    /// <summary> Draws a button at the current position. </summary>
    public static bool DrawButton(string buttonName) {
      int indent = ZeroIndent();
      bool result = GUI.Button(ExtraEditorUtility.ap.contentPosition, buttonName);
      ResetIndent(indent);
      return result;
    }


    /// <summary> Draws an settings-dropdown-button for an enum dropdown. </summary>
    public static void DrawSettingsEnumDropdown(Rect contentPosition, SerializedProperty enumProperty) {
      int indent = ZeroIndent();

      GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton);
      buttonStyle.normal.background = null;

      if (EditorGUI.DropdownButton(contentPosition, EditorGUIUtility.IconContent("Icon Dropdown"), FocusType.Passive, buttonStyle)) {
        GenericMenu settingsMenu = new GenericMenu();

        // Create setting for each enum name.
        for (int i = 0; i < enumProperty.enumDisplayNames.Length; i++) {
          int a = i;
          settingsMenu.AddItem(new GUIContent(enumProperty.enumDisplayNames[i]), enumProperty.enumValueIndex == i, delegate (object o) {
            enumProperty.enumValueIndex = a;
            enumProperty.serializedObject.ApplyModifiedProperties();
          }, i);
        }

        settingsMenu.ShowAsContext();
      }

      ResetIndent(indent);
    }


    //================================[ Utilities ]================================\\
    /// <summary> Gets an array of the available sorting layer names </summary>
    public static string[] GetSortingLayerNames() {
      Type internalEditorUtilityType = typeof(InternalEditorUtility);
      PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
      return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    /// <summary> Finds and returns a list of all scripts of a certain type. </summary>
    public static List<MonoScript> FindScriptsOfType<T>() {
      string[] guid = AssetDatabase.FindAssets("t:MonoScript");
      List<MonoScript> scripts = new List<MonoScript>();

      for (int i = 0; i < guid.Length; i++) {
        string path = AssetDatabase.GUIDToAssetPath(guid[i]);
        MonoScript sc = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
        System.Type t = sc.GetClass();
        if (t != null && !t.IsAbstract && (t == typeof(T) || t.IsSubclassOf(typeof(T)))) {
          scripts.Add(sc);
        }
      }
      return scripts;
    }

    /// <summary> Finds all assets of a certain type. </summary>
    public static List<T> FindAssets<T>() where T : UnityEngine.Object {
      string[] guid = AssetDatabase.FindAssets("t:" + typeof(T).Name);
      List<T> objects = new List<T>();

      for (int i = 0; i < guid.Length; i++) {
        string path = AssetDatabase.GUIDToAssetPath(guid[i]);
        objects.Add(AssetDatabase.LoadAssetAtPath<T>(path));
      }
      return objects;
    }

    /// <summary> Converts the given name to an editor friendly name. </summary>
    public static string EditorFriendlyName(string name) {
      int i = 0;
      while (i < name.Length) {
        if (i > 0 && char.IsUpper(name[i])) {
          char prev = name[i - 1];
          if (char.IsLetterOrDigit(prev)) {
            name = name.Insert(i, " ");
            i++;
          }
        }
        i++;
      }
      return name;
    }

    /// <summary> Inserts an array element into the array property </summary>
    public static void InsertArrayElement<T>(SerializedProperty property, T obj) where T : UnityEngine.Object {
      if (property.isArray) {
        int index = property.arraySize;
        property.InsertArrayElementAtIndex(index);
        property.GetArrayElementAtIndex(index).objectReferenceValue = obj;
      }
    }

    /// <summary> Inserts an array element into the array property </summary>
    public static void InsertArrayElement(SerializedProperty property, string text) {
      if (property.isArray) {
        int index = property.arraySize;
        property.InsertArrayElementAtIndex(index);
        property.GetArrayElementAtIndex(index).stringValue = text;
      }
    }

  }
}
#endif