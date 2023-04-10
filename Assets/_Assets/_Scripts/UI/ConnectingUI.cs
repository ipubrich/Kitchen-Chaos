using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameMultiplayer.Instance.onTryingToJoinGame += KitchenGameMultiplayer_onTryingToJoinGame;
        KitchenGameMultiplayer.Instance.onFailedToJoinGame += KitchenGameMultiplayer_onFailedToJoinGame;
        Hide();
    }

    private void KitchenGameMultiplayer_onFailedToJoinGame(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameMultiplayer_onTryingToJoinGame(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // unsub as UI object destroyed between scenes
        KitchenGameMultiplayer.Instance.onTryingToJoinGame -= KitchenGameMultiplayer_onTryingToJoinGame;
        KitchenGameMultiplayer.Instance.onFailedToJoinGame -= KitchenGameMultiplayer_onFailedToJoinGame;

    }
}
