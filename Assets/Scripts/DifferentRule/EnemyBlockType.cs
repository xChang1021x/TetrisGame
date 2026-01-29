using UnityEngine;
using UnityEngine.Tilemaps;

public enum EnemyBlockType
{
    L,
    I,
    P,
    P2,
    P3,
    J,
    Z,
    S2,
    S,
    Z2,
    T,
    T2,
    T3,
    O,
    N,
    N2,
    X,
    X2
}

[System.Serializable]
public struct EnemyData
{
    public EnemyBlockType enemyBlockType;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }
    public Vector2Int[] healths { get; private set; }

    public void Initialize()
    {
        this.cells = Data.Enemys[this.enemyBlockType];
        this.healths = Data.EnemyHealths[this.enemyBlockType];
    }
}
