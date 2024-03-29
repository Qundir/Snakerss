using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFoodRandomizer : MonoBehaviour
{
    public BoxCollider2D BigFoodArea;
    public SnakeMovement snakeMovement; // Y�lan�n hareketini kontrol eden scriptin referans�
    public void BigFoodSpawner()
    {
        Bounds bounds = this.BigFoodArea.bounds;

        // 0.5 birimlik ad�mlarla rastgele bir x ve y konumu se�
        float x = Mathf.Round(Random.Range(bounds.min.x * 2, bounds.max.x * 2)) / 2;
        float y = Mathf.Round(Random.Range(bounds.min.y * 2, bounds.max.y * 2)) / 2;
        Vector3 spawnPosition = new Vector3(x, y, 0.0f);
        // E�er yiyecek nesnesi y�lan�n segmentleriyle �ak���yorsa, tekrar spawnlamak i�in yeni bir konum se�
        while (IsOverlappingWithSnake(spawnPosition))
        {
            // 0.5 birimlik ad�mlarla rastgele bir x ve y konumu se�
            x = Mathf.Round(Random.Range(bounds.min.x * 2, bounds.max.x * 2)) / 2;
            y = Mathf.Round(Random.Range(bounds.min.y * 2, bounds.max.y * 2)) / 2;
            spawnPosition = new Vector3(x, y, 0.0f);
        }
        this.transform.position = spawnPosition;
    }
    // Yiyecek nesnesinin y�lan�n segmentleriyle �ak���p �ak��mad���n� kontrol eden fonksiyon
    private bool IsOverlappingWithSnake(Vector3 position)
    {
        foreach (Transform segment in snakeMovement._segments)
        {
            if (Vector3.Distance(position, segment.position) < 1.0f) // Uygun bir mesafe se�ebilirsiniz
            {
                return true;
            }
        }
        return false;
    }
}
