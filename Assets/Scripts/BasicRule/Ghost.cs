using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;   // 要显示的Tile样式
    public Board board; // 用于判断游戏是否暂停或结束
    public Piece trakingPiece;  // 正在追踪的Piece
    public Tilemap tilemap { get; private set; }    // 用于显示Ghost的Tilemap
    public Vector3Int[] cells { get; private set; }  // 用于显示Ghost的位置
    public Vector3Int position { get; private set; }    // 用于显示Ghost的位置

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        cells = new Vector3Int[4];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void LateUpdate()
    {
        if (board.isPaused || board.isGameOver)
        {
            return;
        }
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = position + cells[i];
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = trakingPiece.cells[i];
        }
    }

    private void Drop()
    {
        Vector3Int position = trakingPiece.position;

        int current = position.y;
        int bottom = -board.gridSize.y / 2 - 1;

        board.Clear(trakingPiece);
        for (int row = current; row >= bottom; row--)
        {
            position.y = row;

            if (board.IsValidPosition(trakingPiece, position))
            {
                this.position = position;
            }
            else
            {
                break;
            }
        }
        board.Set(trakingPiece);
    }

    private void Set()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = position + cells[i];
            this.tilemap.SetTile(tilePosition, tile);
        }
    }
}
