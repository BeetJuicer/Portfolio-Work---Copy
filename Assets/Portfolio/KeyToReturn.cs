using UnityEngine;

public class KeyToReturn : MonoBehaviour
{
    [SerializeField] private KeyCode key = KeyCode.R;
    [SerializeField] private string sceneName = "Portfolio";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            SceneChanger.Instance.ChangeScene(sceneName);
        }
    }
}
