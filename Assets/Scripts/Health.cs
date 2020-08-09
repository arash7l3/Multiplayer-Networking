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

    public void TakeDamage(int amount)
    {

        if (!isServer)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = MAXHEALTH;

            // called on the Server, but invoked on the Clients
            RpcRespwn();
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
            // move back to zero location
            transform.position = Vector3.zero;
        }
    }
}
