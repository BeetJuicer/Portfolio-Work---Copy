using System.Collections.Generic;
using StateMachineCore;
using UnityEngine;

public class DialogueGameEvents : MonoBehaviour
{
    public static DialogueGameEvents Instance;

    // Action now carries an optional string parameter
    private Dictionary<string, System.Action<string>> eventDictionary = new();

    void Awake() => Instance = this;

    private void Start()
    {
        Subscribe("ChangeScene", ChangeScene);
    }

    private void ChangeScene(string sceneName)
    {
        SceneChanger.Instance.ChangeScene(sceneName);
    }

    public void Subscribe(string name, System.Action<string> callback)
    {
        eventDictionary[name] = callback;
    }

    public void Unsubscribe(string name)
    {
        eventDictionary.Remove(name);
    }

    public void Trigger(string name, string variable = null)
    {
        if (eventDictionary.TryGetValue(name, out var action))
            action?.Invoke(variable);
        else
            Debug.LogWarning("GameEvent triggered but has no subscribers: " + name);
    }
}
