using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public GameObject volume;
    public GameObject mainMenu;
    public GameObject aboutUs;
    public Button playButton;
    public Button optionButton;
    public Button aboutButton;
    public Button quitButton;
    public Button backButton;

    public void Start()
    {
        AudioManager.Instance.PlayAmbient(AmbientKey.ForestAmbience);

        Button btn = playButton.GetComponent<Button>();
        Button btn2 = optionButton.GetComponent<Button>();
        Button btn3 = aboutButton.GetComponent<Button>();
        Button btn4 = quitButton.GetComponent<Button>();
        Button btn5 = backButton.GetComponent<Button>();

        btn.onClick.AddListener(GameStart);
        btn2.onClick.AddListener(Volume);
        btn3.onClick.AddListener(AboutUs);
        btn4.onClick.AddListener(Quit);
        btn5.onClick.AddListener(Back);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Back()
    {
        volume.SetActive(false);
        aboutUs.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void Volume()
    {
        volume.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void AboutUs()
    {
        aboutUs.SetActive(true);
        mainMenu.SetActive(false);
    }
}
