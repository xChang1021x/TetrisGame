using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BoardFun : MonoBehaviour
{
    public GameObject pausePanel;   // 暂停面板
    public GameObject gameOverPanel;    // 游戏结束面板
    public Tilemap tilemap { get; private set; }    // 地图
    public PieceFun activePiece { get; private set; }    // 当前的方块
    public EnemyPiece enemyPiece;    // 敌人方块
    public ShootingPiece shootingPiece;    // 子弹方块
    public TetrominoData[] tetrominoes;    // 所有可用的方块数据
    public EnemyData[] enemyData;    // 所有敌人数据
    public Vector3Int spawnPosition;    // 方块初始位置
    public Vector2Int gridSize = new Vector2Int(20, 24);    // 网格大小
    public Text levelText;    // 等级显示
    public Text scoreText;    // 分数显示
    public Text timeText;    // 时间显示
    public Text livesText;    // 生命显示
    public bool isPaused { get; set; }    // 是否暂停
    public bool isGameOver { get; private set; }    // 是否结束
    public int maxShots = 5;    // 最大子弹数量
    public int currentIndex = 0;    // 当前方块索引
    private int level = 1;    // 等级
    private int score = 0;    // 分数
    private int lives = 3;    // 生命
    private float time = 0f;    // 时间
    private TetrominoData data;    // 当前方块数据
    private EnemyData enemy;    // 当前敌人数据
    private float spawnTime = 5.0f;    // 敌人出生时间间隔
    private float spawnTimer = 0f;    // 敌人出生计时器

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.gridSize.x / 2, -this.gridSize.y / 2);
            return new RectInt(position, this.gridSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<PieceFun>();
        this.isPaused = false;
        this.isGameOver = false;
        this.level = 1;
        this.score = 0;
        this.lives = 3;
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
        for (int i = 0; i < enemyData.Length; i++)
        {
            this.enemyData[i].Initialize();
        }
        this.enemyPiece.Initialize(this);
        this.shootingPiece.Initialize(maxShots, this.enemyPiece);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnTetromino(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (isPaused || isGameOver)
        {
            return;
        }


        // 1. 先更新所有敌人位置
        enemyPiece.UpdateEnemies();

        // 2. 子弹碰撞检测（针对移动后的敌人）
        enemyPiece.CheckBulletCollisions(shootingPiece);

        // 2. 再更新所有子弹位置和检测碰撞
        shootingPiece.UpdateBullets();


        time += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // 将时间转换为 TimeSpan（方便格式化）
        System.TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        // 格式化为 MM:SS.ff（分钟:秒.毫秒）
        timeText.text = string.Format("{0:D2}:{1:D2}",
            timeSpan.Minutes,
            timeSpan.Seconds);

        if (spawnTimer >= spawnTime)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pausePanel.SetActive(true);
            SoundManager.Instance.PlayPauseSound();
            Transform scoreObject = pausePanel.transform.Find("Score");
            Text scoreText = scoreObject.GetComponent<Text>();
            scoreText.text = score.ToString();

            Transform timeObject = pausePanel.transform.Find("Time");
            Text timeText = timeObject.GetComponent<Text>();
            // 将时间转换为 TimeSpan（方便格式化）
            System.TimeSpan timeSpan = TimeSpan.FromSeconds(this.time);

            // 格式化为 MM:SS.ff（分钟:秒.毫秒）
            timeText.text = string.Format("{0:D2}:{1:D2}",
                timeSpan.Minutes,
                timeSpan.Seconds);
        }
        else
        {
            pausePanel.SetActive(false);
            SoundManager.Instance.PlayResumeSound();
        }
    }

    public void AddScore()
    {
        SoundManager.Instance.PlayEnemydieSound();
        score += 200;
        if (score % 1000 == 0 && level < 10)
        {
            level++;
            levelText.text = "Level " + level;
            enemyPiece.upLevel();
            if (level > 3)
            {
                spawnTime -= 0.5f;
                if (spawnTime < 2.0f)
                {
                    spawnTime = 2.0f;
                }
            }

            levelText.text = "Level " + level.ToString();
        }
        scoreText.text = score.ToString();
    }

    public void Hurt()
    {
        SoundManager.Instance.PlayHurtSound();
        lives--;
        if (lives == 0)
        {
            GameOver();
        }

        livesText.text = lives.ToString();
    }

    public void SpawnEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemyData.Length);
        enemy = enemyData[randomIndex];
        enemyPiece.AddEnemy(enemy);
    }

    public void SpawnTetromino(int index)
    {
        this.data = tetrominoes[index];
        this.currentIndex = index;
        bool found = false;
        for (int i = 0; i < this.data.cells.Length; i++)
        {
            Vector3Int tilePosition = spawnPosition + (Vector3Int)this.data.cells[i];
            while (tilePosition.x < Bounds.xMin)
            {
                tilePosition.x++;
                spawnPosition.x++;
                found = true;
            }
            while (tilePosition.x > Bounds.xMax)
            {
                tilePosition.x--;
                spawnPosition.x--;
                found = true;
            }
            if (found)
            {
                break;
            }
        }
        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(this.activePiece);

    }



    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);

        Transform scoreObject = gameOverPanel.transform.Find("Score");
        Text scoreText = scoreObject.GetComponent<Text>();
        scoreText.text = score.ToString();

        Transform timeObject = gameOverPanel.transform.Find("Time");
        Text timeText = timeObject.GetComponent<Text>();
        // 将时间转换为 TimeSpan（方便格式化）
        System.TimeSpan timeSpan = TimeSpan.FromSeconds(this.time);

        // 格式化为 MM:SS.ff（分钟:秒.毫秒）
        timeText.text = string.Format("{0:D2}:{1:D2}",
            timeSpan.Minutes,
            timeSpan.Seconds);

        Debug.Log("Game Over");
    }

    public void Set(PieceFun piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.position + piece.cells[i];
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public void Set(Vector3Int cells, Vector3Int position, TileBase tile)
    {

        Vector3Int tilePosition = position + cells;
        this.tilemap.SetTile(tilePosition, tile);

    }

    public void Clear(PieceFun piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.position + piece.cells[i];
            this.tilemap.SetTile(tilePosition, null);
        }
    }
    public void Clear(Vector3Int cells, Vector3Int position)
    {

        Vector3Int tilePosition = position + cells;
        this.tilemap.SetTile(tilePosition, null);

    }

    public bool IsValidPosition(PieceFun piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = position + piece.cells[i];

            if (tilePosition.x < bounds.xMin || tilePosition.x >= bounds.xMax)
            {
                return false;
            }
        }

        return true;
    }

    public void ResetGame()
    {
        isPaused = false;
        isGameOver = false;
        level = 1;
        score = 0;
        lives = 3;
        time = 0f;
        spawnTime = 5.0f;
        levelText.text = "Level " + level.ToString();
        scoreText.text = score.ToString();
        livesText.text = lives.ToString();
        timeText.text = "00:00";
        enemyPiece.ClearAllEnemies();
        shootingPiece.ClearAllBullets();
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void PlayButtonSound()
    {
        SoundManager.Instance.PlayButtonSound();
    }
}
