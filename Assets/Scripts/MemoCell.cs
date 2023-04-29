using TMPro;
using UnityEngine;

public class MemoCell : MonoBehaviour
{
    private bool[] _memoNumbers;
    private TMP_Text[] _texts;
    private Camera _camera;

    private void Awake()
    {
        _memoNumbers = new bool[BoardManager.BoardSize];
        _texts = new TMP_Text[BoardManager.BoardSize];
        _camera = Camera.main;
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var cellNumber = child.GetComponent<MemoCellNumber>();
            cellNumber.Init(this, i);
            _texts[i] = child.GetComponent<TMP_Text>();
            _texts[i].alpha = 0f;
        }
    }

    private void Update()
    {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var mouseOffset = transform.position - mousePosition;
        if (mouseOffset.x > -0.5 && mouseOffset.x < 0.5 && mouseOffset.y > -0.5 && mouseOffset.y < 0.5)
        {
            for (var i = 0; i < BoardManager.BoardSize; i++)
            {
                if (!_memoNumbers[i])
                {
                    _texts[i].alpha = 0.25f;
                }
            }
        }
        else
        {
            for (var i = 0; i < BoardManager.BoardSize; i++)
            {
                if (!_memoNumbers[i])
                {
                    _texts[i].alpha = 0f;
                }
            }
        }
    }

    public void OnClickNumber(int index)
    {
        _memoNumbers[index] = !_memoNumbers[index];
        if (_memoNumbers[index])
        {
            _texts[index].alpha = 1f;
        }
    }
}