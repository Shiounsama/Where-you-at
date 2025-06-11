using DG.Tweening;
using TMPro;
using UnityEngine;

public class FindYourFriendView : View
{
    [Header("Find your friend")]
    [SerializeField] private Transform silhouettePanel;
    [SerializeField] private TextMeshProUGUI findYourFriendText;
    [SerializeField] private Transform arrowImage;

    [Header("Animation")]
    [SerializeField, Range(.1f, 1.5f)] private float moveDownAnimDuration = .5f;
    [SerializeField, Range(.1f, 1.5f)] private float textPopOutAnimDuration = .5f;
    [SerializeField, Range(1f, 4f)] private float moveRightAnimDelay = 2f;
    [SerializeField, Range(.1f, 1.5f)] private float moveRightAnimDuration = .5f;
    [SerializeField, Range(.1f, .5f)] private float arrowFadeAnimDuration = .5f;

    [Header("Arrow animation")]
    [SerializeField, Range(.1f, .5f)] private float waveAmplitude = .3f;
    [SerializeField, Range(5f, 30f)] private float waveFrequency = 30f;

    private CanvasGroup _arrowCanvasGroup;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }

    private void Awake()
    {
        _arrowCanvasGroup = arrowImage.GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _arrowCanvasGroup.alpha = 0f;

        findYourFriendText.transform.localScale = Vector3.zero;
        silhouettePanel.transform.localPosition = new Vector2(0, Screen.height);

        MoveDownAnim();
    }

    #region Animation
    private void MoveDownAnim()
    {
        silhouettePanel.DOLocalMoveY(0, moveDownAnimDuration)
            .SetEase(Ease.OutExpo)
            .OnComplete(TextPopOutAnim);
    }

    private void TextPopOutAnim()
    {
        findYourFriendText.transform.DOScale(Vector3.one, textPopOutAnimDuration)
            .SetEase(Ease.OutElastic)
            .OnComplete(() => 
        {
            MoveRightAnim();
            ArrowAnimation();
        });
    }

    private void MoveRightAnim()
    {
        silhouettePanel.transform.DOLocalMoveX(Screen.width, moveRightAnimDuration)
            .SetEase(Ease.InExpo)
            .SetDelay(moveRightAnimDelay);
    }

    private void ArrowAnimation()
    {
        _arrowCanvasGroup.DOFade(1f, arrowFadeAnimDuration);

        // Move along y axis following the sine wave
        DOVirtual.Float(1f, 1 + waveAmplitude, moveRightAnimDelay, (t) =>
        {
            // Calculate the new y position using the sine function and apply the shift to our og y
            Vector3 newScale = Vector3.one + Vector3.one * (waveAmplitude * Mathf.Sin(t * waveFrequency * Mathf.PI * 2));
            arrowImage.localScale = Vector3.one * waveAmplitude + newScale;
        })
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            _arrowCanvasGroup.DOFade(0, arrowFadeAnimDuration);
        });
    }
    #endregion
}
