using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearChecker : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private static ClearChecker _instance;
    public static ClearChecker Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<ClearChecker>();
            }
            if (!_instance)
            {
                _instance = new GameObject().AddComponent<ClearChecker>();
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
    }

    public void ClearCheck()
    {
        var board = new int[9, 9];
        for (int i = 0; i < BoardManager.BoardSize; i++)
        {
            for (int j = 0; j < BoardManager.BoardSize; j++)
            {
                board[i, j] = BoardManager.Instance.Board.GetValueOrDefault(new Vector2Int(-4 + i, -4 + j));
            }
        }

        if (BoardChecker.IsBoardFull(board) && BoardChecker.IsBoardValid(board))
        {
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}
