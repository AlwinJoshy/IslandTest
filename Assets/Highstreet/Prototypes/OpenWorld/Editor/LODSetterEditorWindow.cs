using UnityEditor;
using UnityEngine;

public class LODSetterEditorWindow : EditorWindow
{
    private GameObject[] selectedObjects;
    private float lod0ScreenRelativeHeight = 0.5f;
    private float lod1ScreenRelativeHeight = 0.25f;
    private float lod2ScreenRelativeHeight = 0.1f;


    [MenuItem("Window/LOD Setter")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<LODSetterEditorWindow>("LOD Setter");
    }
    private void OnGUI()
    {
        GUILayout.Label("LOD Group Editor", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Selected GameObjects:");

        EditorGUILayout.BeginVertical(GUI.skin.box);

        int prefabsCount = EditorGUILayout.IntField("Size", selectedObjects != null ? selectedObjects.Length : 0);
        if (selectedObjects == null || selectedObjects.Length != prefabsCount)
        {
            System.Array.Resize(ref selectedObjects, prefabsCount);
        }

        for (int i = 0; i < prefabsCount; i++)
        {
            selectedObjects[i] = EditorGUILayout.ObjectField("Prefab " + (i + 1), selectedObjects[i], typeof(GameObject), true) as GameObject;
        }


        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("LOD Transition Heights:");

        lod0ScreenRelativeHeight = EditorGUILayout.FloatField("LOD0 Relative Height:", lod0ScreenRelativeHeight);
        lod1ScreenRelativeHeight = EditorGUILayout.FloatField("LOD1 Relative Height:", lod1ScreenRelativeHeight);
        lod2ScreenRelativeHeight = EditorGUILayout.FloatField("LOD2 Relative Height:", lod2ScreenRelativeHeight);

        EditorGUILayout.Space();

        if (GUILayout.Button("Add LOD Groups"))
        {
            AddLODGroups();
        }
    }

    private void AddLODGroups()
    {
        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            Debug.Log("Please select at least one GameObject first.");
            return;
        }

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            GameObject selectedObject = selectedObjects[i];
            LODGroup lodGroup = selectedObject.GetComponent<LODGroup>();
            if (lodGroup == null)
            {
                lodGroup = selectedObject.AddComponent<LODGroup>();
            }

            LOD[] lods = new LOD[3];
            Renderer[] renderers = selectedObject.GetComponentsInChildren<Renderer>();

            if (renderers.Length < 2)
            {
                Debug.Log("Selected GameObject must have at least 2 child renderers for LOD levels: " + selectedObject.name);
                continue;
            }

            // LOD0 (selected object only)
            lods[0] = new LOD(lod0ScreenRelativeHeight, new Renderer[] { selectedObject.GetComponent<Renderer>() });

            // LOD1 (first child)
            Renderer lod1Renderer = renderers[1];
            lods[1] = new LOD(lod1ScreenRelativeHeight, new Renderer[] { lod1Renderer });

            // LOD2 (second child)
            Renderer lod2Renderer = renderers[2];
            lods[2] = new LOD(lod2ScreenRelativeHeight, new Renderer[] { lod2Renderer });

            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
        }

        Debug.Log("LOD Groups added to the selected GameObjects.");
    }

}
