using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class SceneTransitionSceneView
{
    static SceneTransitionSceneView()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        SceneTransitionHolder[] transitions = Object.FindObjectsOfType<SceneTransitionHolder>();

        foreach (SceneTransitionHolder transition in transitions)
        {
            if (transition == null || string.IsNullOrEmpty(transition.nextSceneName))
                continue;

            Vector3 worldPos = transition.transform.position;

            float size = HandleUtility.GetHandleSize(worldPos) * 0.2f;

            Handles.color = Color.white;

            if (Handles.Button(worldPos,Quaternion.identity,size,size,Handles.DotHandleCap))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    foreach (var scene in EditorBuildSettings.scenes)
                    {
                        if (scene.path.Contains(transition.nextSceneName))
                        {
                            EditorSceneManager.OpenScene(scene.path);
                            break;
                        }
                    }
                }
            }
        }
    }

}
