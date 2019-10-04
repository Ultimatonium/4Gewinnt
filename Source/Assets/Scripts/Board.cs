using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject redStone;
    [SerializeField]
    private GameObject yellowStone;
    [SerializeField]
    private GameObject blankStone;

    private const int boardWidth = 7;
    private const int boardHeight = 6;
    private const float leftBorder = 1.03f;
    private const float bottomBorder = 0.91f;
    private const float rowBorder = 1.21f;
    private const float columnBorder = 1.36f;

    private PlayerEnum[,] boardSlots;

    public void Initialize()
    {
        boardSlots = new PlayerEnum[boardWidth, boardHeight];
        for (int i = 0; i < boardSlots.GetLength(0); i++)
        {
            for (int j = 0; j < boardSlots.GetLength(1); j++)
            {
                boardSlots[i, j] = PlayerEnum.NONE;
            }
        }

        foreach (GameObject stone in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            Destroy(stone);
        }
    }

    public void DropStone(PlayerEnum player, int column)
    {
        if (column < 0 || column > boardWidth)
        {
            Debug.LogError("slot " + column + " out of range");
            return;
        }

        int row = GetFreeRowInColumn(column);
        if (row == -1)
        {
            Debug.LogError("No free Row in Column " + column);
            return;
        }

        switch (player)
        {
            case PlayerEnum.red:
                SetStoneInRow(redStone, column, row);
                break;
            case PlayerEnum.yellow:
                SetStoneInRow(yellowStone, column, row);
                break;
            default:
                Debug.LogError("Impossible Player");
                return;
        }
        boardSlots[column, row] = player;
    }

    private void SetStoneInRow(GameObject stone, int column, int row)
    {
        Instantiate(stone, new Vector2(column * columnBorder + leftBorder, row * rowBorder + bottomBorder), Quaternion.identity);
    }

    public bool HasSlotFreeRows(int column)
    {
        return GetFreeRowInColumn(column) != -1;
    }

    private int GetFreeRowInColumn(int column)
    {
        for (int row = 0; row < boardSlots.GetLength(1); row++)
        {
            if (boardSlots[column, row] == PlayerEnum.NONE)
            {
                return row;
            }
        }
        return -1;
    }

    public bool CheckConnectFour(PlayerEnum player)
    {
        for (int i = 0; i < boardSlots.GetLength(0); i++)
        {
            for (int j = 0; j < boardSlots.GetLength(1); j++)
            {
                if (player == PlayerEnum.NONE) continue;
                if (CheckConnectedFourRow(i, j, player)
                    || CheckConnectedFourColumn(i, j, player)
                    || CheckConnectedFourDiagonal(i, j, player)) return true;
            }
        }
        return false;
    }

    private bool CheckConnectedFourRow(int i, int j, PlayerEnum player)
    {
        try
        {
            if (boardSlots[i, j] == player
                && boardSlots[i + 1, j] == player
                && boardSlots[i + 2, j] == player
                && boardSlots[i + 3, j] == player)
            {
                highliteConnectedFour(new Vector2(i, j), new Vector2(i + 1, j), new Vector2(i + 2, j), new Vector2(i + 3, j));
                return true;
            }
            return false;
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }
    }

    private bool CheckConnectedFourColumn(int i, int j, PlayerEnum player)
    {
        try
        {
            if (boardSlots[i, j] == player
                && boardSlots[i, j + 1] == player
                && boardSlots[i, j + 2] == player
                && boardSlots[i, j + 3] == player)
            {
                highliteConnectedFour(new Vector2(i, j), new Vector2(i, j + 1), new Vector2(i, j + 2), new Vector2(i, j + 3));
                return true;
            }

            return false;

        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }
    }

    private bool CheckConnectedFourDiagonal(int i, int j, PlayerEnum player)
    {
        try
        {
            if (boardSlots[i, j] == player
                && boardSlots[i + 1, j + 1] == player
                && boardSlots[i + 2, j + 2] == player
                && boardSlots[i + 3, j + 3] == player)
            {
                highliteConnectedFour(new Vector2(i, j), new Vector2(i + 1, j + 1), new Vector2(i + 2, j + 2), new Vector2(i + 3, j + 3));
                return true;
            }
            if (boardSlots[i, j] == player
                && boardSlots[i - 1, j - 1] == player
                && boardSlots[i - 2, j - 2] == player
                && boardSlots[i - 3, j - 3] == player)
            {
                highliteConnectedFour(new Vector2(i, j), new Vector2(i - 1, j - 1), new Vector2(i - 2, j - 2), new Vector2(i - 3, j - 3));
                return true;
            }
            return false;
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }
    }

    private void highliteConnectedFour(Vector2 pos1, Vector2 pos2, Vector2 pos3, Vector2 pos4)
    {
        for (int i = 0; i < boardSlots.GetLength(0); i++)
        {
            for (int j = 0; j < boardSlots.GetLength(1); j++)
            {
                if (boardSlots[i, j] == PlayerEnum.NONE) continue;
                if (new Vector2(i, j) != pos1
                    && new Vector2(i, j) != pos2
                    && new Vector2(i, j) != pos3
                    && new Vector2(i, j) != pos4)
                {
                    SetStoneInRow(blankStone, i, j);
                }

            }
        }
    }
}
