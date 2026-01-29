// SceneLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("UI")]
    public GameObject loadingScreen;
    public SoundManager soundManager;
    public float fakeLoadMinTime = 1f; // 最小加载时间（避免一闪而过）
    public Animator topPanelAnimator;
    public Animator bottomPanelAnimator;
    public Animator logoAnimator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(loadingScreen);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 从主菜单加载游戏场景
    public void LoadGameScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    // 从游戏场景返回主菜单
    public void ReturnToMenu()
    {
        StartCoroutine(LoadSceneAsync(0)); // 0是主菜单场景索引
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // 显示加载界面
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        topPanelAnimator.SetBool("FadeIn", true);
        bottomPanelAnimator.SetBool("FadeIn", true);
        logoAnimator.SetBool("FadeIn", true);


        float timer = 0;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        // 模拟加载进度（避免进度条瞬间满）
        while (timer < fakeLoadMinTime || operation.progress < 0.9f)
        {
            timer += Time.deltaTime;

            yield return null;
        }


        // 实际完成加载
        operation.allowSceneActivation = true;

        // 隐藏加载界面（确保新场景的Awake已执行）
        yield return new WaitForSeconds(0.5f);
        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        switch (sceneIndex)
        {
            // 主菜单
            case 0: soundManager.PlayMenuMusic(); break;
            case 1: soundManager.PlayClassicMusic(); break;
            case 2: soundManager.PlayChallengeMusic(); break;
            case 3: soundManager.PlayImpossibleMusic(); break;
            case 4: soundManager.PlayFunplayMusic(); break;
            case 5: soundManager.PlayClassicMusic(); break;
        }
    }
}