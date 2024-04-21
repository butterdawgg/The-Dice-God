using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject levelSelectMenu;
    [Header("MainMenu")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [Header("Settings")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button settingsBackButton;
    [Header("Level Select")]
    [SerializeField] private Button levelSelectBackButton;
    [Header("Transition")]
    [SerializeField] private Image transitionImage;
    [SerializeField] private float sceneTransitionTime;
    [SerializeField] private float menuTransitionTime;

    private float transitionOpacity = 1f;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        settingsBackButton.onClick.AddListener(OnBackButtonClick);
        levelSelectBackButton.onClick.AddListener(OnBackButtonClick);
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderValueChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeSliderValueChanged);

        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        levelSelectMenu.SetActive(false);

        StartCoroutine(TransitionToTransparent());
    }

    private void Update()
    {
        if(transitionOpacity <= 0f)
            transitionImage.gameObject.SetActive(false);
        else
            transitionImage.gameObject.SetActive(true);

        transitionImage.color = new Color(0.19607f, 0.19607f, 0.19607f, transitionOpacity);

        masterVolumeSlider.value = SerializeManager.Instance.GetFloat(FloatType.MasterVolume);
        musicVolumeSlider.value = SerializeManager.Instance.GetFloat(FloatType.MusicVolume);
        sfxVolumeSlider.value = SerializeManager.Instance.GetFloat(FloatType.SfxVolume);
    }

    private void OnPlayButtonClick()
    {
        StartCoroutine(ChangeMenuCoroutine(levelSelectMenu, mainMenu, settingsMenu));
    }

    private void OnSettingsButtonClick()
    {
        StartCoroutine(ChangeMenuCoroutine(settingsMenu, levelSelectMenu, mainMenu));
    }

    private void OnBackButtonClick()
    {
        StartCoroutine(ChangeMenuCoroutine(mainMenu, settingsMenu, levelSelectMenu));
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }

    private void OnMasterVolumeSliderValueChanged(float value)
    {
        SerializeManager.Instance.SetFloat(FloatType.MasterVolume, value);
    }

    private void OnMusicVolumeSliderValueChanged(float value)
    {
        SerializeManager.Instance.SetFloat(FloatType.MusicVolume, value);
    }

    private void OnSfxVolumeSliderValueChanged(float value)
    {
        SerializeManager.Instance.SetFloat(FloatType.SfxVolume, value);
    }

    public void ChangeScene(int sceneID)
    {
        StartCoroutine(ChangeSceneCoroutine(sceneID));
    }

    private IEnumerator ChangeSceneCoroutine(int sceneID)
    {
        while (transitionOpacity < 1f)
        {
            transitionOpacity += 0.01f / (sceneTransitionTime / 2f);
            yield return new WaitForSeconds(0.01f);
        }
        transitionOpacity = 1f;

        yield return new WaitForSeconds(sceneTransitionTime / 2f);

        SceneManager.LoadScene(sceneID);
    }

    private IEnumerator ChangeMenuCoroutine(GameObject setActiveTrue, GameObject setActiveFalse1, GameObject setActiveFalse2)
    {
        transitionOpacity = 0f;
        while (transitionOpacity < 1f)
        {
            transitionOpacity += 0.01f / (menuTransitionTime / 3f);
            yield return new WaitForSeconds(0.01f);
        }
        transitionOpacity = 1f;

        setActiveTrue.SetActive(true);
        setActiveFalse1.SetActive(false);
        setActiveFalse2.SetActive(false);

        yield return new WaitForSeconds(menuTransitionTime / 3f);

        while (transitionOpacity > 0f)
        {
            transitionOpacity -= 0.01f / (menuTransitionTime / 3f);
            yield return new WaitForSeconds(0.01f);
        }
        transitionOpacity = 0f;
    }

    private IEnumerator TransitionToTransparent()
    {
        yield return new WaitForSeconds(sceneTransitionTime / 2f);

        transitionOpacity = 1f;
        while (transitionOpacity > 0f)
        {
            transitionOpacity -= 0.01f / (sceneTransitionTime / 2f);
            yield return new WaitForSeconds(0.01f);
        }
        transitionOpacity = 0f;
    }
}
