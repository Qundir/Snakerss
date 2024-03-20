using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void LoadClassicModScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadHomeScene()
    {
        SceneManager.LoadScene(0);
    }
}
