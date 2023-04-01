using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(() => {
            Debug.Log("NETWORK HOST");
            NetworkManager.Singleton.StartHost(); // start host
            Hide();
        });       
        startClientButton.onClick.AddListener(() => {
            Debug.Log("NETWORK CLIENT");
            NetworkManager.Singleton.StartClient(); // start client
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
