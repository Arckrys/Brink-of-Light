using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioClip villageClip;
    private AudioClip dungeonOneClip;
    private AudioClip dungeonTwoClip;
    private AudioClip dungeonThreeClip;
    private AudioClip cinematicClip;
    private AudioClip deathClip;

    private AudioSource source;

    private Dictionary<string, AudioClip> musicData = new Dictionary<string, AudioClip>();
    private string currentMusic = "village";

    private static MusicManager _instance;

    public static MusicManager MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<MusicManager>();
            }

            return _instance;
        }
    }


    void Start()
    {
        cinematicClip = Resources.Load<AudioClip>("Sounds/Music/Cinematic");
        villageClip = Resources.Load<AudioClip>("Sounds/Music/Village");
        dungeonOneClip = Resources.Load<AudioClip>("Sounds/Music/Dungeon1");
        dungeonTwoClip = Resources.Load<AudioClip>("Sounds/Music/Dungeon2");
        dungeonThreeClip = Resources.Load<AudioClip>("Sounds/Music/Dungeon3");
        deathClip = Resources.Load<AudioClip>("Sounds/Music/Death");

        musicData.Add("village", villageClip);
        musicData.Add("cinematic", cinematicClip);
        musicData.Add("dungeon1", dungeonOneClip);
        musicData.Add("dungeon2", dungeonTwoClip);
        musicData.Add("dungeon3", dungeonThreeClip);
        musicData.Add("death", deathClip);

        source = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SetCurrentMusic(string musicKey)
    {
        if (currentMusic == musicKey)
            return;
    
        currentMusic = musicKey;
        StopAllCoroutines();
        StartCoroutine(ChangeMusic(musicKey));
    }

    private IEnumerator ChangeMusic(string musicKey)
    {
        while(source.volume > 0)
        {
            source.volume -= 0.015f;
            yield return new WaitForSeconds(0.05f);
        }

        source.clip = musicData[musicKey];
        source.Play();

        while (source.volume < 1)
        {
            source.volume += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }

        source.volume = 1f;
    }

}
