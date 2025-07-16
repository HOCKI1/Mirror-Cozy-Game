using Mirror;
using UnityEngine;
using System.Collections;

public class SendPlayerChoice : MonoBehaviour
{
    IEnumerator Start()
    {
        // Дождаться подключения
        while (!NetworkClient.isConnected)
        {
            yield return null;
        }

        string selected = PlayerPrefs.GetString("SelectedCharacter", "Fox");
        Debug.Log($"[Client] Отправка выбора: {selected}");

        NetworkClient.Send(new AddPlayerMessageWithSelection
        {
            character = selected
        });
    }
}
