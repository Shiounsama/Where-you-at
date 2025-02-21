using SoundDesign;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ButtonAudio : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    protected SoundBankSO SoundBank { get; private set; }

    protected AudioClip buttonClickClip;
    protected AudioClip buttonHoverClip;

    public virtual void Start()
    {
        SoundBank = SoundFXManager.Instance.SoundBank;

        buttonHoverClip = SoundBank.buttonHoverClip;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        SoundFXManager.Instance.PlaySFXClip(buttonClickClip, transform, .7f);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        SoundFXManager.Instance.PlaySFXClip(buttonHoverClip, transform, .4f);
    }
}
