using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    public AudioSource bgmSource;
    public Toggle musicToggle;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        bool musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        if (!musicOn)
        {
            bgmSource.Pause();
        }
        else
        {
            bgmSource.Play();
        }
    }

    void Start()
    {
        if (musicToggle != null)
        {
            musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
            musicToggle.onValueChanged.AddListener(OnToggleMusic);
        }
    }

    void OnToggleMusic(bool isOn)
    {
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
        PlayerPrefs.Save();

        if (isOn)
            bgmSource.Play();
        else
            bgmSource.Pause();
    }
}
