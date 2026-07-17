using UnityEngine;
using UnityEngine.Video;

public class VideoTexturePlayer : MonoBehaviour
{
    public string videoName = "video.mp4";
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Application.streamingAssetsPath + "/" + videoName;
        videoPlayer.isLooping = true;

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.Prepare();
    }

    void OnPrepared(VideoPlayer vp)
    {
        Debug.Log("Video ready: " + videoName);
        videoPlayer.Play();
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }
}