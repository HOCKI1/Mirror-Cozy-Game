using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public GameObject foxPrefab;
    public GameObject birdPrefab;

    // Обработчик старта сервера
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<AddPlayerMessageWithSelection>(OnCreateCharacter);
    }

    // Обработчик создания персонажа
    void OnCreateCharacter(NetworkConnectionToClient conn, AddPlayerMessageWithSelection msg)
    {
        GameObject prefab = msg.character == "Bird" ? birdPrefab : foxPrefab;

        if (prefab == null)
        {
            Debug.LogError("foxPrefab и birdPrefab не назначены.");
            return;
        }

        // Задаем стартовую позицию
        Transform startPos = GetStartPosition();
        Vector3 spawnPosition = startPos != null ? startPos.position : Vector3.zero + new Vector3(0, 2, 0);
        Quaternion spawnRotation = startPos != null ? startPos.rotation : Quaternion.identity;

        // Создаем персонажа
        GameObject player = Instantiate(prefab, spawnPosition, spawnRotation);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    // Обработчик выбора персонажа игроком
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("Жду сообщение от клиента с выбором персонажа...");
    }
}
