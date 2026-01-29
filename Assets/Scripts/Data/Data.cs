using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public static class Data
{
    public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
    public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
    public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };

    public static readonly Dictionary<Tetromino, Vector2Int[]> Cells = new Dictionary<Tetromino, Vector2Int[]>()
    {
        { Tetromino.I, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 2, 1) } },
        { Tetromino.J, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.L, new Vector2Int[] { new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.O, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.S, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0) } },
        { Tetromino.T, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.Z, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
    };

    public static readonly Dictionary<EnemyBlockType, Vector2Int[]> Enemys = new Dictionary<EnemyBlockType, Vector2Int[]>()
    {
        { EnemyBlockType.L, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1), new Vector2Int(2, 1), new Vector2Int(0, 2), new Vector2Int(2, 2), new Vector2Int(2, 3) } },
        { EnemyBlockType.I, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1), new Vector2Int(2, 1), new Vector2Int(0, 2), new Vector2Int(2, 2), new Vector2Int(2, 3), new Vector2Int(0, 3), new Vector2Int(0, 4), new Vector2Int(2, 4) } },
        { EnemyBlockType.P, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1), new Vector2Int(2, 1), new Vector2Int(0, 2), new Vector2Int(2, 2) } },
        { EnemyBlockType.P2, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1), new Vector2Int(2, 1), new Vector2Int(0, 2), new Vector2Int(2, 2) } },
        { EnemyBlockType.P3, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1), new Vector2Int(2, 1), new Vector2Int(0, 2), new Vector2Int(2, 2) } },
        { EnemyBlockType.J, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1), new Vector2Int(2, 1), new Vector2Int(0, 2), new Vector2Int(2, 2), new Vector2Int(0, 3) } },
        { EnemyBlockType.Z, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1) } },
        { EnemyBlockType.S2, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1) } },
        { EnemyBlockType.S, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(2, 1) } },
        { EnemyBlockType.Z2, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(2, 1) } },
        { EnemyBlockType.T, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(0, 1) } },
        { EnemyBlockType.T2, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(0, 1) } },
        { EnemyBlockType.T3, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(0, 1) } },
        { EnemyBlockType.O, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(0, 1), new Vector2Int(3, 1), new Vector2Int(0, 2), new Vector2Int(3, 2) } },
        { EnemyBlockType.N, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(3, 1), new Vector2Int(0, 2), new Vector2Int(3, 2), new Vector2Int(0, 3), new Vector2Int(3, 3) } },
        { EnemyBlockType.N2, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(3, 1), new Vector2Int(0, 2), new Vector2Int(3, 2), new Vector2Int(0, 3), new Vector2Int(3, 3) } },
        { EnemyBlockType.X, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(0, 1), new Vector2Int(2, 1), new Vector2Int(3, 1), new Vector2Int(0, 2), new Vector2Int(3, 2), new Vector2Int(0, 3), new Vector2Int(3, 3) } },
        { EnemyBlockType.X2, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(0, 1), new Vector2Int(2, 1), new Vector2Int(3, 1), new Vector2Int(0, 2), new Vector2Int(3, 2), new Vector2Int(0, 3), new Vector2Int(3, 3) } },
    };

    public static readonly Dictionary<EnemyBlockType, Vector2Int[]> EnemyHealths = new Dictionary<EnemyBlockType, Vector2Int[]>()
    {
        { EnemyBlockType.L, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(0, 3), new Vector2Int(1, 3) } },
        { EnemyBlockType.I, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3), new Vector2Int(1, 4) } },
        { EnemyBlockType.P, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3) } },
        { EnemyBlockType.P2, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(0, 3), new Vector2Int(1, 3) } },
        { EnemyBlockType.P3, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3), new Vector2Int(2, 3) } },
        { EnemyBlockType.J, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3), new Vector2Int(2, 3) } },
        { EnemyBlockType.Z, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(0, 2), new Vector2Int(1, 2) } },
        { EnemyBlockType.S2, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(0, 3) } },
        { EnemyBlockType.S, new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(2, 2) } },
        { EnemyBlockType.Z2, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(2, 3) } },
        { EnemyBlockType.T, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(2, 2) } },
        { EnemyBlockType.T2, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(1, 3) } },
        { EnemyBlockType.T3, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(1, 3) } },
        { EnemyBlockType.O, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(1, 2), new Vector2Int(2, 2) } },
        { EnemyBlockType.N, new Vector2Int[] { new Vector2Int(2, 1), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(1, 3) } },
        { EnemyBlockType.N2, new Vector2Int[] { new Vector2Int(2, 1), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(2, 3) } },
        { EnemyBlockType.X, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(2, 3) } },
        { EnemyBlockType.X2, new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(1, 3) } },
    };

    // 踢墙数据 参考链接：https://tetris.fandom.com/wiki/SRS
    private static readonly Vector2Int[,] WallKicksI = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
    };

    private static readonly Vector2Int[,] WallKicksJLOSTZ = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
    };

    public static readonly Dictionary<Tetromino, Vector2Int[,]> WallKicks = new Dictionary<Tetromino, Vector2Int[,]>()
    {
        { Tetromino.I, WallKicksI },
        { Tetromino.J, WallKicksJLOSTZ },
        { Tetromino.L, WallKicksJLOSTZ },
        { Tetromino.O, WallKicksJLOSTZ },
        { Tetromino.S, WallKicksJLOSTZ },
        { Tetromino.T, WallKicksJLOSTZ },
        { Tetromino.Z, WallKicksJLOSTZ },
    };
}
