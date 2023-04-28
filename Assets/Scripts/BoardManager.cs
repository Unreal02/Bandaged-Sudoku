using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private int[,] _board;
    private int[,] _answerBoard;
    private const int BoardSize = 9;
    private const int BoxSize = 3;

    private void Awake()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        InitBoards();
        InitBlocks();
        var str = "";
        for (var r = 0; r < BoardSize; r++)
        {
            for (var c = 0; c < BoardSize; c++)
            {
                str = str + _answerBoard[r, c] + " ";
            }
            str += "\n";
        }
        text.text = str;
    }

    public void InitBoards()
    {
        _board = new int[BoardSize, BoardSize];
        _answerBoard = new int[BoardSize, BoardSize];
        FillNumber(0);
    }

    public void InitBlocks()
    {

    }

    private bool FillNumber(int pos)
    {
        if (pos >= BoardSize * BoardSize)
        {
            return true;
        }

        var r = pos / 9;
        var c = pos % 9;

        var numbers = new int[BoardSize];
        for (var i = 0; i < BoardSize; i++)
        {
            numbers[i] = i + 1;
        }
        for (var i = 0; i < BoardSize; i++)
        {
            var p1 = Random.Range(0, BoardSize);
            var p2 = Random.Range(0, BoardSize);
            (numbers[p1], numbers[p2]) = (numbers[p2], numbers[p1]);
        }

        for (var i = 0; i < BoardSize; i++)
        {
            var number = numbers[i];
            _answerBoard[r, c] = number;
            if (!IsBoardValid(_answerBoard))
            {
                continue;
            }
            if (FillNumber(pos + 1))
            {
                return true;
            }
        }

        _answerBoard[r, c] = 0;
        return false;
    }

    private static bool IsBoardValid(int[,] board)
    {
        // check rows
        for (var r = 0; r < BoardSize; r++)
        {
            var numbers = new HashSet<int>();
            for (var c = 0; c < BoardSize; c++)
            {
                if (board[r, c] == 0)
                {
                    continue;
                }
                if (numbers.Contains(board[r, c]))
                {
                    return false;
                }
                numbers.Add(board[r, c]);
            }
        }

        // check columns
        for (var c = 0; c < BoardSize; c++)
        {
            var numbers = new HashSet<int>();
            for (var r = 0; r < BoardSize; r++)
            {
                if (board[r, c] == 0)
                {
                    continue;
                }
                if (numbers.Contains(board[r, c]))
                {
                    return false;
                }
                numbers.Add(board[r, c]);
            }
        }

        // check 3x3 boxes
        for (var rOrigin = 0; rOrigin < BoardSize; rOrigin += BoxSize)
        {
            for (var cOrigin = 0; cOrigin < BoardSize; cOrigin += BoxSize)
            {
                var numbers = new HashSet<int>();
                for (var r = rOrigin; r < rOrigin + BoxSize; r++)
                {
                    for (var c = cOrigin; c < cOrigin + BoxSize; c++)
                    {
                        if (board[r, c] == 0)
                        {
                            continue;
                        }
                        if (numbers.Contains(board[r, c]))
                        {
                            return false;
                        }
                        numbers.Add(board[r, c]);
                    }
                }
            }
        }

        return true;
    }

    private static bool IsBoardFull(int[,] board)
    {
        for (var r = 0; r < BoardSize; r++)
        {
            for (var c = 0; c < BoardSize; c++)
            {
                if (board[r, c] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
}