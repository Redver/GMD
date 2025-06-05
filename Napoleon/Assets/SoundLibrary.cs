using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    [SerializeField] private AudioClip musketClip1;
    [SerializeField] private AudioClip musketClip2;
    [SerializeField] private AudioClip musketClip3;
    [SerializeField] private AudioClip buildSfx;
    [SerializeField] private AudioClip buttonSoundNormal;
    [SerializeField] private AudioClip buttonSoundNormal2;
    [SerializeField] private AudioClip forbiddenSfx;

    private static SoundLibrary _instance;
    public static SoundLibrary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<SoundLibrary>();
                if (_instance == null)
                {
                    Debug.LogError("SoundLibrary instance not found in scene.");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public AudioClip GetRandomMusketClip()
    {
        int rand = Random.Range(0, 3);
        return rand switch
        {
            0 => musketClip1,
            1 => musketClip2,
            _ => musketClip3,
        };
    }

    public AudioClip GetRandomButtonClick()
    {
        return Random.value > 0.5f ? buttonSoundNormal : buttonSoundNormal2;
    }

    public AudioClip GetBuildSfx() => buildSfx;
    public AudioClip GetForbiddenSfx() => forbiddenSfx;

    public void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}
