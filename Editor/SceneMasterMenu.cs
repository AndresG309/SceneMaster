using UnityEditor;
using UnityEngine;

namespace AndresG09.SceneMaster.Editor
{
    public static class SceneMasterMenu
    {
        [MenuItem("GameObject/SceneMaster/Create SceneMaster", false, 10)]
        private static void CreateSceneMaster(MenuCommand command)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Packages/com.andresg09.scenemaster/Prefabs/SceneMaster.prefab"
            );

            if (prefab == null)
            {
                Debug.LogError("SceneMaster prefab could not be found.");
                return;
            }

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            GameObjectUtility.SetParentAndAlign(instance, command.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, "Create SceneMaster");

            Selection.activeGameObject = instance;
        }
    }
}