using UnityEngine;
using UnityEngine.UI;

public class CameraSpin : MonoBehaviour
{
    public Board board; // 绑定到Board脚本
    public Image panelImage;    // 绑定到屏幕遮挡面板
    public float spinSpeed = 0.0f;  // 旋转速度
    public float originalSpeed = 0.0f;  // 初始速度
    public float changeSpeed = 10.0f;   // 速度变化速度
    private float spinTime = 5.0f;  // 旋转时间
    private float spinTimer = 0.0f;  // 旋转计时器
    private float colorTimer = 0.0f;    // 颜色变化计时器
    private bool isIncrease = true;  // 颜色变化方向
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (board.isPaused)
        {
            return;
        }

        if (board.isGameOver)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            spinSpeed = originalSpeed;
            spinTimer = 0.0f;
            isIncrease = true;
            colorTimer = 0.0f;
            return;
        }
        // 旋转
        spinTimer += Time.deltaTime;
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        // 方向变化
        if (spinTimer >= spinTime)
        {
            spinSpeed = originalSpeed;
            spinTimer = 0.0f;
            isIncrease = !isIncrease;
        }
        // 速度变化
        if (isIncrease)
        {
            spinSpeed += changeSpeed * Time.deltaTime;
        }
        else
        {
            spinSpeed -= changeSpeed * Time.deltaTime;
        }
        // 颜色变化
        colorTimer += Time.deltaTime;
        panelImage.color = new Color(0, 0, 0, Mathf.Abs(Mathf.Sin(colorTimer)));
    }
}
