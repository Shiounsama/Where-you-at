using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CheckPNJSelected : NetworkBehaviour
{
    private PlayerData _playerData;

    public IsoCameraSelection cameraSelection;

    public PlayerScoring score;

    public ScoreGame scoreGame;

    private SeekerView _seekerView;

    private SeekerView seekerView
    {
        get
        {
            if (_seekerView == null)
                _seekerView = ViewManager.Instance.GetView<SeekerView>();

            return _seekerView;
        }
        set
        {
            _seekerView = value;
        }
    }

    private void Awake()
    {
        cameraSelection = GetComponentInChildren<IsoCameraSelection>();
        score = GetComponentInChildren<PlayerScoring>();
        scoreGame = FindObjectOfType<ScoreGame>();
        _playerData = GetComponent<PlayerData>();
        
    }

    public void IsGuess()
    {
        //NetworkServer.Spawn(cameraSelection.selectedObject.gameObject);
        float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
        Vector3 testPNJ = new Vector3();

        if (isLocalPlayer)
        {
            _playerData = GetComponent<PlayerData>();
            testPNJ = cameraSelection.selectedObject.localPosition;
            cameraSelection.OnObjectUnselected();

            _playerData.setPNJvalide(testPNJ);
        }

        score.ServeurScore(TestZoneNumber(), resultat, testPNJ);

        _playerData.testPNJ();
        seekerView.guessButton.gameObject.SetActive(false);

        foreach (NetworkConnection conn in NetworkServer.connections.Values)
        {
            timerTo30(conn);
        }
    }

    [TargetRpc]
    private void timerTo30(NetworkConnection conn)
    {
        timer timerScript = FindObjectOfType<timer>();

        if (timerScript.time > 30)
            timerScript.time = 30;
    }

    private bool TestZoneNumber()
    {
        bool testZone = false;

        if (isLocalPlayer)
        {
            GameObject player = cameraSelection.selectedObject.gameObject;
            RaycastHit hit;

           

            if (Physics.Raycast(player.transform.position, player.transform.TransformDirection(Vector3.down), out hit, 100f))
            {
                if (hit.collider.CompareTag("Map"))
                {
                    cityNumber cityNum = hit.collider.GetComponent<cityNumber>();
                    if (cityNum == null)
                    {
                        cityNum = hit.collider.GetComponentInParent<cityNumber>();
                    }

                    if (cityNum.zone == FindObjectOfType<CityManager>()._plateformWhereHiderIsIn)
                    {
                        testZone = true;
                    }
                }
            }
        }

        return testZone;
    }
}