using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject successUI;
    public GameObject failureUI;
    void Start()
    {
        successUI.SetActive(false);
        failureUI.SetActive(false);
    }

    void Update()
    {
        
    }

    public void ShowSuccessUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        successUI.SetActive(true);
    }

    public void ShowFailureUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        failureUI.SetActive(true);
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
