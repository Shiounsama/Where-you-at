using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.UI;

public class ScoreGame : NetworkBehaviour
{
    public List<scoringPlayer> scoreJoueur;
    public Canvas classementCanvas;
    public Transform parentTransform;
    public bool finish = false;

    private Button restartButton;
    public Button restartButtonPrefab;
    public GameObject BackgroundImage;

    


    public void showScore()
    {
        scoreJoueur = new List<scoringPlayer>(FindObjectsOfType<scoringPlayer>());
        scoreJoueur = scoreJoueur.Where(score => score.finish).OrderBy(scoreJoueur => scoreJoueur.ScoreFinal).ToList();

        AfficherClassement(scoreJoueur);
    }

    void AfficherClassement(List<scoringPlayer> scores)
    {
        classementCanvas.enabled = true;

        parentTransform = classementCanvas.transform;

        foreach (Transform child in parentTransform)
        {
            if (child.GetComponent<Text>() != null)
            {
                Destroy(child.gameObject);
            }
        }


        for (int i = 0; i < scores.Count; i++)
        {
            
            GameObject textObject = new GameObject($"Entry_{i + 1}");
            textObject.transform.SetParent(parentTransform);

            Text textComponent = textObject.AddComponent<Text>();
            textComponent.text = $"{i + 1} - {scores[i].transform.parent.GetComponentInChildren<PlayerData>().playerName} avec {scores[i].ScoreFinal} mètres";

            textComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
            textComponent.fontSize = 24;
            textComponent.color = Color.black;
            textComponent.alignment = TextAnchor.MiddleCenter;

            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 30);
            rectTransform.anchoredPosition = new Vector2(0, -i * 35);

            scores[i].GetComponent<PlayerData>().desactivatePlayer();
            scores[i].GetComponent<PlayerData>().ObjectsStateSetter(scores[i].GetComponent<PlayerData>().seekerObjects, false);
            scores[i].GetComponent<PlayerData>().ObjectsStateSetter(scores[i].GetComponent<PlayerData>().charlieObjects, false);

            BackgroundImage.SetActive(true);

        }
    }

    
}