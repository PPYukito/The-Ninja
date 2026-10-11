using UnityEngine;

public class SceneControllerEntities : MonoBehaviour
{
    [SerializeField] private AudioSource clickAudioSound;
    protected AudioSource sceneBGMAudioSource;

    protected virtual void Awake()
    {
        sceneBGMAudioSource = GetComponent<AudioSource>();
    }

    protected void StopBGMSound()
    {
        sceneBGMAudioSource.Stop();
    }

    protected void PlayClickSound() => clickAudioSound.Play(0);
}
