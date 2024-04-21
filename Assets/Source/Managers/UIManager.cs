using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private Button quitToMenuButton;
    [SerializeField] private Button quitToMenuButton1;
    [SerializeField] private Button quitToMenuButton2;
    [SerializeField] private Button backButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Image transitionImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private float sceneTransitionTime;
    [SerializeField] private float menuTransitionTime;

    private float transitionOpacity = 1f;

    public static bool IsPaused { get; private set; } = false;
    public static bool IsTransitioning { get; private set; } = false;

    private void Awake()
    {
        quitToMenuButton.onClick.AddListener(QuitToMenu);
        quitToMenuButton1.onClick.AddListener(QuitToMenu);
        quitToMenuButton2.onClick.AddListener(QuitToMenu);
        backButton.onClick.AddListener(OnBackButtonClick);
        restartButton.onClick.AddListener(Restart);
        nextLevelButton.onClick.AddListener(NextLevel);

        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        victoryScreen.SetActive(false);

        backgroundImage.gameObject.SetActive(false);

        StartCoroutine(TransitionToTransparent());
    }

    private void Update()
    {
        if (transitionOpacity <= 0f)
            transitionImage.gameObject.SetActive(false);
        else
            transitionImage.gameObject.SetActive(true);

        transitionImage.color = new Color(0.19607f, 0.19607f, 0.19607f, transitionOpacity);

        if (!IsTransitioning & !IsPaused & Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void Resume()
    {
        StartCoroutine(ResumeCoroutine());
    }

    private void Pause()
    {
        StartCoroutine(PauseCoroutine());
    }

    public void ShowDeathScreen()
    {
        StartCoroutine(ShowDeathScreenCoroutine());
    }

    public void ShowVictoryScreen()
    {
        StartCoroutine(ShowVictoryScreenCoroutine());
    }

    public void QuitToMenu()
    {
        if (!IsTransitioning)
        {
            ChangeScene(0);
        }
    }

    public void Restart()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < 7)
            ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            ChangeScene(0);
    }

    public void OnBackButtonClick()
    {
        Resume();
    }

    public void ChangeScene(int sceneID)
    {
        StartCoroutine(ChangeSceneCoroutine(sceneID));
    }

    private IEnumerator ChangeSceneCoroutine(int sceneID)
    {
        Time.timeScale = 1f;
        IsTransitioning = true;

        transitionOpacity = 0f;
        while (transitionOpacity < 1f)
        {
            transitionOpacity += 0.01f / (sceneTransitionTime / 2f);
            yield return new WaitForSeconds(0.01f);
        }
        transitionOpacity = 1f;

        backgroundImage.gameObject.SetActive(true);

        Cursor.visible = true;
        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        victoryScreen.SetActive(false);

        yield return new WaitForSeconds(sceneTransitionTime / 2f);

        SceneManager.LoadScene(sceneID);

        IsTransitioning = false;
    }

    private IEnumerator TransitionToTransparent()
    {
        if (!IsTransitioning)
        {
            Time.timeScale = 1f;
            IsTransitioning = true;

            yield return new WaitForSeconds(sceneTransitionTime / 2f);

            transitionOpacity = 1f;
            while (transitionOpacity > 0f)
            {
                transitionOpacity -= 0.01f / (sceneTransitionTime / 2f);
                yield return new WaitForSeconds(0.01f);
            }
            transitionOpacity = 0f;

            IsTransitioning = false;
        }
    }

    private IEnumerator PauseCoroutine()
    {
        if (!IsTransitioning)
        {
            IsTransitioning = true;

            transitionOpacity = 0f;
            while (transitionOpacity < 1f)
            {
                transitionOpacity += 0.01f / (menuTransitionTime / 3f);
                yield return new WaitForSeconds(0.01f);
            }
            transitionOpacity = 1f;

            deathScreen.SetActive(false);
            victoryScreen.SetActive(false);
            pauseMenu.SetActive(true);
            Cursor.visible = true;
            IsPaused = true;

            yield return new WaitForSeconds(menuTransitionTime / 3f);

            while (transitionOpacity > 0f)
            {
                transitionOpacity -= 0.01f / (menuTransitionTime / 3f);
                yield return new WaitForSeconds(0.01f);
            }
            transitionOpacity = 0f;

            IsTransitioning = false;
            Time.timeScale = 0f;
        }
    }

    private IEnumerator ResumeCoroutine()
    {
        if (!IsTransitioning)
        {
            Time.timeScale = 1f;

            IsTransitioning = true;

            transitionOpacity = 0f;
            while (transitionOpacity < 1f)
            {
                transitionOpacity += 0.01f / (menuTransitionTime / 3f);
                yield return new WaitForSeconds(0.01f);
            }
            transitionOpacity = 1f;

            pauseMenu.SetActive(false);
            Cursor.visible = false;
            IsPaused = false;

            yield return new WaitForSeconds(menuTransitionTime / 3f);

            while (transitionOpacity > 0f)
            {
                transitionOpacity -= 0.01f / (menuTransitionTime / 3f);
                yield return new WaitForSeconds(0.01f);
            }
            transitionOpacity = 0f;

            IsTransitioning = false;
        }
    }

    private IEnumerator ShowDeathScreenCoroutine()
    {
        IsTransitioning = true;

        while (transitionOpacity < 1f)
        {
            transitionOpacity += 0.01f / (sceneTransitionTime / 3f);
            yield return new WaitForSeconds(0.01f);
        }
        transitionOpacity = 1f;

        pauseMenu.SetActive(false);
        victoryScreen.SetActive(false);
        deathScreen.SetActive(true);
        Cursor.visible = true;
        IsPaused = true;

        yield return new WaitForSeconds(sceneTransitionTime / 3f);

        while (transitionOpacity > 0f)
        {
            transitionOpacity -= 0.01f / (sceneTransitionTime / 3f);
            yield return new WaitForSeconds(0.01f);
        }
        transitionOpacity = 0f;

        IsTransitioning = false;
        Time.timeScale = 0f;
    }

    private IEnumerator ShowVictoryScreenCoroutine()
    {
        IsTransitioning = true;

        while (transitionOpacity < 1f)
        {
            transitionOpacity += 0.01f / (sceneTransitionTime / 3f);
            yield return new WaitForSeconds(0.01f);
        }
        transitionOpacity = 1f;

        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        victoryScreen.SetActive(true);
        Cursor.visible = true;
        IsPaused = true;

        yield return new WaitForSeconds(sceneTransitionTime / 3f);

        while (transitionOpacity > 0f)
        {
            transitionOpacity -= 0.01f / (sceneTransitionTime / 3f);
            yield return new WaitForSeconds(0.01f);
        }
        transitionOpacity = 0f;

        IsTransitioning = false;
        Time.timeScale = 0f;
    }
}