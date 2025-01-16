using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerData : NetworkBehaviour
{
    [SyncVar]
    public Role role = Role.None;

    [SyncVar]
    public string playerName;

    public GameObject PremierJoueurSpawn;
    public GameObject DeuxiemeJoueurSpawn;

    public List<GameObject> seekerObjects;
    public List<GameObject> charlieObjects;

    public static GameObject PNJcible { get; private set; }
    public GameObject PNJBESOIN;

    private void Update()
    {
        if (isLocalPlayer)
        {
            frontPNJ();
        }

        PNJBESOIN = PNJcible;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    /// <summary>
    /// Assigne un nouveau r�le au joueur.
    /// </summary>
    /// <param name="newRole">Nouveau r�le � assigner.</param>
    public void AssignRole(Role newRole)
    {
        role = newRole;
    }

    [Server]
    public void ServerSetRole(Role newRole)
    {
        role = newRole;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public void StartScene(PlayerData playerData)
    {
        if (isLocalPlayer)
        {
            PremierJoueurSpawn = GameObject.Find("spawn1");
            DeuxiemeJoueurSpawn = GameObject.Find("spawn2");

            ClearOtherTchat();
            EnablePlayer(role);
        }
    }

    public void frontPNJ()
    {
        if (isLocalPlayer)
        {
            GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");
            GameObject[] allPNJPI = GameObject.FindGameObjectsWithTag("pnj pi");

            foreach (GameObject obj in allPNJ)
            {
                obj.transform.LookAt(transform.position);
                Vector3 lockedRotation = obj.transform.eulerAngles;
                lockedRotation.x = 0;
                lockedRotation.z = 0;
                obj.transform.eulerAngles = lockedRotation;

                Rigidbody objRigid = obj.GetComponent<Rigidbody>();
                objRigid.constraints = RigidbodyConstraints.FreezePositionX;
                objRigid.constraints = RigidbodyConstraints.FreezePositionZ;

            }

            foreach (GameObject obj in allPNJPI)
            {
                obj.transform.LookAt(transform.position);
                Vector3 lockedRotation = obj.transform.eulerAngles;
                lockedRotation.x = 0;
                lockedRotation.z = 0;
                obj.transform.eulerAngles = lockedRotation;

                Rigidbody objRigid = obj.GetComponent<Rigidbody>();
                objRigid.constraints = RigidbodyConstraints.FreezePositionX;
                objRigid.constraints = RigidbodyConstraints.FreezePositionZ;

            }
        }
    }

    public void ClearOtherTchat()
    {
        if (isLocalPlayer)
        {
            TchatManager tchatGeneral = FindObjectOfType<TchatManager>();
            List<TchatPlayer> listTchat = new List<TchatPlayer>(FindObjectsOfType<TchatPlayer>());
            tchatGeneral.gameObject.GetComponentInChildren<Canvas>().enabled = true;

            tchatGeneral.clearTchat();

            foreach (TchatPlayer tchat in listTchat)
            {
                if (tchat.isLocalPlayer)
                {

                    tchat.gameObject.GetComponentInChildren<Canvas>().enabled = true;

                }
                else
                {

                    tchat.gameObject.GetComponentInChildren<Canvas>().enabled = false;
                }
            }
        }
    }

    public void StartGame()
    {
        CmdRequestSceneChange("VilleJeu");
    }

    [Command]
    void CmdRequestSceneChange(string SceneChange)
    {
        if (isServer)
        {
            NetworkManager.singleton.ServerChangeScene(SceneChange);
        }
    }

    public void ObjectsStateSetter(List<GameObject> listOfObjectToChangeState, bool setOnObject)
    {
        if (listOfObjectToChangeState.Count > 0)
        {
            for (int i = 0; i < listOfObjectToChangeState.Count; i++)
            {
                listOfObjectToChangeState[i].SetActive(setOnObject);
            }
        }
    }

    public void EnablePlayer(Role role)
    {
        IsoCameraDrag camDragIso = this.GetComponent<IsoCameraDrag>();
        IsoCameraRotation camRotaIso = this.GetComponent<IsoCameraRotation>();
        IsoCameraZoom camZoomIso = this.GetComponent<IsoCameraZoom>();

        Camera360 cam360 = this.GetComponent<Camera360>();

        Camera camPlayer = this.GetComponent<Camera>();

        if (role != Role.None)
        {
            GameObject building = GameObject.Find("monde"); 
            building.transform.position = new Vector3(0, 0, 0);

            this.GetComponent<PlayerInput>().enabled = false;

            cam360.enabled = false;

            camDragIso.enabled = false;
            camDragIso.objectToMove = building.transform;

            camZoomIso.enabled = false;

            camRotaIso.enabled = false;
            camRotaIso.objectToRotate = building.transform;

            camPlayer.enabled = true;

            GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");
            List<GameObject> ListPNJ = new List<GameObject>();
            foreach (GameObject obj in allPNJ)
            {
                ListPNJ.Add(obj);
            }

            int randomNumber = Random.Range(0, ListPNJ.Count);
            PNJcible = ListPNJ[randomNumber];

            if (role == Role.Seeker)
            {
                Debug.Log("Seeker view");
                SeekerView seekerView = transform.parent.GetComponentInChildren<SeekerView>(true);
                ViewManager.Instance.AddView(seekerView);
                ViewManager.Instance.GetView<SeekerView>().Initialize();

                ObjectsStateSetter(charlieObjects, false);
                ObjectsStateSetter(seekerObjects, true);

                ViewManager.Instance.Show<SeekerView>();

                camDragIso.enabled = true;
                camZoomIso.enabled = true;
                camRotaIso.enabled = true;

                this.GetComponent<PlayerInput>().enabled = true;
                camPlayer.orthographic = true;

                transform.position = DeuxiemeJoueurSpawn.transform.position;
                transform.rotation = DeuxiemeJoueurSpawn.transform.rotation;

            }

            else if (role == Role.Lost)
            {
                LostView lostView = transform.parent.GetComponentInChildren<LostView>(true);
                ViewManager.Instance.AddView(lostView);
                ViewManager.Instance.GetView<SeekerView>().Initialize();

                ObjectsStateSetter(charlieObjects, true);
                ObjectsStateSetter(seekerObjects, false);

                ViewManager.Instance.Show<LostView>();

                frontPNJ();
                cam360.enabled = true;
                camPlayer.orthographic = false;

                transform.position = PremierJoueurSpawn.transform.position;
                transform.rotation = PremierJoueurSpawn.transform.rotation;

                transform.position = new Vector3(PNJcible.transform.position.x, 1f, PNJcible.transform.position.z);
                transform.rotation = PNJcible.transform.rotation;
                Destroy(PNJcible);
            }
        }
    }

    public void DisablePlayer()
    {
        IsoCameraDrag camDragIso = this.GetComponent<IsoCameraDrag>();
        IsoCameraRotation camRotaIso = this.GetComponent<IsoCameraRotation>();
        IsoCameraZoom camZoomIso = this.GetComponent<IsoCameraZoom>();
        TchatManager tchatGeneral = FindObjectOfType<TchatManager>();
        Camera360 cam360 = this.GetComponent<Camera360>();

        this.GetComponent<PlayerInput>().enabled = false;

        cam360.enabled = false;

        camDragIso.enabled = false;

        camZoomIso.enabled = false;

        camRotaIso.enabled = false;
  
        tchatGeneral.gameObject.GetComponentInChildren<Canvas>().enabled = false;
    }

}

public enum Role
{
    Seeker,
    Lost,
    None
}