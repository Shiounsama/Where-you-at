using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GuessWaitingText : MonoBehaviour
{
    [SerializeField] private float timeBeforeFade = 1f;
    [SerializeField] private float fadeDuration = 5f;
    [SerializeField] private float moveUpSpeed = 1f;

    private CanvasGroup m_canvasGroup;

    private IEnumerator Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();

        GetComponent<TextMeshProUGUI>().text = $"Wait {30 - timer.Instance.GetPassedTime()} seconds before guess";

        yield return new WaitForSeconds(timeBeforeFade);

        m_canvasGroup.DOFade(0, fadeDuration).OnComplete(() => Destroy(gameObject));
    }

    private void Update()
    {
        transform.position += Vector3.up * moveUpSpeed;
    }
}
