using TMPro;
using UnityEngine;

public class FontUpdater : MonoBehaviour
{
    public TMP_FontAsset newFontAsset;

    [ContextMenu("Update All Fonts In Scene")]
    void UpdateFonts()
    {
        TMP_Text[] textComponents = FindObjectsByType<TMP_Text>(FindObjectsSortMode.InstanceID);
        foreach(var tmp in textComponents) {
            tmp.font = newFontAsset;
        }
        Debug.Log($"Updated {textComponents.Length} TMP components.");
    }
}
