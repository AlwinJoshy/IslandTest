using UnityEditor;
using UnityEngine;

public class PrefabPopulatorEditorWindow : EditorWindow
{
    private GameObject[] prefabs;
    private float areaRadius;
    private float maxDensityDistance;
    private float densityReductionRate;
    private Vector3 populatorCenter;
    private GameObject parentObject;
    private int maxPrefabsToSpawn;
    private int totalSpawned = 0;
    private bool isPopulating = false;

    [MenuItem("Window/Prefab Populator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<PrefabPopulatorEditorWindow>("Prefab Populator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Populator Settings", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Prefabs");
        EditorGUILayout.BeginVertical(GUI.skin.box);

        int prefabsCount = EditorGUILayout.IntField("Size", prefabs != null ? prefabs.Length : 0);
        if (prefabs == null || prefabs.Length != prefabsCount)
        {
            System.Array.Resize(ref prefabs, prefabsCount);
        }

        for (int i = 0; i < prefabsCount; i++)
        {
            prefabs[i] = EditorGUILayout.ObjectField("Prefab " + (i + 1), prefabs[i], typeof(GameObject), false) as GameObject;
        }

        EditorGUILayout.EndVertical();

        areaRadius = EditorGUILayout.FloatField("Area Radius", areaRadius);
        maxDensityDistance = EditorGUILayout.FloatField("Max Density Distance", maxDensityDistance);
        densityReductionRate = EditorGUILayout.FloatField("Density Reduction Rate", densityReductionRate);
        populatorCenter = EditorGUILayout.Vector3Field("Populator Center", populatorCenter);
        parentObject = EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true) as GameObject;
        maxPrefabsToSpawn = EditorGUILayout.IntField("Max Prefabs To Spawn", maxPrefabsToSpawn);

        EditorGUI.BeginDisabledGroup(isPopulating);
        if (GUILayout.Button("Populate"))
        {
            PopulatePrefabs();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Total Spawned: " + totalSpawned);
    }

    private void PopulatePrefabs()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("Please assign at least one prefab!");
            return;
        }

        isPopulating = true;
        totalSpawned = 0;

        GameObject parent = parentObject != null ? new GameObject("Spawned Prefabs") : null;

        for (int i = 0; i < maxPrefabsToSpawn; i++)
        {
            float distance = Random.Range(0f, areaRadius);
            float density = GetDensity(distance);

            if (Random.value <= density)
            {
                float angle = Random.Range(0f, 360f);
                Vector3 position = populatorCenter + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * distance;

                GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

                GameObject spawnedPrefab = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                spawnedPrefab.transform.position = position;
                spawnedPrefab.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                if (parent != null)
                {
                    spawnedPrefab.transform.SetParent(parent.transform);
                }

                totalSpawned++;
            }
        }

        if (parent != null)
        {
            Selection.activeGameObject = parent;
        }

        isPopulating = false;
    }

    private float GetDensity(float distance)
    {
        float density = 1f;

        if (distance > maxDensityDistance)
        {
            density -= (distance - maxDensityDistance) * densityReductionRate;
            density = Mathf.Max(0f, density);
        }

        return density;
    }
}
