using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayClassicMode() => SceneLoader.Instance.LoadGameScene(1);
    public void PlayChallengeMode() => SceneLoader.Instance.LoadGameScene(2);
    public void PlayImpossibleMode() => SceneLoader.Instance.LoadGameScene(3);
    public void PlayFunplayMode() => SceneLoader.Instance.LoadGameScene(4);
    public void PlayTwoPlayerMode() => SceneLoader.Instance.LoadGameScene(5);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayButtonSound()
    {
        SoundManager.Instance.PlayButtonSound();
    }

    // 绑定到按钮的OnClick事件
    public void QuitGame()
    {
#if UNITY_WEBGL
        // WebGL特殊处理
        HandleWebGLQuit();
#else
        // Windows/其他平台处理
        HandleStandaloneQuit();
#endif
    }

    void HandleWebGLQuit()
    {
        Application.OpenURL("about:blank");
    }

    void HandleStandaloneQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
