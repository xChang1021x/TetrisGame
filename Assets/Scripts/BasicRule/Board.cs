using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject pauseCanvas;  // 暂停画面
    public GameObject gameOverCanvas;   // 游戏结束画面
    public Tilemap tilemap { get; private set; }    // 游戏区域格子地图
    public Piece activePiece { get; private set; }  // 当前俄罗斯方块
    public TetrominoData[] tetrominoes;  // 俄罗斯方块数据
    public TetrominoData savedTetromino { get; set; }   // 保存的俄罗斯方块数据
    public Image[] nextTetrominoImages;  // 下一个俄罗斯方块图片
    public Image[] savedTetrominoImages;    // 保存的俄罗斯方块图片
    public Vector3Int spawnPosition;    // 俄罗斯方块初始位置
    public Vector2Int gridSize = new Vector2Int(10, 20);    // 游戏区域格子大小
    public Text levelText;    // 等级文字
    public Text scoreText;    // 分数文字
    public Text timeText;    // 时间文字
    public bool isSaved { get; set; }    // 是否保存了俄罗斯方块
    public bool isPaused { get; set; }    // 是否暂停游戏
    public bool isGameOver { get; set; }    // 是否游戏结束
    public float stepDelay { get; set; }    // 俄罗斯方块移动延迟
    public float spawnDelay { get; set; }    // 俄罗斯方块生成延迟
    private int score = 0;  // 分数
    private float time = 0f;    // 时间
    private int level = 1;  // 等级
    private int nextLevelScore = 1000;  // 下一级所需分数

    private TetrominoData data;  // 当前俄罗斯方块数据
    private TetrominoData nextData;  // 下一个俄罗斯方块数据

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
        this.activePiece = GetComponentInChildren<Piece>();
        this.isSaved = false;
        this.isPaused = false;
        this.isGameOver = false;
        this.stepDelay = 1.0f;
        this.spawnDelay = 5.0f;
        this.pauseCanvas.SetActive(false);
        this.gameOverCanvas.SetActive(false);
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (isPaused)
        {
            return;
        }

        time += Time.deltaTime;

        // 将时间转换为 TimeSpan（方便格式化）
        System.TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        // 格式化为 MM:SS.ff（分钟:秒.毫秒）
        timeText.text = string.Format("{0:D2}:{1:D2}",
            timeSpan.Minutes,
            timeSpan.Seconds);
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            // 显示暂停画面
            pauseCanvas.SetActive(true);
            // 暂停音效
            SoundManager.Instance.PlayPauseSound();
            // 绑定分数文字框
            Transform scoreObject = pauseCanvas.transform.Find("Score");
            Text scoreText = scoreObject.GetComponent<Text>();
            scoreText.text = score.ToString();
            // 绑定时间文字框
            Transform timeObject = pauseCanvas.transform.Find("Time");
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
            // 隐藏暂停画面
            pauseCanvas.SetActive(false);
            // 恢复音效
            SoundManager.Instance.PlayResumeSound();
        }
    }

    /// <summary>
    /// 随机生成一个新的俄罗斯方块并设置到下一个俄罗斯方块
    /// </summary>
    public void SpawnTetromino()
    {
        if (data.cells == null)
        {
            int randomIndex = UnityEngine.Random.Range(0, tetrominoes.Length);
            this.data = tetrominoes[randomIndex];
        }
        else
        {
            data = nextData;
        }

        int randomIndex2 = UnityEngine.Random.Range(0, tetrominoes.Length);
        this.nextData = tetrominoes[randomIndex2];
        this.activePiece.Initialize(this, this.spawnPosition, data, stepDelay);

        SetNextTetromino();

        if (!IsValidPosition(this.activePiece, this.spawnPosition))
        {
            GameOver();
        }
        else
        {
            Set(this.activePiece);
        }
    }

    /// <summary>
    /// 设置下一个俄罗斯方块的图片
    /// </summary>
    public void SetNextTetromino()
    {
        Sprite sprite = GetSpriteFromTileRuntime(nextData.tile);
        for (int i = 0; i < 4; i++)
        {
            int col = nextData.cells[i].x;
            int row = nextData.cells[i].y;

            nextTetrominoImages[row * 4 + col + 5].sprite = sprite;
        }
    }


    /// <summary>
    /// 清除下一个俄罗斯方块的图片
    /// </summary>
    public void ClearNextTetromino()
    {
        Sprite sprite = Resources.Load<Sprite>($"Sprites/slot");
        for (int i = 0; i < 4; i++)
        {
            int col = nextData.cells[i].x;
            int row = nextData.cells[i].y;
            nextTetrominoImages[row * 4 + col + 5].sprite = sprite;
        }
    }

    /// <summary>
    /// 保存当前俄罗斯方块的状态
    /// </summary>
    public void SetSavedTetromino()
    {
        Sprite sprite = GetSpriteFromTileRuntime(savedTetromino.tile);
        for (int i = 0; i < 4; i++)
        {
            int col = savedTetromino.cells[i].x;
            int row = savedTetromino.cells[i].y;
            savedTetrominoImages[row * 4 + col + 5].sprite = sprite;
        }
    }

    /// <summary>
    /// 清除保存的俄罗斯方块的图片
    /// </summary>
    public void ClearSavedTetromino()
    {
        if (savedTetromino.cells == null)
        {
            return;
        }
        Sprite sprite = Resources.Load<Sprite>($"Sprites/slot");
        for (int i = 0; i < 4; i++)
        {
            int col = savedTetromino.cells[i].x;
            int row = savedTetromino.cells[i].y;
            savedTetrominoImages[row * 4 + col + 5].sprite = sprite;
        }
    }


    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver()
    {
        // 游戏结束
        isGameOver = true;
        // 显示游戏结束画面
        gameOverCanvas.SetActive(true);
        // 绑定分数文字框
        Transform scoreObject = gameOverCanvas.transform.Find("Score");
        Text scoreText = scoreObject.GetComponent<Text>();
        scoreText.text = score.ToString();
        // 绑定时间文字框
        Transform timeObject = gameOverCanvas.transform.Find("Time");
        Text timeText = timeObject.GetComponent<Text>();
        // 将时间转换为 TimeSpan（方便格式化）
        System.TimeSpan timeSpan = TimeSpan.FromSeconds(this.time);
        // 格式化为 MM:SS.ff（分钟:秒.毫秒）
        timeText.text = string.Format("{0:D2}:{1:D2}",
            timeSpan.Minutes,
            timeSpan.Seconds);
    }

    /// <summary>
    /// 渲染俄罗斯方块
    /// </summary>
    /// <param name="piece"> 俄罗斯方块 </param>
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.position + piece.cells[i];
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    /// <summary>
    /// 清除俄罗斯方块的显示
    /// </summary>
    /// <param name="piece"></param>
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.position + piece.cells[i];
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int clearedLines = 0;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                clearedLines++;
            }
            else
            {
                row++;
            }
        }
        if (clearedLines > 0)
        {
            SoundManager.Instance.PlayLineClearSound();
        }
        // 分数计算
        this.score += (100 + level * 10) * (clearedLines * clearedLines);
        if (score > nextLevelScore && level < 10)
        {
            level++;
            nextLevelScore += 1000 * level;
            stepDelay -= 0.1f;
            spawnDelay -= 0.1f;
            if (stepDelay < 0.1f)
            {
                stepDelay = 0.1f;
            }
            if (spawnDelay < 3.0f)
            {
                spawnDelay = 3.0f;
            }
            levelText.text = "Level " + level.ToString();
        }

        scoreText.text = score.ToString();
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = score.ToString();
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int tilePosition = new Vector3Int(col, row, 0);
            tilemap.SetTile(tilePosition, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int tilePosition = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(tilePosition);

                tilePosition = new Vector3Int(col, row, 0);
                tilemap.SetTile(tilePosition, above);
            }

            row++;
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int tilePosition = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = position + piece.cells[i];

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }
    // 重置游戏数据
    public void ResetGame()
    {
        this.pauseCanvas.SetActive(false);
        this.gameOverCanvas.SetActive(false);
        this.score = 0;
        this.time = 0f;
        this.level = 1;
        this.nextLevelScore = 1000;
        this.stepDelay = 1.0f;
        this.spawnDelay = 5.0f;
        this.isSaved = false;
        this.isPaused = false;
        this.isGameOver = false;
        this.levelText.text = "Level " + level.ToString();
        this.scoreText.text = score.ToString();
        this.timeText.text = "00:00";
        this.tilemap.ClearAllTiles();
        SpawnTetromino();
    }

    public Sprite GetSpriteFromTileRuntime(TileBase tile)
    {
        if (tile == null) return null;

        // 假设图片与Tile同名且放在Resources/Sprites文件夹
        string spriteName = tile.name;
        Sprite sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");

        if (sprite == null)
        {
            Debug.LogWarning($"找不到Resources/Sprites/{spriteName}.png");
        }
        return sprite;
    }

    public void PlayButtonSound()
    {
        SoundManager.Instance.PlayButtonSound();
    }
}
