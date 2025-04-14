using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    public AudioSource bgmSource;
    public Toggle musicToggle;

    private static BGMController instance;
    private bool toggleInitialized = false;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        bool musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        if (musicOn)
            bgmSource.Play();
        else
            bgmSource.Pause();
    }

    void Start()
    {
        if (musicToggle == null)
        {
            GameObject foundToggle = GameObject.Find("MusicToggle");
            if (foundToggle != null)
            {
                musicToggle = foundToggle.GetComponent<Toggle>();
            }
        }

        if (musicToggle != null)
        {
            musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
            musicToggle.onValueChanged.AddListener(OnToggleMusic);
        }
    }

    void Update()
    {
        if (musicToggle == null)
        {
            GameObject foundToggle = GameObject.Find("MusicToggle");
            if (foundToggle != null)
            {
                musicToggle = foundToggle.GetComponent<Toggle>();

                musicToggle.onValueChanged.RemoveListener(OnToggleMusic);

                musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
                musicToggle.onValueChanged.AddListener(OnToggleMusic);
            }
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
