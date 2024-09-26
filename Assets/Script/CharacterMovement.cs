using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterMovement : NetworkBehaviour
{
    public Rigidbody rb;
    public int speed = 10;
    public string role = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        if (Input.GetKeyDown(KeyCode.E))
        {
            CmdSendMessageToServer();
        }
    }

    [Command]
    void CmdSendMessageToServer()
    {
        Debug.Log("Bonjour je suis le joueur avec l'ID : " + netId);
        RpcShowMessageToAllClients("Bonjour je suis le joueur avec l'ID : " + netId);
    }

    [ClientRpc]
    void RpcShowMessageToAllClients(string message)
    {
        Debug.Log(message);
    }
}
