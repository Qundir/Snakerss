using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeScaleController : MonoBehaviour
{
    private float defaultTimeScale = 1f; // Varsayýlan zaman ölçeði deðeri

    private void Start()
    {
        // Varsayýlan zaman ölçeði deðerini kaydedin
        defaultTimeScale = Time.timeScale;

        // SceneManager'ý sahne yükleme olaylarýna abone yapýn
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Aboneliði kaldýrýn
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Yeni bir sahne yüklendiðinde zaman ölçeðini varsayýlan deðere geri döndürün
        Time.timeScale = defaultTimeScale;
    }
}
