using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRandomizer : MonoBehaviour
{
    public BoxCollider2D FoodArea;
    public SnakeMovement snakeMovement; // Yýlanýn hareketini kontrol eden scriptin referansý

    public void Start()
    {
        FoodSpawner();
    }

    public void FoodSpawner()
    {
        Bounds bounds = this.FoodArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        Vector3 spawnPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);

        // Eðer yiyecek nesnesi yýlanýn segmentleriyle çakýþýyorsa, tekrar spawnlamak için yeni bir konum seç
        while (IsOverlappingWithSnake(spawnPosition))
        {
            x = Random.Range(bounds.min.x, bounds.max.x);
            y = Random.Range(bounds.min.y, bounds.max.y);
            spawnPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
        }

        this.transform.position = spawnPosition;
    }

    // Yiyecek nesnesinin yýlanýn segmentleriyle çakýþýp çakýþmadýðýný kontrol eden fonksiyon
    private bool IsOverlappingWithSnake(Vector3 position)
    {
        foreach (Transform segment in snakeMovement ._segments)
        {
            if (Vector3.Distance(position, segment.position) < 0.5f) // Uygun bir mesafe seçebilirsiniz
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FoodSpawner();
        }
    }
}
