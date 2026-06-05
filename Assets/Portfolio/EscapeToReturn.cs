using UnityEngine;

public class EscapeToReturn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChanger.Instance.ChangeScene("Portfolio");
        }
    }
}
