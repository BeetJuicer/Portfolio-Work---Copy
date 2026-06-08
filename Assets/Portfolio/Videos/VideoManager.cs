using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance;
    public VideoPlayer[] players;
    int prepared = 0;
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        foreach (var vp in players)
        {
            vp.prepareCompleted += OnPrepared;
            vp.Prepare();
        }

        StartCoroutine(nameof(SceneFadeWhenReady));
    }

    private void Start()
    {
        SceneChanger.Instance.Close(); //wait for videos to load.

    }

    IEnumerator SceneFadeWhenReady()
    {
        yield return new WaitUntil(() => prepared == players.Length);
        SceneChanger.Instance.FadeIn();
    }

    void OnPrepared(VideoPlayer vp)
    {
        prepared++;
        vp.Play();
    }

}