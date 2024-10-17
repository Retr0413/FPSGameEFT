using UnityEngine;
using TMPro; // TextMeshProを使う場合
using UnityEditor; // UnityEditorを使うための名前空間

public class EndRoll : MonoBehaviour
{
    public TextMeshProUGUI endRollText; // エンドロールのTextMeshProコンポーネント
    public float scrollSpeed = 50f; // スクロール速度

    public GameObject[] objects; // 表示するゲームオブジェクトのリスト
    public float switchInterval = 2f; // オブジェクトを切り替える間隔（秒）

    private RectTransform rectTransform;
    private float screenHeight;
    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        // テキストをプログラム内で設定
        string credits = "Director: John Doe\n" +
                         "Producer: Jane Doe\n" +
                         "Lead Programmer: Alice Smith\n" +
                         "Artist: Bob Johnson\n" +
                         "Music: Charlie Brown\n" +
                         "Special Thanks: Everyone!\n\n" +
                         "Thank you for playing!";
        
        // TextMeshProUGUIコンポーネントにテキストを設定
        endRollText.text = credits;

        // RectTransformを取得
        rectTransform = endRollText.GetComponent<RectTransform>();

        // 画面の高さを取得
        screenHeight = Screen.height;

        // テキストを画面下に配置する
        rectTransform.anchoredPosition = new Vector2(0, -screenHeight);

        // 最初にすべてのオブジェクトを非表示にする
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        // 最初のオブジェクトを表示する
        if (objects.Length > 0)
        {
            objects[currentIndex].SetActive(true);
        }
    }

    void Update()
    {
        // テキストを上にスクロールさせる
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        // エンドロールが完全に画面外に出たかを確認
        if (rectTransform.anchoredPosition.y >= rectTransform.sizeDelta.y + screenHeight)
        {
            Debug.Log("End of credits. Stopping play mode.");
            UnityEditor.EditorApplication.isPlaying = false; // エディターのプレイモードを停止
        }

        // オブジェクトの表示を切り替える
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            // 現在のオブジェクトを非表示にする
            objects[currentIndex].SetActive(false);

            // 次のオブジェクトを表示する
            currentIndex = (currentIndex + 1) % objects.Length;
            objects[currentIndex].SetActive(true);

            // タイマーをリセットする
            timer = 0f;
        }
    }
}
