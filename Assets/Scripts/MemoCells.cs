using UnityEngine;

public class MemoCells : MonoBehaviour
{
    private MemoCell[] _cells;

    private void Awake()
    {
        _cells = transform.GetComponentsInChildren<MemoCell>();
    }

    public void ResetMemoCells()
    {
        foreach (var cell in _cells)
        {
            cell.Init();
        }
    }
}
