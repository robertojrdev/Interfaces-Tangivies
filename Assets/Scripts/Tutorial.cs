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
        anim.SetBool("Visible", true);
    }

    public void Stop()
    {
        player.Stop();
        anim.SetBool("Visible", false);

    }
}
