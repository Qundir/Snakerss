using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void LoadClassicModScene()
    {
        Time.timeScale = 1f;        
        SceneManager.LoadScene(1);
    }
    public void LoadHomeScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void LoadHardClassicModScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
}
