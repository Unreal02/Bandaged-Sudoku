using UnityEngine;

public class MemoCellNumber : MonoBehaviour
{
    private MemoCell _memoCell;
    private int _index;

    private void Awake()
    {
        _memoCell = transform.parent.GetComponent<MemoCell>();
    }

    public void Init(MemoCell memoCell, int index)
    {
        _memoCell = memoCell;
        _index = index;
    }

    private void OnMouseUp()
    {
        _memoCell.OnClickNumber(_index);
    }
}
