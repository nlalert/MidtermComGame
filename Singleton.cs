﻿using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Singleton
{
    public const int SCREEN_WIDTH = 640;
    public const int SCREEN_HEIGHT = 480;

    public const int CHIP_GRID_WIDTH = 8;
    public const int CHIP_GRID_HEIGHT = 12;
    public const int CHIP_SHOOTING_HEIGHT = 430;
    public const int CHIP_SIZE = 32;
    public const int CHIP_SHADOW_HEIGHT = 3;
    public const int PLAY_AREA_START_X = (SCREEN_WIDTH - (CHIP_GRID_WIDTH * CHIP_SIZE)) / 2;
    public const int PLAY_AREA_END_X = PLAY_AREA_START_X + (CHIP_GRID_WIDTH * CHIP_SIZE);
    public const float MAX_PLAYER_ROTATION = (float)(80 * (Math.PI / 180)); //80 Degree

    public const int CHIP_BREAK_AMOUNT = 3;
    public const int CEILING_WAITING_TURN = 8;

    public int CeilingPosition;
    public int ChipShotAmount;
    public int Stage;

    public int Score;

    public GameBoard GameBoard;

    public ChipType CurrentChip;
    public ChipType NextChip;
    
    public Random Random;

    public enum GameState
    {
        MainMenu,
        SetLevel,
        Playing,
        CheckChipAndCeiling,
        Pause,
        PassingLevel,
        GameOver,
    }

    public GameState CurrentGameState;

    public KeyboardState PreviousKey, CurrentKey;

    private static Singleton instance;
    public float Volume = 1.0f;
    private Singleton() { 
        Random = new Random();
    }

    public static Singleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
    }

    public static Rectangle GetChipViewPort(ChipType chipType)
    {
        switch (chipType)
        {
            case ChipType.Explosive:
                return new Rectangle(0, 48, CHIP_SIZE, CHIP_SIZE + CHIP_SHADOW_HEIGHT);
            default:
                return new Rectangle(((int)chipType - 1) * CHIP_SIZE, 0, CHIP_SIZE, CHIP_SIZE + CHIP_SHADOW_HEIGHT);
        }

    }
}

