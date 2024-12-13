using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.UI;

public class ScoreGame : NetworkBehaviour
{
    private List<scoringPlayer> scoreJoueur;
    public Canvas classementCanvas;

    private void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            showScore();
        }
    }

    public void showScore()
    {
        scoreJoueur = new List<scoringPlayer>(FindObjectsOfType<scoringPlayer>());

        scoreJoueur = scoreJoueur.OrderBy(scoreJoueur => scoreJoueur.ScoreFinal).ToList();


        /*foreach (scoringPlayer score in scoreJoueur)
        {
            Debug.Log("je suis " + score.playerName + " et j'ai fait un score de " + score.ScoreFinal);
        }*/

        classementCanvas.enabled = true;
    }

    void AfficherClassement(List<scoringPlayer> scores)
    {
        if (classementCanvas == null)
        {
            Debug.LogError("Canvas de classement non défini !");
            return;
        }

        Transform parentTransform = classementCanvas.transform;

        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < scores.Count; i++)
        {
            GameObject textObject = new GameObject($"Entry_{i + 1}");
            textObject.transform.SetParent(parentTransform);

            Text textComponent = textObject.AddComponent<Text>();
            textComponent.text = $"{i + 1} {scores[i].playerName} avec {scores[i].ScoreFinal} points";

            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = 24;
            textComponent.color = Color.black;
            textComponent.alignment = TextAnchor.MiddleCenter;

            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 30);
            rectTransform.anchoredPosition = new Vector2(0, -i * 35);
        }
    }
}
