using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private Image m_VibrationImageButton;
    [SerializeField] private Sprite m_VibrationOn;
    [SerializeField] private Sprite m_VibrationOff;
    [SerializeField] private Image m_SoundImageButton;
    [SerializeField] private Sprite m_SoundOn;
    [SerializeField] private Sprite m_SoundOff;

    private void Start()
    {
        if(m_VibrationImageButton != null)
        {
            int isVibrationOn = PlayerPrefs.GetInt("Vibration");
            if (isVibrationOn == 1)
            {
                m_VibrationImageButton.sprite = m_VibrationOff;
            }
        }

        if(m_SoundImageButton != null)
        {
            int isSoundOn = PlayerPrefs.GetInt("Sound");
            if (isSoundOn == 1)
            {
                m_SoundImageButton.sprite = m_SoundOff;
            }
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartCurrentLevel(float delay)
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(int buildNumber)
    {
        SceneManager.LoadScene(buildNumber);
    }

    public void LoadLevel(string sceneName)
    {
        Debug.Log("Loading " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleVibration()
    {
        int isVibrationOn = PlayerPrefs.GetInt("Vibration");
        if (isVibrationOn == 0)
        {
            m_VibrationImageButton.sprite = m_VibrationOff;
            PlayerPrefs.SetInt("Vibration", 1);
        }
        else
        {
            m_VibrationImageButton.sprite = m_VibrationOn;
            PlayerPrefs.SetInt("Vibration", 0);
        }
    }

    public void ToggleSound()
    {
        int isSoundOn = PlayerPrefs.GetInt("Sound");
        if (isSoundOn == 0)
        {
            m_SoundImageButton.sprite = m_SoundOff;
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            m_SoundImageButton.sprite = m_SoundOn;
            PlayerPrefs.SetInt("Sound", 0);
        }
    }
}
