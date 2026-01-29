using UnityEngine;
using UnityEngine.Tilemaps;

public class AttackLine : MonoBehaviour
{
    public Board2P opponentBoard;
    public TileBase tile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnLines()
    {
        if (opponentBoard.isGameOver)
        {
            return;
        }

        // 1. 先检查活动方块是否会被上移的方块卡住
        if (WillActivePieceCollide())
        {
            // 如果会卡住，则强制锁定当前方块
            opponentBoard.activePiece.Lock();
        }

        // 2. 正常执行方块上移
        opponentBoard.Clear(opponentBoard.activePiece);

        // 从顶部开始逐行上移
        for (int row = opponentBoard.Bounds.yMax - 1; row > opponentBoard.Bounds.yMin; row--)
        {
            for (int col = opponentBoard.Bounds.xMin; col < opponentBoard.Bounds.xMax; col++)
            {
                Vector3Int srcPos = new Vector3Int(col, row - 1, 0);
                TileBase below = opponentBoard.tilemap.GetTile(srcPos);
                opponentBoard.tilemap.SetTile(new Vector3Int(col, row, 0), below);
            }
        }

        // 3. 在底部生成新行（随机留一个空位）
        int emptyCol = Random.Range(opponentBoard.Bounds.xMin, opponentBoard.Bounds.xMax);
        for (int col = opponentBoard.Bounds.xMin; col < opponentBoard.Bounds.xMax; col++)
        {
            Vector3Int pos = new Vector3Int(col, opponentBoard.Bounds.yMin, 0);
            opponentBoard.tilemap.SetTile(pos, col == emptyCol ? null : tile);
        }

        // 4. 重新设置活动方块
        opponentBoard.Set(opponentBoard.activePiece);
    }

    private bool WillActivePieceCollide()
    {
        opponentBoard.Clear(opponentBoard.activePiece);
        if (opponentBoard.IsValidPosition(opponentBoard.activePiece, opponentBoard.activePiece.position + Vector3Int.down))
        {
            opponentBoard.Set(opponentBoard.activePiece);
            return false;
        }
        opponentBoard.Set(opponentBoard.activePiece);
        return true;
    }
}
