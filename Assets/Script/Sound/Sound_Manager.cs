using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private const int BGM_CHANNEL = 2;

    private const int JNG_CHANNEL = 4;

    private const int SE_CHANNEL = 8;

    private const float BGM_DEFAULT_VOLUME = 0.6f;

    private const float JNG_DEFAULT_VOLUME = 1f;

    private const float SE_DEFAULT_VOLUME = 1;

    private static string[] fileNamePrefix =
    {
        "bgm",
        "jgl",
        "se"
    };

    private static string fileNameFormat = "{0}_{1}";

    public enum AudioType
    {
        BGM,
        JNG,
        SE
    }

    private Dictionary<AudioType, List<AudioSource>> channelDict = null;

    private static SoundManager instance = null;

    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;

        channelDict = new Dictionary<AudioType, List<AudioSource>>();

        // BGM再生用チャンネル追加
        channelDict[AudioType.BGM] = new List<AudioSource>();

        for (int i = 0; i < BGM_CHANNEL; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            channelDict[AudioType.BGM].Add(audioSource);
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = BGM_DEFAULT_VOLUME;
        }

        // JNG再生用チャンネル追加
        channelDict[AudioType.JNG] = new List<AudioSource>();

        for (int i = 0; i < JNG_CHANNEL; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            channelDict[AudioType.JNG].Add(audioSource);
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.volume = JNG_DEFAULT_VOLUME;
        }

        // SE再生用チャンネル追加
        channelDict[AudioType.SE] = new List<AudioSource>();

        for (int i = 0; i < SE_CHANNEL; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            channelDict[AudioType.SE].Add(audioSource);
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.volume = SE_DEFAULT_VOLUME;
        }
    }

    public bool PlayBGM(BGM bgm)
    {
        // BGMチャネルから空いてる方を探す
        AudioSource freeChannel = null;

        foreach (var channel in channelDict[AudioType.BGM])
        {
            if (!channel.isPlaying)
            {
                if (freeChannel == null)
                {
                    freeChannel = channel;
                }
            }
            else
            {
                channel.Stop();
            }
        }

        if (freeChannel == null)
        {
            return false;
        }


        freeChannel.clip = Resources.Load<AudioClip>("Sound/" + GetAudioClipName(bgm));
        freeChannel.Play();

        return true;
    }

    public bool PlayJNG(JNG jng)
    {
        // JNGチャネルから空いてる方を探す
        AudioSource freeChannel = null;

        foreach (var channel in channelDict[AudioType.JNG])
        {
            if (channel.isPlaying == false)
            {
                freeChannel = channel;
                break;
            }
        }

        if (freeChannel == null)
        {
            return false;
        }


        freeChannel.clip = Resources.Load<AudioClip>("Sound/" + GetAudioClipName(jng));
        freeChannel.Play();

        return true;
    }

    public bool PlaySE(SE se)
    {
        // SEチャネルから空いてる方を探す
        AudioSource freeChannel = null;

        foreach (var channel in channelDict[AudioType.SE])
        {
            if (channel.isPlaying == false)
            {
                freeChannel = channel;
                break;
            }
        }

        if (freeChannel == null)
        {
            return false;
        }


        freeChannel.clip = Resources.Load<AudioClip>("Sound/" + GetAudioClipName(se));
        freeChannel.Play();

        return true;
    }

    public void ChangeVolume(AudioType targetType, float volume)
    {
        var targetChannels = channelDict[targetType];
        foreach (AudioSource channel in targetChannels)
        {
            channel.volume = volume;
        }
    }

    private string GetAudioClipName(BGM bgm)
    {
        return GetAudioClipNameInternal(
            fileNamePrefix[(int)AudioType.BGM],
            (int)bgm);
    }

    private string GetAudioClipName(JNG jng)
    {
        return GetAudioClipNameInternal(
            fileNamePrefix[(int)AudioType.JNG],
            (int)jng);
    }

    private string GetAudioClipName(SE se)
    {
        return GetAudioClipNameInternal(
            fileNamePrefix[(int)AudioType.SE],
            (int)se);
    }

    private string GetAudioClipNameInternal(string prefix, int id)
    {
        return string.Format(fileNameFormat, prefix, id);
    }
}