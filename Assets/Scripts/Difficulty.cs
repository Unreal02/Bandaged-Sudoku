using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    private void Awake()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var toggle = child.GetComponent<Toggle>();
            var difficulty = 41 - 10 * i;
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    BoardDivider.SetDifficulty(difficulty);
                }
            });
        }
    }
}
