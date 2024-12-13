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

        scoreJoueur = scoreJoueur.Where(score => score.finish).OrderBy(scoreJoueur => scoreJoueur.ScoreFinal).ToList();

        AfficherClassement(scoreJoueur);
        
    }

    void AfficherClassement(List<scoringPlayer> scores)
    {
        classementCanvas.enabled = true;

        Transform parentTransform = classementCanvas.transform;

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
            textComponent.text = $"{i + 1} - {scores[i].playerName} avec {scores[i].ScoreFinal} points";

            textComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
            textComponent.fontSize = 24;
            textComponent.color = Color.black;
            textComponent.alignment = TextAnchor.MiddleCenter;

            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 30);
            rectTransform.anchoredPosition = new Vector2(0, -i * 35);
        }
    }
}
