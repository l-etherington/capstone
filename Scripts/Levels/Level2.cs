using System;
using System.Collections.Generic;

public class Level2
{
    public static int[,] horizontal = {
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,2,0,0,1,1,1,0,1,1,1,0},
    {0,1,1,1,1,1,1,1,1,0,1,1,0,0,1},
    {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1},
    {1,1,0,1,1,1,1,1,1,1,1,1,0,1,1},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };
public static int[,] vertical = {
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1}
    };
// public int[,] arrows = {{0,1,"Up"},{3,2,"Down"},{0,5,"Right"}};
public static int[,] goblin_tokens = {};
public static int[,] goat_tokens = {{0,1},{0,2},{0,3},{0,4},{2,8}};
public static int[,] change_floor_buttons = {{2,2}};
}

