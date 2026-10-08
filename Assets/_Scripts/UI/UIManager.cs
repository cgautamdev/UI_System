using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject inventoryPanel;

    private void Start()
    {
        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }
}
