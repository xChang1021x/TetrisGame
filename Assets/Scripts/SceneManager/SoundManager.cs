using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource musicSource;
    public AudioSource effectSource;
    public AudioClip buttonClickSound;
    public float effectVolume = 0.5f;
    public float musicVolume = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            effectVolume = 1.0f;
            musicVolume = 1.0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.clip = Resources.Load<AudioClip>("Music/MenuMusic");
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayButtonSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/Click");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayMoveSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/Move");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayRotateSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/Move");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayHardDropSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/HardDrop");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayLineClearSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/Clear");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayShootSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/Shoot");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayEnemydieSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/EnemyDie");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayHurtSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/Hurt");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayPauseSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/Pause");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayResumeSound()
    {
        effectSource.clip = Resources.Load<AudioClip>("Sounds/Resume");
        effectSource.PlayOneShot(effectSource.clip);
    }

    public void PlayMenuMusic()
    {
        musicSource.clip = Resources.Load<AudioClip>("Music/MenuMusic");
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayClassicMusic()
    {
        musicSource.clip = Resources.Load<AudioClip>("Music/ClassicMusic");
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlayChallengeMusic()
    {
        musicSource.clip = Resources.Load<AudioClip>("Music/ChallengeMusic");
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlayImpossibleMusic()
    {
        musicSource.clip = Resources.Load<AudioClip>("Music/ImpossibleMusic");
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlayFunplayMusic()
    {
        musicSource.clip = Resources.Load<AudioClip>("Music/FunplayMusic");
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicVolume = volume;
    }

    public void SetEffectVolume(float volume)
    {
        effectSource.volume = volume;
        effectVolume = volume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetEffectVolume()
    {
        return effectVolume;
    }
}
