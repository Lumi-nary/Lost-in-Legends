using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject volume;
    public GameObject mainMenu;
    
   
    public void Quit()
    {
        Application.Quit();
    }

    public void Start()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Back()
    {
        volume.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void Volume()
    {
        volume.SetActive(true);
        mainMenu.SetActive(false);
    }
}
