using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI elementlerine eriþmek için gerekli kütüphane

public class HardSnakeMovement : MonoBehaviour
{
    private float initialTimeScale; // Baþlangýçta kaydedilecek zaman ölçeði deðeri
    private Vector2 _direction = Vector2.zero;
    public List<Transform> _segments;
    public Button RestartGame; // StartGame butonuna referans
    public Text scoreText, highScoreText; // Text elementine referans
    public GameObject BigFood;
    private int highScore = 0; // Baþlangýçta en yüksek skor sýfýr olacak
    private FoodRandomizer foodRandomizer;
    public SoundEffect soundEffect;
    public HardGameModeScript hardGameModeScript;
    public void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
        RestartGame.gameObject.SetActive(false);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();
        BigFood.SetActive(false);
        // foodRandomizer deðiþkenini baþlat
        foodRandomizer = FindObjectOfType<FoodRandomizer>(); // FoodRandomizer scriptini sahnedeki nesneler arasýnda bul
        initialTimeScale = Time.timeScale; // Baþlangýç zaman ölçeði deðerini kaydet
    }

    public void GameStarter()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }

    public void GameRestarter()
    {
        RestartGame.gameObject.SetActive(false);
        Time.timeScale = initialTimeScale; // Baþlangýç hýzýna geri dön
        hardGameModeScript.previousScore = 0;
    }

    public Transform segmentPrefab;

    private void Update()
    {
    }
    // Android için kontrolleri buttonlara atamak için oluþturuldu
    public void SnakeGoRight()
    {
        // Eðer mevcut yön sol deðilse, saða hareket etmeye izin ver
        if (_direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
    }

    public void SnakeGoLeft()
    {
        // Eðer mevcut yön sol deðilse, saða hareket etmeye izin ver
        if (_direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
    }

    public void SnakeGoUp()
    {
        // Eðer mevcut yön sol deðilse, saða hareket etmeye izin ver
        if (_direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
    }

    public void SnakeGoDown()
    {
        // Eðer mevcut yön sol deðilse, saða hareket etmeye izin ver
        if (_direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
        Vector3 newPosition = transform.position + new Vector3(_direction.x * 0.5f, _direction.y * 0.5f, 0f);

        // Her bileþeni 0.5 birimden tam sayýya yuvarlayarak yýlanýn her adýmda 0.5 birim hareket etmesini saðlar
        float newX = Mathf.Round(newPosition.x * 2) / 2;
        float newY = Mathf.Round(newPosition.y * 2) / 2;

        transform.position = new Vector3(newX, newY, 0f);
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    public void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(this.transform);
        this.transform.position = Vector3.zero;
        RestartGame.gameObject.SetActive(true);
        BigFood.SetActive(false);
        foodRandomizer.SpawnCount = 0;
        foodRandomizer.FoodSpawner();
        _direction = Vector2.zero; //yandýktan sonra head objesinin sabit kalmasý için

    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString(); // Text elementini güncelle
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }
    }

    public int score = 0; // Baþlangýçta score deðeri

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            soundEffect.PlayFoodEatSound();
            Grow();
            score++; // Score deðerini arttýr
            UpdateScoreText(); // Score deðerini güncelleyerek ekrana yazdý
        }
        else if (other.tag == "Obstacle")
        {
            soundEffect.PlayWallCrashSound();
            ResetState();
            score = 0; // Score deðerini sýfýrla
            UpdateScoreText(); // Score deðerini güncelleyerek ekrana yazdýr
            Time.timeScale = 0;

        }
        else if (other.tag == "BigFood")
        {
            // BigFood tetiklendiðinde 4 kez Grow fonksiyonunu çaðýr ve skora +4 ekle
            for (int i = 0; i < 4; i++)
            {
                Grow();
                score++;
                UpdateScoreText();
            }
            soundEffect.PlayBigFoodEatSound();
            BigFood.SetActive(false);//yem yendikten sonra sabit kalmamasý için
        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }
}
