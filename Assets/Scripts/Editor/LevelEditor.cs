using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelEditorObject))]
    public class LevelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Add Graph To DB"))
            {
                CreateGraph();
            }
        }

        public void CreateGraph()
        {
            var me = (LevelEditorObject) target;
            me.Database.AddItem(GraphDescription.SampleGraph());
//            var asset = ScriptableObject.CreateInstance<LevelDataBase>();
//            AssetDatabase.CreateAsset(asset, "Assets/Database.asset");
//            asset.AddItem(GraphDescription.SampleGraph());
            EditorUtility.SetDirty(me);
            EditorSceneManager.MarkSceneDirty(me.gameObject.scene); 
            AssetDatabase.SaveAssets();
        }
    }
}
