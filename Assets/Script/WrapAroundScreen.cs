using UnityEngine;

public class WrapAroundArea : MonoBehaviour
{
    private float minX = -7f; // Minumum X koordinatý
    private float maxX = 7;  // Maksimum X koordinatý
    private float minY = -5.25f; // Minumum Y koordinatý
    private float maxY = 11.25f;  // Maksimum Y koordinatý

    private void Update()
    {
        // Yýlanýn pozisyonunu kontrol et
        Vector3 currentPosition = transform.position;

        // Eðer yýlan sýnýrlarýn dýþýna çýkarsa, sýnýrlar içinde kalmasýný saðla
        currentPosition.x = WrapAround(currentPosition.x, minX, maxX);
        currentPosition.y = WrapAround(currentPosition.y, minY, maxY);

        // Yýlanýn pozisyonunu güncelle
        transform.position = currentPosition;
    }

    // Belirli bir koordinatýn sýnýrlar içine sýðdýrýlmasýný saðlayan fonksiyon
    private float WrapAround(float value, float min, float max)
    {
        // Eðer deðer sýnýrlarýn dýþýna çýkarsa, diðer tarafa taþý
        if (value < min)
        {
            return max - (min - value);
        }
        else if (value > max)
        {
            return min + (value - max);
        }
        else
        {
            return value;
        }
    }
}
