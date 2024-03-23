using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager'ý ekleyin

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Yöneticiyi sahne deðiþikliklerinden koru
        }
        else
        {
            Destroy(gameObject); // Birden fazla kopyayý engelle
        }
    }

    // Tüm sesleri durdurma fonksiyonu
    public void StopAllSounds()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Sahne yüklendiðinde devam eden sesleri durdur
        StopAllSounds();
    }
}
