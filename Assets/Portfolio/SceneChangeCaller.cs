using UnityEngine;

public class SceneChangeCaller : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void ChangeScene()
    {
        SceneChanger.Instance.ChangeScene(sceneName);
    }
}