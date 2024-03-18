using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI elementlerine eriþmek için gerekli kütüphane

public class SnakeMovement : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    public List<Transform> _segments;
    public Button RestartGame; // StartGame butonuna referans
    public Text scoreText, highScoreText; // Text elementine referans

    private int highScore = 0; // Baþlangýçta en yüksek skor sýfýr olacak

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
        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = Vector2.right;
        }
    }

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
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
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
        scoreText.text = "Score: " + score.ToString(); // Text elementini güncelle
    }

    private int score = 0; // Baþlangýçta score deðeri

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
            score++; // Score deðerini arttýr
            UpdateScoreText(); // Score deðerini güncelleyerek ekrana yazdý
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
            score = 0; // Score deðerini sýfýrla
            UpdateScoreText(); // Score deðerini güncelleyerek ekrana yazdýr
            Time.timeScale = 0;

        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }

}
