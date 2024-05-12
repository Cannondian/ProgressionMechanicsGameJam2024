using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class FixMismatchedLODs : EditorWindow
    {
        [MenuItem("MENUITEM/MENUITEMCOMMAND")]
        private static void ShowWindow()
        {
            var window = GetWindow<FixMismatchedLODs>("Fix Mismatched LODs");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Fix"))
            {
                // get the objects in the scene and find Module14
                var objects = FindObjectsOfType<GameObject>();
                foreach (var go in objects)
                {
                    if (go.name == "_Addons14")
                    {
                        Fix(go);
                    }
                }
                
            }
        }

        private void Fix(GameObject go)
        {
            // for each object, if it has children, reset the child's transforms
            foreach (Transform child in go.transform)
            {
                if (child.childCount > 0)
                {
                    foreach (Transform grandchild in child)
                    {
                        grandchild.localPosition = Vector3.zero;
                        grandchild.localRotation = Quaternion.identity;
                        grandchild.localScale = Vector3.one;
                    }
                }
            }
        }
    }
}