using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI elementlerine eri�mek i�in gerekli k�t�phane

public class HardSnakeMovement : MonoBehaviour
{
    private float initialTimeScale; // Ba�lang��ta kaydedilecek zaman �l�e�i de�eri
    private Vector2 _direction = Vector2.zero;
    public List<Transform> _segments;
    public Transform segmentPrefab;
    public Button RestartGame; // StartGame butonuna referans
    public Text scoreText, highScoreText; // Text elementine referans
    public GameObject BigFood;
    public int score = 0; // Ba�lang��ta score de�eri
    private int highScore = 0; // Ba�lang��ta en y�ksek skor s�f�r olacak
    private HardFoodRandomizer hardFoodRandomizer;
    public HardGameModeScript hardGameModeScript;
    private List<Vector3> checkpoints = new List<Vector3>(); // Oyuncunun kay�t noktalar�n� tutmak i�in bir liste
    public GameObject revivePanel;
    float ContinueSpeed;
    public int initialSize;
    public void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        AudioManager.Instance.PlaySFX("GameStartSound");
        UpdateHighScoreText();
        BigFood.SetActive(false);
        // foodRandomizer de�i�kenini ba�lat
        hardFoodRandomizer = FindObjectOfType<HardFoodRandomizer>(); // FoodRandomizer scriptini sahnedeki nesneler aras�nda bul
        initialTimeScale = Time.timeScale; // Ba�lang�� zaman �l�e�i de�erini kaydet
    }

    public void GameStarter()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }
    public void SnakeGoRight()
    {
        // E�er mevcut y�n sol de�ilse, sa�a hareket etmeye izin ver
        if (_direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
    }

    public void SnakeGoLeft()
    {
        // E�er mevcut y�n sol de�ilse, sa�a hareket etmeye izin ver
        if (_direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
    }

    public void SnakeGoUp()
    {
        // E�er mevcut y�n sol de�ilse, sa�a hareket etmeye izin ver
        if (_direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
    }

    public void SnakeGoDown()
    {
        // E�er mevcut y�n sol de�ilse, sa�a hareket etmeye izin ver
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

        // Her bile�eni 0.5 birimden tam say�ya yuvarlayarak y�lan�n her ad�mda 0.5 birim hareket etmesini sa�lar
        float newX = Mathf.Round(newPosition.x * 2) / 2;
        float newY = Mathf.Round(newPosition.y * 2) / 2;

        transform.position = new Vector3(newX, newY, 0f);
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
        checkpoints.Add(segment.position);
        ContinueSpeed = Time.timeScale;
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
        BigFood.SetActive(false);
        hardFoodRandomizer.SpawnCount = 0;
        hardFoodRandomizer.FoodSpawner();
        _direction = Vector2.zero; //yand�ktan sonra head objesinin sabit kalmas� i�in
        revivePanel.SetActive(false);
        checkpoints.Clear();
        score = 0;
        UpdateScoreText(); // Score de�erini g�ncelleyerek ekrana yazd�r
        Time.timeScale = 1f;

    }
    public void HardContinueFromLastCheckpoint()
    {
        initialSize = _segments.Count;
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(this.transform);
        Time.timeScale = ContinueSpeed;
        for (int i = 1; i < this.initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }
        this.transform.position = Vector3.zero;
        revivePanel.SetActive(false);
        _direction = Vector2.up;

    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString(); // Text elementini g�ncelle
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            AudioManager.Instance.PlaySFX("FoodEatSound");
            Grow();
            score++; // Score de�erini artt�r
            UpdateScoreText(); // Score de�erini g�ncelleyerek ekrana yazd�
        }
        else if (other.tag == "Obstacle")
        {
            AudioManager.Instance.PlaySFX("WallCrushSound");
            Vibration.Vibrate(100);
            //UpdateScoreText(); // Score de�erini g�ncelleyerek ekrana yazd�r
            revivePanel.SetActive(true);
            Time.timeScale = 0;

        }
        else if (other.tag == "BigFood")
        {
            AudioManager.Instance.PlaySFX("BigFoodEatSound");
            // BigFood tetiklendi�inde 4 kez Grow fonksiyonunu �a��r ve skora +4 ekle
            for (int i = 0; i < 4; i++)
            {
                Grow();
                score++;
                UpdateScoreText();
            }
            BigFood.SetActive(false);//yem yendikten sonra sabit kalmamas� i�in
        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }
}
