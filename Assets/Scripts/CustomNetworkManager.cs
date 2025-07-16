using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public GameObject foxPrefab;
    public GameObject birdPrefab;

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<AddPlayerMessageWithSelection>(OnCreateCharacter);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, AddPlayerMessageWithSelection msg)
    {
        GameObject prefab = msg.character == "Bird" ? birdPrefab : foxPrefab;

        if (prefab == null)
        {
            Debug.LogError("❌ Prefab не назначен! Проверь foxPrefab и birdPrefab в инспекторе.");
            return;
        }

        Transform startPos = GetStartPosition();
        Vector3 spawnPosition = startPos != null ? startPos.position : Vector3.zero + new Vector3(0, 2, 0);
        Quaternion spawnRotation = startPos != null ? startPos.rotation : Quaternion.identity;

        GameObject player = Instantiate(prefab, spawnPosition, spawnRotation);
        NetworkServer.AddPlayerForConnection(conn, player);
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Отключаем стандартную систему добавления игрока
        Debug.Log("Жду сообщение от клиента с выбором персонажа...");
    }
}
