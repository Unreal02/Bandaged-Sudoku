using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public enum BlockType
{
    None,
    Block11,
    Block12,
    Block21
}

public static class BoardDivider
{
    private static int BoardSize => BoardManager.BoardSize;
    private static BlockType[,] _dividedBoard;
    private static bool[,] _visited;

    public static BlockType[,] DivideBoard()
    {
        _dividedBoard = new BlockType[BoardSize, BoardSize];
        _visited = new bool[BoardSize, BoardSize];
        FillBlock(0);
        return _dividedBoard;
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

        var blockTypes = Enum.GetValues(typeof(BlockType))
            .ConvertTo<List<BlockType>>()
            .Where(blockType => blockType != BlockType.None)
            .ToArray();
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
