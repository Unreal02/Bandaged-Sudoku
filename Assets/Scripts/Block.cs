using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class Block : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    private int[,] _numbers;
    private TMP_Text[,] _texts;
    private Blocks _blocks;

    private Vector2Int _position;
    private Vector3 _clickedPosition;
    private Vector3 _clickedMousePosition;

    public void Init(Vector2Int position, int[] numbers, bool isMovable)
    {
        Assert.AreEqual(numbers.Length, size.x * size.y);
        Assert.AreEqual(numbers.Length, transform.childCount);

        _position = new Vector2Int();
        _numbers = new int[size.x, size.y];
        _texts = new TMP_Text[size.x, size.y];
        _blocks = GetComponentInParent<Blocks>();

        if (isMovable)
        {
            transform.AddComponent<PolygonCollider2D>().isTrigger = true;
        }
        else
        {
            var sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.HSVToRGB(0, 0, 0.75f);
        }

        transform.position = new Vector3(position.x, position.y);
        _position = position;

        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var i = x * size.y + y;
                var number = numbers[i];
                _numbers[x, y] = number;
                var text = transform.GetChild(i).GetComponent<TMP_Text>();
                _texts[x, y] = text;
                text.text = number.ToString();
                BoardManager.Instance.AddBlock(_position + new Vector2Int(x, y), number);
            }
        }
    }

    private void OnMouseDown()
    {
        _clickedPosition = transform.position;
        _clickedMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _blocks.IsDraggingBlock = true;

        for (var i = 0; i < size.x; i++)
        {
            for (var j = 0; j < size.y; j++)
            {
                BoardManager.Instance.RemoveBlock(_position + new Vector2Int(i, j));
            }
        }
    }

    private void OnMouseDrag()
    {
        transform.position = _clickedPosition + Camera.main.ScreenToWorldPoint(Input.mousePosition) - _clickedMousePosition;
    }

    private void OnMouseUp()
    {
        _blocks.IsDraggingBlock = false;

        var x = Mathf.RoundToInt(transform.position.x);
        var y = Mathf.RoundToInt(transform.position.y);

        bool canMove = true;
        for (var i = 0; i < size.x; i++)
        {
            for (var j = 0; j < size.y; j++)
            {
                if (BoardManager.Instance.Board.ContainsKey(new Vector2Int(x + i, y + j)))
                {
                    canMove = false;
                    break;
                }
            }
            if (!canMove)
            {
                break;
            }
        }

        if (canMove)
        {
            transform.position = new Vector3(x, y);
            _position.x = x;
            _position.y = y;
        }
        else
        {
            transform.position = _clickedPosition;
        }

        for (var i = 0; i < size.x; i++)
        {
            for (var j = 0; j < size.y; j++)
            {
                BoardManager.Instance.AddBlock(_position + new Vector2Int(i, j), _numbers[i, j]);
            }
        }

        if (canMove)
        {
            ClearChecker.Instance.ClearCheck();
        }
    }

    public void SetNumberColor(int numberOnCursor)
    {
        for (var i = 0; i < size.x; i++)
        {
            for (var j = 0; j < size.y; j++)
            {
                _texts[i, j].color = _numbers[i, j] == numberOnCursor ? Color.blue : Color.black;
            }
        }
    }
}
