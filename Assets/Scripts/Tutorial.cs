using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    public VideoPlayer player;
    public Animator anim;

    public void ShowVideo(VideoClip video)
    {
        player.clip = video;
        player.time = 0;
        player.Play();
        player.isLooping = false;
        anim.SetBool("Visible", true);
        StartCoroutine(CloseOnFinish());
    }

    IEnumerator CloseOnFinish()
    {
        yield return new WaitWhile(() => !player.isPlaying);
        yield return new WaitWhile(() => player.isPlaying);
        Stop();
    }

    public void Stop()
    {
        StopAllCoroutines();
        player.Stop();
        anim.SetBool("Visible", false);

    }
}
