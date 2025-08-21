using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSettingsSO audioDefaults;

    [Header("#Mixer (assign in Inspector)")] //  NEW
    [SerializeField] private AudioMixer audioMixer;             // GameMixer
    [SerializeField] private AudioMixerGroup musicGroup;        // GameMixer/Music
    [SerializeField] private AudioMixerGroup sfxGroup;          // GameMixer/SFX

    // Exposed parameter names (AudioMixer�� Exposed�� ��Ȯ�� ��ġ)
    private const string PARAM_MASTER = "MasterVolume";
    private const string PARAM_MUSIC = "MusicVolume";
    private const string PARAM_SFX = "SFXVolume";

    [Header("#BGM")]
    public AudioClip bgmClip;
    [Range(0f, 1f)] public float bgmVolume = 1f; // pre-mix gain (����)
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    [Range(0f, 1f)] public float sfxVolume = 1f; // pre-mix gain (����)
    public int channels = 8;
    AudioSource[] sfxPlayers;
    int channelIndex;
    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win, GetItem}


    void Awake()
    {
        instance = this;
        Init();
        // �ʱ� Mixer ���� ����(���ϸ� PlayerPrefs���� �ҷ��� ����)
        if (audioDefaults != null)
        {
            SetMasterVolume01(audioDefaults.master);
            SetMusicVolume01(audioDefaults.music);
            SetSfxVolume01(audioDefaults.sfx);
        }

        // 2) SettingsManager�� ��� ������(��Ʈ��Ʈ�� ����) ���������� �����
        var sm = SettingsManager.Instance;
        if (sm != null)
        {
            SetMasterVolume01(sm.Master);
            SetMusicVolume01(sm.Music);
            SetSfxVolume01(sm.Sfx);
        }
    }
    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        // Mixer ����� 
        if (musicGroup != null) bgmPlayer.outputAudioMixerGroup = musicGroup;

        // ī�޶� HighPassFilter�� ���ٸ� null�� �� ����
        var cam = Camera.main;
        if (cam != null) bgmEffect = cam.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            //sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            //sfxPlayers[index].playOnAwake = false;
            //sfxPlayers[index].bypassListenerEffects = true;
            //sfxPlayers[index].volume = sfxVolume;
            var src = sfxObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.bypassListenerEffects = true;  // �������� ����Ʈ ���� ���� (����)
            src.volume = sfxVolume;            // pre-mix
            if (sfxGroup != null) src.outputAudioMixerGroup = sfxGroup; // Mixer ����� 
            sfxPlayers[index] = src;

        }
    }
    public void PlayBGM(bool isPlay)
    {
        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }
    public void EffectBGM(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            var src = sfxPlayers[loopIndex];
            if (src.isPlaying) continue;

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee) ranIndex = Random.Range(0, 2);

            channelIndex = loopIndex;
            src.clip = sfxClips[(int)sfx + ranIndex];
            src.Play();
            break;

        }
    }
    // === Volume via AudioMixer (�����̴� 0..1) ===
    public void SetMasterVolume01(float v01) => SetMixer01(PARAM_MASTER, v01);
    public void SetMusicVolume01(float v01) => SetMixer01(PARAM_MUSIC, v01);
    public void SetSfxVolume01(float v01) => SetMixer01(PARAM_SFX, v01);

    private void SetMixer01(string param, float v01)
    {
        if (audioMixer == null) return;
        float dB = Linear01ToDecibel(v01);
        audioMixer.SetFloat(param, dB);
    }

    // 0..1 -> dB(�α� ������). 0�̸� -80dB�� ��ǻ� mute
    private float Linear01ToDecibel(float v)
    {
        if (v <= 0.0001f) return -80f;
        return Mathf.Log10(Mathf.Clamp01(v)) * 20f;
    }

    // (����) pre-mix ������ �����̴��� ���� ������ �ʹٸ�:
    public void SetPreMixBgm(float v01)
    {
        bgmVolume = Mathf.Clamp01(v01);
        if (bgmPlayer != null) bgmPlayer.volume = bgmVolume;
    }
    public void SetPreMixSfx(float v01)
    {
        sfxVolume = Mathf.Clamp01(v01);
        if (sfxPlayers != null)
            for (int i = 0; i < sfxPlayers.Length; i++)
                if (sfxPlayers[i] != null) sfxPlayers[i].volume = sfxVolume;
    }

}
