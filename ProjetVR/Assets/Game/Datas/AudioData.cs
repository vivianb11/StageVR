using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioData 
{
    public static Dictionary<SoundType, float> volumes = new Dictionary<SoundType, float>
    {
        { SoundType.Music, 1},
        { SoundType.SFX, 1},
        { SoundType.Voice, 1}
    };

    public static Dictionary<SoundType, float> expirationTime = new Dictionary<SoundType, float>
    {
        { SoundType.Music, -1},
        { SoundType.SFX, 5},
        { SoundType.Voice, 15}
    };
}
