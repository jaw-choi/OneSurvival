using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Toggles")]
    [SerializeField] private Toggle hapticsToggle;
    [SerializeField] private Toggle shakeToggle;
    [SerializeField] private Toggle hitStopToggle;

    [Header("Optional Labels")]
    [SerializeField] private TextMeshProUGUI masterLabel;
    [SerializeField] private TextMeshProUGUI musicLabel;
    [SerializeField] private TextMeshProUGUI sfxLabel;

    void OnEnable()
    {
        var sm = SettingsManager.Instance;
        masterSlider.SetValueWithoutNotify(sm.Master);
        musicSlider.SetValueWithoutNotify(sm.Music);
        sfxSlider.SetValueWithoutNotify(sm.Sfx);

        hapticsToggle.SetIsOnWithoutNotify(sm.Haptics);
        shakeToggle.SetIsOnWithoutNotify(sm.ScreenShake);
        //hitStopToggle.SetIsOnWithoutNotify(sm.HitStop);

        UpdateLabels();
        HookEvents(true);
    }

    void OnDisable() { HookEvents(false); }

    private void HookEvents(bool on)
    {
        if (on)
        {
            masterSlider.onValueChanged.AddListener(OnMasterChanged);
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
            sfxSlider.onValueChanged.AddListener(OnSfxChanged);

            hapticsToggle.onValueChanged.AddListener(OnHaptics);
            shakeToggle.onValueChanged.AddListener(OnShake);
            //hitStopToggle.onValueChanged.AddListener(OnHitStop);

        }
        else
        {
            masterSlider.onValueChanged.RemoveListener(OnMasterChanged);
            musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
            sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);

            hapticsToggle.onValueChanged.RemoveListener(OnHaptics);
            shakeToggle.onValueChanged.RemoveListener(OnShake);
            //hitStopToggle.onValueChanged.RemoveListener(OnHitStop);

        }
    }

    private void OnMasterChanged(float v) { SettingsManager.Instance.SetMaster(v); UpdateLabels(); }
    private void OnMusicChanged(float v) { SettingsManager.Instance.SetMusic(v); UpdateLabels(); }
    private void OnSfxChanged(float v) { SettingsManager.Instance.SetSfx(v); UpdateLabels(); }

    private void OnHaptics(bool on) { SettingsManager.Instance.SetHaptics(on); }
    private void OnShake(bool on) { SettingsManager.Instance.SetScreenShake(on); }
    private void OnHitStop(bool on) { SettingsManager.Instance.SetHitStop(on); }

    private void UpdateLabels()
    {
        if (masterLabel) masterLabel.text = Mathf.RoundToInt(SettingsManager.Instance.Master * 100f) + "%";
        if (musicLabel) musicLabel.text = Mathf.RoundToInt(SettingsManager.Instance.Music * 100f) + "%";
        if (sfxLabel) sfxLabel.text = Mathf.RoundToInt(SettingsManager.Instance.Sfx * 100f) + "%";
    }

    public void OnClickClose() { gameObject.SetActive(false); }
}
