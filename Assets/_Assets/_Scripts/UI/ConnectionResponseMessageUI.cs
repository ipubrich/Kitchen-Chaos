using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseMessageUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.onFailedToJoinGame += KitchenGameMultiplayer_onFailedToJoinGame;
        Hide();
    }

    private void KitchenGameMultiplayer_onFailedToJoinGame(object sender, System.EventArgs e)
    {
        Show();

        messageText.text = NetworkManager.Singleton.DisconnectReason;

        if (messageText.text == "")
        {
            messageText.text = "Failed to connect";
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // unsub as UI object destroyed between scenes
    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.onFailedToJoinGame -= KitchenGameMultiplayer_onFailedToJoinGame;

    }
}
