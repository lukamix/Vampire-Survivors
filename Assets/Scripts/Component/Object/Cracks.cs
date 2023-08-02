using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cracks : MonoBehaviour
{
    public List<Crack> crackList;
    public void PlaySFx()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_impact15_2");
    }
}
