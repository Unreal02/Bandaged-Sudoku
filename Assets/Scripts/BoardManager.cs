using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Blocks blocks;

    private Dictionary<Vector2Int, int> _board;
    public Dictionary<Vector2Int, int> Board { get { return _board; } }
    private int[,] _answer;
    public static readonly int BoardSize = 9;
    public static readonly int BoxSize = 3;
    private const int _originX = -4;
    private const int _originY = -4;
    private MemoCells _memoCells;

    [SerializeField] private GameObject block11;
    [SerializeField] private GameObject block12;
    [SerializeField] private GameObject block21;

    private static BoardManager _instance;
    public static BoardManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<BoardManager>();
            }
            if (!_instance)
            {
                _instance = new GameObject().AddComponent<BoardManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        _memoCells = FindObjectOfType<MemoCells>();
    }

    private void Start()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        InitBoards();
        InitBlocks();
        _memoCells.ResetMemoCells();
    }

    public void InitBoards()
    {
        _board = new Dictionary<Vector2Int, int>();
        _answer = new int[BoardSize, BoardSize];
        FillNumber(0);
        var str = "";
        for (var x = 0; x < BoardSize; x++)
        {
            for (var y = 0; y < BoardSize; y++)
            {
                str = str + _answer[y, BoardSize - 1 - x] + " ";
            }
            str += "\n";
        }
        text.text = str;
    }

    public void InitBlocks()
    {
        var existingBlocks = FindObjectsOfType<Block>();
        foreach (var existingBlock in existingBlocks)
        {
            Destroy(existingBlock.gameObject);
        }

        var dividedBoard = BoardDivider.DivideBoard();
        Block block;

        var blockPositionsDict = new Dictionary<BlockType, List<Vector2Int>>();

        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                var blockType = dividedBoard[i, j];
                if (!blockPositionsDict.ContainsKey(blockType))
                {
                    blockPositionsDict.Add(blockType, new List<Vector2Int>());
                }
                if (blockType != BlockType.None)
                {
                    blockPositionsDict[blockType].Add(new Vector2Int(i, j));
                }
            }
        }

        var randomBlockPosX = 6;
        var randomBlockPosY = -6;
        foreach (var (blockType, blockPositions) in blockPositionsDict)
        {
            var arr = blockPositions.ToArray();
            arr.Shuffle();
            foreach (var position in arr)
            {
                var i = position.x;
                var j = position.y;
                switch (blockType)
                {
                    case BlockType.None:
                        break;
                    case BlockType.Block11:
                        block = Instantiate(block11, blocks.transform).GetComponent<Block>();
                        block.Init(new Vector2Int(_originX + i, _originY + j), new int[] { _answer[i, j] }, false);
                        break;
                    case BlockType.Block12:
                        block = Instantiate(block12, blocks.transform).GetComponent<Block>();
                        block.Init(new Vector2Int(randomBlockPosX, randomBlockPosY), new int[] { _answer[i, j], _answer[i, j + 1] }, true);
                        break;
                    case BlockType.Block21:
                        block = Instantiate(block21, blocks.transform).GetComponent<Block>();
                        block.Init(new Vector2Int(randomBlockPosX, randomBlockPosY), new int[] { _answer[i, j], _answer[i + 1, j] }, true);
                        break;
                }

                if (blockType != BlockType.Block11)
                {
                    randomBlockPosY += 2;
                    if (randomBlockPosY >= 6)
                    {
                        randomBlockPosX += 2;
                        randomBlockPosY = -6;
                    }
                }
            }
        }

        blocks.Init();
    }

    public void AddBlock(Vector2Int pos, int number)
    {
        Assert.IsTrue(!_board.ContainsKey(pos));
        _board[pos] = number;
    }

    public void RemoveBlock(Vector2Int pos)
    {
        _board.Remove(pos);
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
        numbers.Shuffle();

        foreach (var number in numbers)
        {
            _answer[r, c] = number;
            if (!BoardChecker.IsBoardValid(_answer))
            {
                continue;
            }
            if (FillNumber(pos + 1))
            {
                return true;
            }
        }

        _answer[r, c] = 0;
        return false;
    }
}