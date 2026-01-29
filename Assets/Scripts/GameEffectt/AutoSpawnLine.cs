using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoSpawnLine : MonoBehaviour
{
    public Board board; // 绑定到游戏板
    public TileBase tile; // 绑定到方块
    private float timer = 0f; // 计时器
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

        timer += Time.deltaTime;
        if (timer >= board.spawnDelay)
        {
            timer = 0f;
            SpawnLines();
        }
    }

    private void SpawnLines()
    {
        // 1. 先检查活动方块是否会被上移的方块卡住
        if (WillActivePieceCollide())
        {
            // 如果会卡住，则强制锁定当前方块
            board.activePiece.Lock();
        }

        // 2. 正常执行方块上移
        board.Clear(board.activePiece);

        // 从顶部开始逐行上移
        for (int row = board.Bounds.yMax - 1; row > board.Bounds.yMin; row--)
        {
            for (int col = board.Bounds.xMin; col < board.Bounds.xMax; col++)
            {
                Vector3Int srcPos = new Vector3Int(col, row - 1, 0);
                TileBase below = board.tilemap.GetTile(srcPos);
                board.tilemap.SetTile(new Vector3Int(col, row, 0), below);
            }
        }

        // 3. 在底部生成新行（随机留一个空位）
        int emptyCol = Random.Range(board.Bounds.xMin, board.Bounds.xMax);
        for (int col = board.Bounds.xMin; col < board.Bounds.xMax; col++)
        {
            Vector3Int pos = new Vector3Int(col, board.Bounds.yMin, 0);
            board.tilemap.SetTile(pos, col == emptyCol ? null : tile);
        }

        // 4. 重新设置活动方块
        board.Set(board.activePiece);
    }

    private bool WillActivePieceCollide()
    {
        board.Clear(board.activePiece);
        if (board.IsValidPosition(board.activePiece, board.activePiece.position + Vector3Int.down))
        {
            board.Set(board.activePiece);
            return false;
        }
        board.Set(board.activePiece);
        return true;
    }
}
