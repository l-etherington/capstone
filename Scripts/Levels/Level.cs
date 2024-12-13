using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level{
    public int[,] horizontal;
    public int[,] vertical;
    public Dictionary<Vector2Int, bool> goblin_tokens;
    public Dictionary<Vector2Int, bool> goat_tokens;
    public Dictionary<string, List<object>> change_floor_buttons;
    public Vector2Int endzone_pos;
    public int max_moves;
    public Vector2Int PCstart;
    public Vector2Int NPCstart;
    public Vector2Int origin;

    // gravity direction change by number: 0=down, 1=left, 2=up, 3=right
    public Dictionary<Vector2Int, int> gravity_arrows;

    // Constructor to initialize the fields
    public Level(int[,] horizontal, int[,] vertical, Dictionary<Vector2Int, bool> goblinTokens, Dictionary<Vector2Int, bool> goatTokens,
        Dictionary<string, List<object>> changeFloorButtons, Dictionary<Vector2Int, int> gravityArrows, Vector2Int endzonePos, int maxMoves, 
        Vector2Int PCstart, Vector2Int NPCstart, Vector2Int origin)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
        this.goblin_tokens = goblinTokens;
        this.goat_tokens = goatTokens;
        this.change_floor_buttons = changeFloorButtons;
        this.gravity_arrows = gravityArrows;
        this.endzone_pos = endzonePos;
        this.max_moves = maxMoves;
        this.PCstart = PCstart;
        this.NPCstart = NPCstart;
        this.origin = origin;
    }
}