using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private int[,] _board;
    private const int BoardSize = 9;
    private const int BoxSize = 3;

    private void Awake()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        GenerateNumbers();
        GenerateBlocks();
        var str = "";
        for (var i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                str = str + _board[i, j] + " ";
            }
            str += "\n";
        }
        text.text = str;
    }

    public void GenerateNumbers()
    {
        _board = new int[BoardSize, BoardSize];
        FillNumber(0);
    }

    public void GenerateBlocks()
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
            _board[r, c] = number;
            if (!ValidateBoard())
            {
                continue;
            }
            if (FillNumber(pos + 1))
            {
                return true;
            }
        }

        _board[r, c] = 0;
        return false;
    }

    private bool ValidateBoard()
    {
        // 가로줄 검사
        for (var r = 0; r < BoardSize; r++)
        {
            var numbers = new HashSet<int>();
            for (var c = 0; c < BoardSize; c++)
            {
                if (_board[r, c] == 0)
                {
                    continue;
                }
                if (numbers.Contains(_board[r, c]))
                {
                    return false;
                }
                numbers.Add(_board[r, c]);
            }
        }

        // 세로줄 검사
        for (var c = 0; c < BoardSize; c++)
        {
            var numbers = new HashSet<int>();
            for (var r = 0; r < BoardSize; r++)
            {
                if (_board[r, c] == 0)
                {
                    continue;
                }
                if (numbers.Contains(_board[r, c]))
                {
                    return false;
                }
                numbers.Add(_board[r, c]);
            }
        }

        // 3x3 박스 검사
        for (var rOrigin = 0; rOrigin < BoardSize; rOrigin += BoxSize)
        {
            for (var cOrigin = 0; cOrigin < BoardSize; cOrigin += BoxSize)
            {
                var numbers = new HashSet<int>();
                for (var r = rOrigin; r < rOrigin + BoxSize; r++)
                {
                    for (var c = cOrigin; c < cOrigin + BoxSize; c++)
                    {
                        if (_board[r, c] == 0)
                        {
                            continue;
                        }
                        if (numbers.Contains(_board[r, c]))
                        {
                            return false;
                        }
                        numbers.Add(_board[r, c]);
                    }
                }
            }
        }

        return true;
    }
}