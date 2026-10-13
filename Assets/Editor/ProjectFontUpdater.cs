using UnityEngine;
using UnityEditor;
using TMPro;

public class ProjectFontUpdater : EditorWindow
{
    private TMP_FontAsset newFontAsset;

    [MenuItem("Tools/Project Font Updater")]
    public static void ShowWindow()
    {
        GetWindow<ProjectFontUpdater>("Font Updater");
    }

    private void OnGUI()
    {
        GUILayout.Label("Update All Fonts Project-Wide", EditorStyles.boldLabel);

        newFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font Asset", newFontAsset, typeof(TMP_FontAsset), false);

        if(GUILayout.Button("Update All Prefabs")) {
            if(newFontAsset == null) {
                EditorUtility.DisplayDialog("Error", "Please assign a New TMP Font Asset first!", "OK");
                return;
            }

            UpdateFontsInPrefabs();
        }
    }

    private void UpdateFontsInPrefabs()
    {
        // Find all prefab asset GUIDs in the project
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        int updatedCount = 0;

        for(int i = 0; i < guids.Length; i++) {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);

            // Display a progress bar for large projects
            EditorUtility.DisplayProgressBar("Updating Fonts", $"Processing: {path}", (float)i / guids.Length);

            // Load the prefab asset
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if(prefab != null) {
                // Find all TMP components inside the prefab (including inactive ones)
                TMP_Text[] tmpTexts = prefab.GetComponentsInChildren<TMP_Text>(true);

                if(tmpTexts.Length > 0) {
                    bool isDirty = false;

                    foreach(var tmp in tmpTexts) {
                        if(tmp.font != newFontAsset) {
                            // Record change for Undo and flag it to save
                            Undo.RecordObject(tmp, "Change TMP Font");
                            tmp.font = newFontAsset;
                            isDirty = true;
                        }
                    }

                    if(isDirty) {
                        // Save changes back to the prefab asset
                        PrefabUtility.SavePrefabAsset(prefab);
                        updatedCount++;
                    }
                }
            }
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", $"Updated fonts in {updatedCount} prefabs across the project!", "OK");
    }
}
