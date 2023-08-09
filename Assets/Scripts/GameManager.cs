using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }
    public static bool isGameActive { get; private set; }
    public static bool isInTransit { get; private set; }

    public static bool isMenuOnMain = true;

    [SerializeField] private Transition transition;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private TextMeshProUGUI levelNumberText;

    private Vector2 startingPos = new Vector2(-1.6f, -0.8f);

    public static int level = 1;

    private bool isGameCompleted = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        isGameActive = true;
        
        StartCoroutine(PlayBGM());
    }
    private void Update()
    {
        if (((Input.GetButtonDown("Restart") && isGameActive) || player.transform.position.y < -3) && 
            !SceneManager.GetSceneByName("End_Scene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Level " + level);
            LoadLevel();
        }
        if (Input.GetButtonDown("Cancel") && !isInTransit)
        {
            if (isGameActive)
            {
                OpenPauseMenu();
            }
            else if (isMenuOnMain)
            {
                ClosePauseMenu();
            }
            else
            {
                UIManager.Instance.SwitchUI("Main UI");
            }
        }
    }
    public void UpdateLevelNumberText()
    {
        levelNumberText.text = level.ToString();
    }
    private IEnumerator PlayBGM()
    {
        yield return new WaitForSeconds(1.5f);
        AudioManager.PlaySound("Theme");
        yield break;
    }
    private void OpenPauseMenu()
    {
        isGameActive = false;
        Time.timeScale = 0;
        isMenuOnMain = true;
        pauseMenu.SetActive(true);
        UIManager.Instance.SwitchUI("Main UI");
    }
    private void ClosePauseMenu()
    {
        isGameActive = true;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void ResetLevel()
    {
        ClosePauseMenu();
        StartCoroutine(TransitionLevel(1));
    }
    public void LoadLevel()
    {
        floor.SetActive(true);
        leftWall.SetActive(true);
        rightWall.SetActive(true);

        if (SceneUtility.GetBuildIndexByScenePath("Level " + level) != -1)
        {
            SceneManager.LoadSceneAsync("Level " + level, LoadSceneMode.Additive);
            levelNumberText.gameObject.SetActive(true);
            UpdateLevelNumberText();
            isGameCompleted = false;
        }
        else
        {
            SceneManager.LoadSceneAsync("End_Scene", LoadSceneMode.Additive);
            levelNumberText.gameObject.SetActive(false);
            isGameCompleted = true;
        }
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.transform.position = startingPos;

        switch (level)
        {
            case 9:
            case 10:
                {
                    player.transform.position = startingPos + new Vector2(0, 0.8f);
                    break;
                }
            case 11:
                {
                    floor.SetActive(false);
                    player.transform.position = new Vector2(0.8f, 1.4f);
                    break;
                }
            case 14:
            case 15:
            case 12:
                {
                    floor.SetActive(false);
                    leftWall.SetActive(false);
                    rightWall.SetActive(false);
                    player.transform.position = new Vector2(0, 2f);
                    break;
                }
            case 13:
                {
                    floor.SetActive(false);
                    leftWall.SetActive(false);
                    rightWall.SetActive(false);
                    player.transform.position = new Vector2(0, 1f);
                    break;
                }
            case 17:
                {
                    floor.SetActive(false);
                    leftWall.SetActive(false);
                    rightWall.SetActive(false);
                    player.transform.position += new Vector3(0, 0.4f);
                    break;
                }
            case 20:
            case 18:
                {
                    floor.SetActive(false);
                    leftWall.SetActive(false);
                    rightWall.SetActive(false);
                    player.transform.position = new Vector2(0, 1.5f);
                    break;
                }
        }
        
    }
    public void NextLevel()
    {
        if (!isGameCompleted)
        {
            StartCoroutine(TransitionLevel(level + 1));
        }
    }
    private IEnumerator TransitionLevel(int newLevel)
    {
        isInTransit = true;
        ClosePauseMenu();
        transition.StartTransition();
        isGameActive = false;
        yield return new WaitForSeconds(2);
        SceneManager.UnloadSceneAsync("Level " + level);
        level = newLevel;
        LoadLevel();
        yield return new WaitForSeconds(0.5f);
        transition.EndTransition();
        yield return new WaitForSeconds(1f);
        isGameActive = true;
        isInTransit = false;
        yield break;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
