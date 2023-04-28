using System.Collections.Generic;

public static class BoardChecker
{
    public static int BoardSize => BoardManager.BoardSize;
    public static int BoxSize => BoardManager.BoxSize;

    public static bool IsBoardValid(int[,] board)
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

    public static bool IsBoardFull(int[,] board)
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
