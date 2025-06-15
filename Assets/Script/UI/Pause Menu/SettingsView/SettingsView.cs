using SoundDesign;
using UnityEngine;
using UnityEngine.UI;

public class SettingsView : View
{
    [Header("Settings View")]
    [SerializeField] private Button toggleSoundButton;

    [Header("Sliders")]
    [SerializeField] private Slider soundSliderMaster;
    [SerializeField] private Slider soundSliderMusic, soundSliderSFX;

    [Header("Toggle sound button sprites")]
    [SerializeField] private Sprite enabledSoundSprite;
    [SerializeField] private Sprite disabledSoundSprite;

    private Image _toggleSoundButtonLogoImage;

    private bool _isSoundEnabled = true;

    public override void Awake()
    {
        base.Awake();

        _toggleSoundButtonLogoImage = toggleSoundButton.transform.GetChild(0).GetComponent<Image>();

        ToggleSoundButtonLogoImage();

        // Initialize buttons

        toggleSoundButton.onClick.AddListener(OnClick_ToggleSound);

        // Initialize sliders

        soundSliderMaster.onValueChanged.AddListener(OnValueChanged_SoundSliderMaster);
        soundSliderMusic.onValueChanged.AddListener(OnValueChanged_SoundSliderMusic);
        soundSliderSFX.onValueChanged.AddListener(OnValueChanged_SoundSliderSFX);
    }

    private void ToggleSoundButtonLogoImage()
    {
        _toggleSoundButtonLogoImage.sprite = _isSoundEnabled ? enabledSoundSprite : disabledSoundSprite;
    }

    #region Button events
    private void OnClick_ToggleSound()
    {
        _isSoundEnabled = !_isSoundEnabled;
        ToggleSoundButtonLogoImage();
    }

    public override void OnClick_Return()
    {
        base.OnClick_Return();

        ViewManager.Instance.Show<PauseMenuView>();
    }
    #endregion

    #region Slider events
    private void OnValueChanged_SoundSliderMaster(float value) { SoundMixerManager.Instance.SetMasterVolume(value); }

    private void OnValueChanged_SoundSliderMusic(float value) { SoundMixerManager.Instance.SetMusicVolume(value); }

    private void OnValueChanged_SoundSliderSFX(float value) { SoundMixerManager.Instance.SetSFXVolume(value); }
    #endregion
}
