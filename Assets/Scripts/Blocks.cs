using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    private Block[] _blocks;
    private Camera _camera;
    private int _numberOnCursor;

    public bool IsDraggingBlock;

    public void Init()
    {
        _blocks = GetComponentsInChildren<Block>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (IsDraggingBlock)
        {
            return;
        }

        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var mousePoint = new Vector2Int(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));

        var number = BoardManager.Instance.Board.GetValueOrDefault(mousePoint);
        if (number != _numberOnCursor)
        {
            SetNumberOnCursor(number);
        }
    }

    private void SetNumberOnCursor(int number)
    {
        _numberOnCursor = number;
        foreach (var block in _blocks)
        {
            block.SetNumberColor(number);
        }
    }
}
