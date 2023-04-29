using UnityEngine;

public enum BlockType
{
    None,
    Block11,
    Block12,
    Block21
}

public static class BoardDivider
{
    private static int _filledBlockCount = 31;

    private static int BoardSize => BoardManager.BoardSize;
    private static BlockType[,] _dividedBoard;
    private static bool[,] _visited;

    public static void SetDifficulty(int filledBlockCount)
    {
        _filledBlockCount = filledBlockCount;
    }

    public static BlockType[,] DivideBoard()
    {
        _dividedBoard = new BlockType[BoardSize, BoardSize];
        _visited = new bool[BoardSize, BoardSize];
        while (true)
        {
            var positions = new Vector2Int[BoardSize * BoardSize];
            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    positions[i * BoardSize + j] = new Vector2Int(i, j);
                    _dividedBoard[i, j] = BlockType.None;
                    _visited[i, j] = false;
                }
            }
            positions.Shuffle();

            for (var i = 0; i < _filledBlockCount; i++)
            {
                var position = positions[i];
                _dividedBoard[position.x, position.y] = BlockType.Block11;
                _visited[position.x, position.y] = true;
            }

            if (FillBlock(0))
            {
                return _dividedBoard;
            }
        }
    }

    private static bool FillBlock(int pos)
    {
        if (pos >= BoardSize * BoardSize)
        {
            return true;
        }

        var x = pos / 9;
        var y = pos % 9;

        if (_visited[x, y])
        {
            return FillBlock(pos + 1);
        }

        var blockTypes = new BlockType[] { BlockType.Block12, BlockType.Block21 };
        blockTypes.Shuffle();

        foreach (var blockType in blockTypes)
        {
            _dividedBoard[x, y] = blockType;
            if (!ApplyVisited(x, y, blockType, true))
            {
                continue;
            }

            if (FillBlock(pos + 1))
            {
                return true;
            }

            ApplyVisited(x, y, blockType, false);
        }

        _dividedBoard[x, y] = BlockType.None;
        return false;
    }

    private static bool ApplyVisited(int x, int y, BlockType blockType, bool visited)
    {
        switch (blockType)
        {
            case BlockType.Block11:
                _visited[x, y] = visited;
                break;
            case BlockType.Block12:
                if (y >= BoardSize - 1 || _visited[x, y + 1])
                {
                    return false;
                }
                _visited[x, y] = visited;
                _visited[x, y + 1] = visited;
                break;
            case BlockType.Block21:
                if (x >= BoardSize - 1 || _visited[x + 1, y])
                {
                    return false;
                }
                _visited[x, y] = visited;
                _visited[x + 1, y] = visited;
                break;
        }
        return true;
    }
}
