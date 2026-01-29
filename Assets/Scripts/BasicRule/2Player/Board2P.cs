using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Board2P : MonoBehaviour
{
    public GameStatusManager gameStatusManager;
    public AttackLine attackLine;
    public int playerId;
    public Tilemap tilemap { get; private set; }
    public Piece2P activePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public TetrominoData savedTetromino { get; set; }
    public Image[] nextTetrominoImages;
    public Image[] savedTetrominoImages;
    public Vector3Int spawnPosition;
    public Vector2Int gridSize = new Vector2Int(10, 20);
    public Text scoreText;
    public bool isSaved { get; set; }
    public bool isPaused { get; set; }
    public bool isGameOver { get; set; }
    public float stepDelay { get; set; }
    public float spawnDelay { get; set; }
    public int score { get; set; }

    private TetrominoData data;
    private TetrominoData nextData;

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
        this.activePiece = GetComponentInChildren<Piece2P>();
        this.isSaved = false;
        this.isPaused = false;
        this.isGameOver = false;
        this.stepDelay = 1.0f;
        this.spawnDelay = 5.0f;
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

    }

    public void PauseGame()
    {
        gameStatusManager.PauseGame();
    }

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
        this.activePiece.Initialize(this, this.spawnPosition, data, playerId);

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

    public void GameOver()
    {
        gameStatusManager.GameOver(playerId);
        isGameOver = true;
    }

    public void Set(Piece2P piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.position + piece.cells[i];
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece2P piece)
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
            while (clearedLines > 1)
            {
                attackLine.SpawnLines();
                clearedLines--;
            }
        }

        this.score += 100 * (clearedLines * clearedLines);

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

    public bool IsValidPosition(Piece2P piece, Vector3Int position)
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

    public void ResetGame()
    {
        this.score = 0;
        this.stepDelay = 1.0f;
        this.spawnDelay = 5.0f;
        this.isSaved = false;
        this.isPaused = false;
        this.isGameOver = false;
        this.scoreText.text = score.ToString();
        this.tilemap.ClearAllTiles();
        ClearNextTetromino();
        ClearSavedTetromino();
        savedTetromino = new TetrominoData();
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
