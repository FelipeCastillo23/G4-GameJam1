using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioBackground;
    public AudioSource audioButton;
    // Start is called before the first frame update
    void Start()
    {
        audioBackground.Play();
    }

    public void playButton()
    {
        audioBackground.Stop();
        audioButton.Play();
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);

    }

    public void ControlButton()
    {
        audioButton.Play();

    }


    public void Cerrar()
    {
        Application.Quit();
    }

    public void Pausa()
    {

        Time.timeScale = 0f;
        audioButton.Play();
    }

    public void Reanudar()
    {
        audioButton.Play();
        Time.timeScale = 1f;

    }
}
