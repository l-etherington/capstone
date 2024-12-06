using System;
using System.Collections.Generic;

public class Level1
{
    public static int[,] horizontal = {
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,0,0,0,1,1,1,0,1,1,1,0},
    {0,0,1,1,1,0,1,1,0,0,0,0,0,0,1},
    {1,1,1,0,0,2,0,0,0,0,0,0,0,2,0},
    {0,0,0,1,0,0,1,1,0,0,1,1,0,0,1},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };
public static int[,] vertical = {
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
    {1,0,0,0,0,2,0,0,0,0,1,0,0,1,0,1},
    {1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1}
    };
// public int[,] arrows = {{0,1,"Up"},{3,2,"Down"},{0,5,"Right"}};
public static int[,] goblin_tokens = {{4,5},{2,3},{4,7},{1,8},{0,7}};
public static int[,] goat_tokens = {{1,1},{3,5},{2,4},{3,9},{2,8}};
public static Interactable[,] change_floor_buttons = {
    new Interactable(loc = {2,3}, util = , add = , rem = , color = "red", hfloors = {{3,5}, {3,13}}, vfloors = {{2,5}})};
}

