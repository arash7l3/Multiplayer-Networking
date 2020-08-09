using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour
{
    public const int MAXHEALTH = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = MAXHEALTH;

    public RectTransform healthBar;
    public bool destroyOnDeath;

    private NetworkStartPosition[] spawnPoints;

    private void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int amount)
    {

        if (!isServer)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = MAXHEALTH;

                // called on the Server, but invoked on the Clients
                RpcRespwn();
            }
        }
    }

    private void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    private void RpcRespwn()
    {
        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick a spawn point at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;
        }
    }
}
