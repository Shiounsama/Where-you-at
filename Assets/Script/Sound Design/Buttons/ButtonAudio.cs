using SoundDesign;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ButtonAudio : MonoBehaviour, IPointerClickHandler
{
    protected SoundBankSO SoundBank { get; private set; }

    protected AudioClip buttonClickClip;

    public virtual void Start()
    {
        SoundBank = SoundFXManager.Instance.SoundBank;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        SoundFXManager.Instance.PlaySFXClip(buttonClickClip, transform, .7f);
    }
}
