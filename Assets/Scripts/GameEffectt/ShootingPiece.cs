using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShootingPiece : MonoBehaviour
{
    public BoardFun board;  // 用于获取游戏板信息
    public Tilemap tilemap;  // 用于获取游戏板信息

    public class Bullet // 子弹类
    {
        public Vector3Int position;
        public Vector3Int[] cells;
        public TileBase tile;
    }

    private List<Bullet> bullets = new List<Bullet>();  // 子弹列表
    private EnemyPiece enemyPiece;  // 敌人方块
    public float fallSpeed = 0.25f;  // 子弹下落速度
    private float fallTimer = 0f;   // 子弹下落计时器

    public void Initialize(int maxShots, EnemyPiece enemyPiece)
    {
        this.enemyPiece = enemyPiece;
        bullets = new List<Bullet>(maxShots);
    }

    public bool AddShotPiece(PieceFun piece)
    {
        if (bullets.Count >= bullets.Capacity) return false;

        var bullet = new Bullet()
        {
            position = piece.position,
            cells = new Vector3Int[piece.cells.Length],
            tile = piece.data.tile
        };

        for (int i = 0; i < piece.cells.Length; i++)
        {
            bullet.cells[i] = piece.cells[i];
        }

        bullets.Add(bullet);
        return true;
    }

    public void UpdateBullets()
    {
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallSpeed)
        {
            fallTimer = 0f;
            ProcessBullets();
        }
    }

    private void ProcessBullets()
    {
        ClearAllBullets();

        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            var bullet = bullets[i];
            bullet.position += Vector3Int.down;

            if (!IsValidPosition(bullet))
            {
                bullets.RemoveAt(i);
                continue;
            }

            if (CheckCollisionWithSnapshot(bullet, enemyPiece.GetEnemySnapshots()))
            {
                bullets.RemoveAt(i);
                continue;
            }

            DrawBullet(bullet);
        }
    }

    private bool CheckCollisionWithSnapshot(Bullet bullet, List<EnemyPiece.EnemySnapshot> snapshots)
    {
        foreach (var enemy in snapshots)
        {
            if (CheckPerfectMatch(bullet, enemy))
            {
                board.AddScore();
                enemyPiece.DestroyEnemy(snapshots.IndexOf(enemy));
                return true;
            }

            if (CheckSimpleCollision(bullet, enemy))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckPerfectMatch(Bullet bullet, EnemyPiece.EnemySnapshot enemy)
    {
        // 检查当前帧和下一帧位置的双重覆盖
        return CheckCoverage(bullet.position, bullet.cells, enemy) ||
               CheckCoverage(bullet.position + Vector3Int.down, bullet.cells, enemy);
    }

    private bool CheckCoverage(Vector3Int bulletPos, Vector3Int[] cells, EnemyPiece.EnemySnapshot enemy)
    {
        var healthPositions = new HashSet<Vector3Int>();
        foreach (var health in enemy.healths)
        {
            healthPositions.Add(enemy.position + (Vector3Int)health);
        }

        var bulletPositions = new HashSet<Vector3Int>();
        foreach (var cell in cells)
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

    private bool CheckSimpleCollision(Bullet bullet, EnemyPiece.EnemySnapshot enemy)
    {
        foreach (var bulletCell in bullet.cells)
        {
            Vector3Int bulletWorldPos = bullet.position + bulletCell;

            foreach (var enemyCell in enemy.cells)
            {
                Vector3Int enemyWorldPos = enemy.position + (Vector3Int)enemyCell;
                if (bulletWorldPos == enemyWorldPos)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public List<Bullet> GetBullets() => bullets;

    public void ClearAllBullets()
    {
        foreach (var bullet in bullets)
        {
            foreach (var cell in bullet.cells)
            {
                board.Clear(cell, bullet.position);
            }
        }
    }

    private void DrawBullet(Bullet bullet)
    {
        foreach (var cell in bullet.cells)
        {
            board.Set(cell, bullet.position, bullet.tile);
        }
    }

    private bool IsValidPosition(Bullet bullet)
    {
        foreach (var cell in bullet.cells)
        {
            Vector3Int tilePosition = bullet.position + cell;
            if (tilePosition.y < board.Bounds.yMin)
            {
                return false;
            }
        }
        return true;
    }
}