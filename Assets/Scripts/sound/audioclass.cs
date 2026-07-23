using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class audioclass
{
    public SoundClass[] soundClass;
    public AudioSource source;

    public void playClipOnce(string name)
    {
        foreach (var soundClass in soundClass)
        {
            if (name == soundClass._clipName)
            {
                source.PlayOneShot(soundClass._clip);
            }
        }
    }
    public void playClipOnLoop(string name)
    {
        foreach (var soundClass in soundClass)
        {
            if (name == soundClass._clipName)
            {
                source.PlayOneShot(soundClass._clip);
            }

            if (source.isPlaying == false)
            {
                source.PlayOneShot(soundClass._clip);
            }
        }
    }

    public void stopAudio()
    {
        source.Stop();
    }

}
