using UnityEngine;

public class HardGameModeScript : MonoBehaviour
{
    public HardSnakeMovement hardSnakeMovement;
    public int previousScore = 0; // Önceki puaný tutmak için

    void Start()
    {
        InvokeRepeating("ScoreCheck", 0f, 3f);
    }
    void ScoreCheck()
    {
        if (hardSnakeMovement != null && hardSnakeMovement.score % 3 == 0 && hardSnakeMovement.score != previousScore)
        {
            // Oyun hýzýný artýran fonksiyonu çaðýr
            IncreaseGameSpeed(0.1f); // %10 artýþ

            // Önceki skoru güncelle
            previousScore = hardSnakeMovement.score;
        }
    }
    private void IncreaseGameSpeed(float speedIncrease)
    {
        // Mevcut oyun hýzýný al
        float currentSpeed = Time.timeScale;

        // Yeni hýzý hesapla (mevcut hýzýn %10'u kadar artýr)
        float newSpeed = currentSpeed + (currentSpeed * speedIncrease);

        // Yeni hýzý uygula
        Time.timeScale = newSpeed;
    }
}