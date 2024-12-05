using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject mainMenuPanel_Ref;
    public GameObject settingPanel_Ref;
    public GameObject gameOverPanel_Ref;
    public GameObject gameWinPanel_Ref;
    public GameObject pausemenuPanel_Ref;


    public GameObject PrevPlayer;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            Time.timeScale = 0f;
        }
        else { 
            Time.timeScale = 1f;
        }
    }

    public void OnClick_PlayButton()
    {
        Time.timeScale = 1f;
        mainMenuPanel_Ref.SetActive(false);
        SoundManager.instance.mainMenuAudio.Stop();
        SoundManager.instance.gamePlayAudio.Play();
    }

    public void OnClick_SettingButton()
    {
        settingPanel_Ref.SetActive(true);
    }

    public void OnClick_PauseButton()
    {
        pausemenuPanel_Ref.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnClick_ResumeButton()
    {
        pausemenuPanel_Ref.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClick_GameRestartButton()
    {
        SceneManager.LoadScene(0);
        gameOverPanel_Ref.SetActive(false);
        gameWinPanel_Ref.SetActive(false);
        Destroy(PrevPlayer.gameObject);
    }


}
