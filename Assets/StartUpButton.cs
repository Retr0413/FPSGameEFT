using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class StartUpButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonClicked()
    {
        Debug.Log("Button Clicked");

        // BattleScene に遷移
        SceneManager.LoadScene("StartScene");
    }
}
