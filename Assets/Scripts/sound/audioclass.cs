using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class audioclass
{
    public SoundClass[] soundClass;
    public AudioSource source;

    public void playClipOnce(string name)
    {
        foreach (var soundClass1 in soundClass)
        {
            source.Stop();
            Debug.Log(name + " " + soundClass1._clipName);
            Debug.Log(name == soundClass1._clipName);
            if (name == soundClass1._clipName)
            {
                source.PlayOneShot(soundClass1._clip);
                return;
            }
        }
    }
    public void playClipOnLoop(string name)
    {
        foreach (var soundClass in soundClass)
        {
            if (name == soundClass._clipName)
            {
                if (source.isPlaying == false)
                {
                    source.PlayOneShot(soundClass._clip);
                }
            }

            
        }
    }

    public void stopAudio()
    {
        source.Stop();
    }

}
