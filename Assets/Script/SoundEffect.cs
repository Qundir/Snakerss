using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource FoodEatSound, BigFoodEatSound, WallCrashSound;

    public void PlayFoodEatSound()
    {
        FoodEatSound.Play();
    }

    public void PlayBigFoodEatSound()
    {
        BigFoodEatSound.Play();
    }

    public void PlayWallCrashSound()
    {
        WallCrashSound.Play();
    }
}
