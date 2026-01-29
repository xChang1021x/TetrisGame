using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }    // 主板
    public TetrominoData data { get; private set; }  // 形态数据
    public Vector3Int[] cells { get; private set; }  // 形态数据中的每个格子的坐标
    public Vector3Int position { get; private set; }    // 形态的位置
    public int rotationIndex { get; private set; }  // 形态的旋转索引

    public float stepDelay = 1.0f;    // 移动延迟
    public float lockDelay = 0.5f;    // 锁定延迟

    private float stepTime;    // 移动时间
    private float lockTime;    // 锁定时间

    private float moveInterval = 0.1f; // 移动间隔时间（秒）
    private float moveTimer = 0f;
    private bool isLeftPressed = false;
    private bool isRightPressed = false;
    private bool isDownPressed = false;

    public void Initialize(Board board, Vector3Int position, TetrominoData data, float stepDelay)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.stepDelay = stepDelay;
        this.rotationIndex = 0;
        this.stepTime = Time.time + stepDelay;
        this.lockTime = 0f;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (board.isPaused || board.isGameOver)
        {
            return;
        }

        this.board.Clear(this);
        this.lockTime += Time.deltaTime;

        // 处理按键按下（瞬时触发）
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
            SoundManager.Instance.PlayMoveSound();
            isLeftPressed = true;
            moveTimer = 0f; // 重置计时器
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
            SoundManager.Instance.PlayMoveSound();
            isRightPressed = true;
            moveTimer = 0f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
            SoundManager.Instance.PlayMoveSound();
            isDownPressed = true;
            moveTimer = 0f;
        }

        // 处理按键释放
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            isLeftPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            isRightPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            isDownPressed = false;
        }

        // 长按处理（按固定间隔移动）
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval)
        {
            if (isLeftPressed)
            {
                Move(Vector2Int.left);
                SoundManager.Instance.PlayMoveSound();
            }
            else if (isRightPressed)
            {
                Move(Vector2Int.right);
                SoundManager.Instance.PlayMoveSound();
            }
            else if (isDownPressed)
            {
                Move(Vector2Int.down);
                SoundManager.Instance.PlayMoveSound();
            }
            moveTimer = 0f; // 重置计时器
        }

        // 其他按键（瞬时触发）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
            SoundManager.Instance.PlayHardDropSound();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Rotate();
            SoundManager.Instance.PlayRotateSound();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SaveTetromino();
        }

        // 自动下落逻辑
        if (Time.time >= stepTime)
        {
            Step();
        }

        board.Set(this);
    }

    private void SaveTetromino()
    {
        if (board.isSaved)
        {
            return;
        }
        board.isSaved = true;
        board.ClearSavedTetromino();
        if (board.savedTetromino.cells == null)
        {
            board.savedTetromino = this.data;
            board.ClearNextTetromino();
            board.SpawnTetromino();
        }
        else
        {
            TetrominoData temp = board.savedTetromino;
            board.savedTetromino = this.data;
            Initialize(board, board.spawnPosition, temp, board.stepDelay);
        }
        board.SetSavedTetromino();
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if (lockTime >= lockDelay)
        {
            Lock();
        }
    }

    public void Lock()
    {
        board.isSaved = false;
        board.Set(this);
        board.ClearLines();
        board.ClearNextTetromino();
        board.SpawnTetromino();
    }

    private bool Move(Vector2Int direction)
    {
        Vector3Int newPosition = position;
        newPosition.x += direction.x;
        newPosition.y += direction.y;

        bool valid = board.IsValidPosition(this, newPosition);
        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0f;
        }
        return valid;
    }

    private void HardDrop()
    {
        int i = 0;
        while (Move(Vector2Int.down))
        {
            i++;
            continue;
        }

        board.AddScore(i);

        Lock();
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = this.cells[i];
            int x, y;
            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private void Rotate()
    {
        int originalRotation = rotationIndex;
        this.rotationIndex += 1;
        rotationIndex %= 4;

        ApplyRotationMatrix(1);

        if (!TestWallKicks(rotationIndex))
        {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-1);
        }
    }

    private bool TestWallKicks(int rotationIndex)
    {
        int wallkickIndex = GetWallKickIndex(rotationIndex);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallkickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex)
    {
        int wallkickIndex = rotationIndex * 2;
        wallkickIndex %= data.wallKicks.GetLength(0);
        return wallkickIndex;
    }

}
