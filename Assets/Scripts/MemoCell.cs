using TMPro;
using UnityEngine;

public class MemoCell : MonoBehaviour
{
    private bool[] _memoNumbers;
    private TMP_Text[] _texts;

    private void Awake()
    {
        _memoNumbers = new bool[BoardManager.BoardSize];
        _texts = new TMP_Text[BoardManager.BoardSize];
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var cellNumber = child.GetComponent<MemoCellNumber>();
            cellNumber.Init(this, i);
            _texts[i] = child.GetComponent<TMP_Text>();
            _texts[i].enabled = false;
        }
    }

    public void OnClickNumber(int index)
    {
        _memoNumbers[index] = !_memoNumbers[index];
        _texts[index].enabled = _memoNumbers[index];
    }
}