using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource shootSound;
    [SerializeField] AudioSource reloadSound;
    [SerializeField] AudioSource explosionSound;
    [SerializeField] AudioSource bloodSound;

    public void ShootSound()
    {
        shootSound.Play();
    }

    public void ReloadSound()
    {
        reloadSound.Play();
    }

    public void ExplosionSound()
    {
        explosionSound.Play();
    }

    public void BloodSound()
    {
        bloodSound.Play();
    }
}
