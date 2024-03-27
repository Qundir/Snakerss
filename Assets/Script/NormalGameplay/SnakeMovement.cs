using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI elementlerine eriþmek için gerekli kütüphane

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
        // foodRandomizer deðiþkenini baþlat
        foodRandomizer = FindObjectOfType<FoodRandomizer>(); // FoodRandomizer scriptini sahnedeki nesneler arasýnda bul
        initialTimeScale = Time.timeScale; // Baþlangýç zaman ölçeði deðerini kaydet
    }

    public void GameStarter()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }


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
        Time.timeScale = initialTimeScale; // Baþlangýç hýzýna geri dön
        _segments.Clear();
        _segments.Add(this.transform);
        this.transform.position = Vector3.zero;
        BigFood.SetActive(false);
        foodRandomizer.SpawnCount = 0;
        foodRandomizer.FoodSpawner();
        _direction = Vector2.zero; //yandýktan sonra head objesinin sabit kalmasý için
        revivePanel.SetActive(false);
        score = 0;
        UpdateScoreText(); // Score deðerini güncelleyerek ekrana yazdý
    }
    public void ContinueFromLastCheckpoint()
    {
        initialSize = _segments.Count;
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        Time.timeScale = initialTimeScale; // Baþlangýç hýzýna geri dön
        _segments.Clear();
        _segments.Add(this.transform);
        for (int i = 1; i < this.initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }
        this.transform.position = new Vector3(0f, 0f, 0f);
        revivePanel.SetActive(false);

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            AudioManager.Instance.PlaySFX("FoodEatSound");
            Grow();
            score++; // Score deðerini arttýr
            UpdateScoreText(); // Score deðerini güncelleyerek ekrana yazdý
        }
        else if (other.tag == "Obstacle")
        {
            AudioManager.Instance.PlaySFX("WallCrushSound");
            Vibration.Vibrate(100);
            //UpdateScoreText(); // Score deðerini güncelleyerek ekrana yazdýr
            Debug.Log("yandý");
            Time.timeScale = 0f;
            revivePanel.SetActive(true);

        }
        else if (other.tag == "BigFood")
        {
            AudioManager.Instance.PlaySFX("BigFoodEatSound");
            // BigFood tetiklendiðinde 4 kez Grow fonksiyonunu çaðýr ve skora +4 ekle
            for (int i = 0; i < 4; i++)
            {
                Grow();
                score++;
                UpdateScoreText();
            }
            BigFood.SetActive(false);//yem yendikten sonra sabit kalmamasý için
        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }
}
