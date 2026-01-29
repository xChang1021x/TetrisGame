using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PieceFun : MonoBehaviour
{
    public BoardFun board { get; private set; } // 绑定到BoardFun脚本
    public TetrominoData data { get; private set; } // 绑定到TetrominoData脚本
    public Vector3Int[] cells { get; private set; } // 方块的位置
    public Vector3Int position { get; private set; }    // 俄罗斯方块的坐标
    public int rotationIndex { get; private set; }  // 俄罗斯方块的旋转角度


    public void Initialize(BoardFun board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;

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


        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
            SoundManager.Instance.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
            SoundManager.Instance.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
            SoundManager.Instance.PlayShootSound();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Rotate();
            SoundManager.Instance.PlayRotateSound();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeTetromino(0);
            SoundManager.Instance.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeTetromino(1);
            SoundManager.Instance.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeTetromino(2);
            SoundManager.Instance.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeTetromino(3);
            SoundManager.Instance.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeTetromino(4);
            SoundManager.Instance.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeTetromino(5);
            SoundManager.Instance.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ChangeTetromino(6);
            SoundManager.Instance.PlayMoveSound();
        }
        board.Set(this);
    }

    private void ChangeTetromino(int index)
    {
        board.spawnPosition = position;
        board.SpawnTetromino(index);
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
        }
        return valid;
    }

    private void Shoot()
    {

        if (!board.shootingPiece.AddShotPiece(this))
        {
            return;
        }
        board.spawnPosition = position;
        board.Clear(this);
        board.SpawnTetromino(board.currentIndex);
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
