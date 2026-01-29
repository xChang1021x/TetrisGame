using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyPiece : MonoBehaviour
{
    public TileBase healthTile; // 匹配方块的渲染图
    public BoardFun board { get; private set; } // 绑定的游戏板
    public List<EnemyData> data { get; private set; }   // 敌人数据
    public List<Vector3Int> positions { get; private set; } // 敌人位置
    private float moveSpeed = 0.5f; // 敌人移动速度
    private float moveTimer = 0f; // 敌人移动计时器

    public void Initialize(BoardFun board)
    {
        this.board = board;
        this.data = new List<EnemyData>();
        this.positions = new List<Vector3Int>();
    }

    public void AddEnemy(EnemyData enemyData)
    {
        int randomX = Random.Range(board.Bounds.xMin, board.Bounds.xMax - 3);
        data.Add(enemyData);
        positions.Add(new Vector3Int(randomX, -13, 0));
        DrawEnemy(data.Count - 1);
    }

    public void UpdateEnemies()
    {
        if (data.Count == 0) return;

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveSpeed)
        {
            moveTimer = 0f;
            MoveUp();
        }
    }

    public void upLevel()
    {
        moveSpeed -= 0.05f;
        if (moveSpeed < 0.25f)
        {
            moveSpeed = 0.25f;
        }
    }

    public void CheckBulletCollisions(ShootingPiece shootingPiece)
    {
        for (int i = data.Count - 1; i >= 0; i--)
        {
            if (data[i].healths == null) continue;

            foreach (var bullet in shootingPiece.GetBullets())
            {
                if (CheckCoverage(bullet.position, bullet.cells, i))
                {
                    board.AddScore();
                    DestroyEnemy(i);
                    break;
                }
            }
        }
    }

    private bool CheckCoverage(Vector3Int bulletPos, Vector3Int[] bulletCells, int enemyIndex)
    {
        HashSet<Vector3Int> healthPositions = new HashSet<Vector3Int>();
        foreach (var health in data[enemyIndex].healths)
        {
            healthPositions.Add(positions[enemyIndex] + (Vector3Int)health);
        }

        HashSet<Vector3Int> bulletPositions = new HashSet<Vector3Int>();
        foreach (var cell in bulletCells)
        {
            bulletPositions.Add(bulletPos + cell);
        }

        foreach (var healthPos in healthPositions)
        {
            if (!bulletPositions.Contains(healthPos))
            {
                return false;
            }
        }
        return healthPositions.Count > 0;
    }

    public List<EnemySnapshot> GetEnemySnapshots()
    {
        var snapshots = new List<EnemySnapshot>();
        for (int i = 0; i < data.Count; i++)
        {
            snapshots.Add(new EnemySnapshot
            {
                position = positions[i],
                cells = data[i].cells,
                healths = data[i].healths
            });
        }
        return snapshots;
    }

    public struct EnemySnapshot
    {
        public Vector3Int position;
        public Vector2Int[] cells;
        public Vector2Int[] healths;
    }

    public void DestroyEnemy(int enemyIndex)
    {
        ClearEnemy(enemyIndex);
        ClearEnemyHealth(enemyIndex);
        data.RemoveAt(enemyIndex);
        positions.RemoveAt(enemyIndex);
    }

    private void ClearEnemy(int index)
    {
        foreach (var cell in data[index].cells)
        {
            board.Clear((Vector3Int)cell, positions[index]);
        }
    }

    private void ClearEnemyHealth(int index)
    {
        foreach (var health in data[index].healths)
        {
            board.Clear((Vector3Int)health, positions[index]);
        }
    }

    private void MoveUp()
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].cells == null) continue;

            ClearEnemy(i);
            ClearEnemyHealth(i);
            positions[i] += Vector3Int.up;

            if (CheckPosition(i))
            {
                board.Hurt();
                DestroyEnemy(i);
                continue;
            }

            DrawEnemy(i);
            DrawEnemyHealth(i);
        }
    }

    private bool CheckPosition(int index)
    {
        int yMax = 0;
        foreach (var cell in data[index].cells)
        {
            Vector3Int position = positions[index] + (Vector3Int)cell;
            if (position.y > yMax)
            {
                yMax = position.y;
            }
        }
        if (yMax >= board.Bounds.yMax - 5)
        {
            return true;
        }

        return false;
    }

    private void DrawEnemy(int index)
    {
        foreach (var cell in data[index].cells)
        {
            board.Set((Vector3Int)cell, positions[index], data[index].tile);
        }
    }

    private void DrawEnemyHealth(int index)
    {
        foreach (var health in data[index].healths)
        {
            board.Set((Vector3Int)health, positions[index], healthTile);
        }
    }

    public void ClearAllEnemies()
    {
        for (int i = data.Count - 1; i >= 0; i--)
        {
            DestroyEnemy(i);
        }
    }
}