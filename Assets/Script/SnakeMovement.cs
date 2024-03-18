using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI elementlerine eri�mek i�in gerekli k�t�phane

public class SnakeMovement : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    public List<Transform> _segments;
    public Button RestartGame; // StartGame butonuna referans
    public Text scoreText, highScoreText; // Text elementine referans

    private int highScore = 0; // Ba�lang��ta en y�ksek skor s�f�r olacak

    public void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
        RestartGame.gameObject.SetActive(false);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();
    }

    public void GameStarter()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }

    public void GameRestarter()
    {
        RestartGame.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public Transform segmentPrefab;

    private void Update()
    {
    }
    // Android i�in kontrolleri buttonlara atamak i�in olu�turuldu
    public void SnakeGoRight()
    {
        _direction = Vector2.right;
    }

    public void SnakeGoLeft()
    {
        _direction = Vector2.left;
    }

    public void SnakeGoUp()
    {
        _direction = Vector2.up;
    }

    public void SnakeGoDown()
    {
        _direction = Vector2.down;
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
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString(); // Text elementini g�ncelle
    }

    private int score = 0; // Ba�lang��ta score de�eri

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
            score++; // Score de�erini artt�r
            UpdateScoreText(); // Score de�erini g�ncelleyerek ekrana yazd�
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
                UpdateHighScoreText();
            }
        }
        else if (other.tag == "Obstacle")
        {
            ResetState();
            score = 0; // Score de�erini s�f�rla
            UpdateScoreText(); // Score de�erini g�ncelleyerek ekrana yazd�r
            Time.timeScale = 0;

        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }

}