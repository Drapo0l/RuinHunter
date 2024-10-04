using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]

public class SceneField
{
    [SerializeField] private Object _sceneAsset;
    [SerializeField] private string _sceneName = "";

    public string SceneName
    {
        get { return _sceneName; }   
    }

    //makes it work with the existing Unity methods (LoadLevel/LoadScene)
    public static implicit operator string(SceneField scene)
    {
        return scene.SceneName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneField))]

public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);

        SerializedProperty sceneAssest = property.FindPropertyRelative("_sceneAsset");
        SerializedProperty sceneName = property.FindPropertyRelative("_sceneName");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        if (sceneAssest != null)
        {
            sceneAssest.objectReferenceValue = EditorGUI.ObjectField(position, sceneAssest.objectReferenceValue, typeof(SceneAsset), false);

            if (sceneAssest.objectReferenceValue != null)
            {
                sceneName.stringValue = (sceneAssest.objectReferenceValue as SceneAsset).name;
            }
        }

        EditorGUI.EndProperty();
    }
}
#endif