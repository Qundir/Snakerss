using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI elementlerine eri�mek i�in gerekli k�t�phane

public class SnakeMovement : MonoBehaviour
{
    private float initialTimeScale;
    private Vector2 _direction = Vector2.zero;
    public Transform segmentPrefab;
    public List<Transform> _segments;
    public Button RestartGame;
    public Text scoreText, highScoreText;
    public GameObject BigFood;
    public int score = 0;
    private int highScore = 0;
    private FoodRandomizer foodRandomizer;
    public GameObject revivePanel;
    public int initialSize ;
    public void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();
        AudioManager.Instance.PlaySFX("GameStartSound");
        BigFood.SetActive(false);
        // foodRandomizer de�i�kenini ba�lat
        foodRandomizer = FindObjectOfType<FoodRandomizer>(); // FoodRandomizer scriptini sahnedeki nesneler aras�nda bul
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
        Time.timeScale = initialTimeScale; // Ba�lang�� h�z�na geri d�n
        _segments.Clear();
        _segments.Add(this.transform);
        this.transform.position = Vector3.zero;
        BigFood.SetActive(false);
        foodRandomizer.SpawnCount = 0;
        foodRandomizer.FoodSpawner();
        _direction = Vector2.zero; //yand�ktan sonra head objesinin sabit kalmas� i�in
        revivePanel.SetActive(false);
        score = 0;
        UpdateScoreText(); // Score de�erini g�ncelleyerek ekrana yazd�
    }
    public void ContinueFromLastCheckpoint()
    {
        initialSize = _segments.Count;
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        Time.timeScale = initialTimeScale; // Ba�lang�� h�z�na geri d�n
        _segments.Clear();
        _segments.Add(this.transform);
        for (int i = 1; i < this.initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }
        this.transform.position = new Vector3(0f, 0f, 0f);
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
            Debug.Log("yand�");
            Time.timeScale = 0f;
            revivePanel.SetActive(true);

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
