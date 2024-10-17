using UnityEngine;
using TMPro;

public class EnemyCount : MonoBehaviour
{
    public int enemyCount = 0; // 敵の総カウント

    public int requiredEnemyCount = 5; // ゴールを有効にするために必要なカウント数

    public TextMeshProUGUI enemyText; // 敵のカウントを表示するTextMeshPro

    void Start()
    {
        UpdateText();
    }

    public void IncrementEnemyCount(int amount)
    {
        enemyCount += amount;
        UpdateText();
        CheckForSceneChange();
    }

    private void UpdateText()
    {
        enemyText.text = "Point: " + enemyCount.ToString();
    }

    private void CheckForSceneChange()
    {
        if (enemyCount >= requiredEnemyCount)
        {
            Goal goal = FindObjectOfType<Goal>();
            if (goal != null)
            {
                goal.ActivateGoal();
            }
        }
    }
}
