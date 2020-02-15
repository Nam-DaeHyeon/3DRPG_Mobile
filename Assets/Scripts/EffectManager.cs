using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    private void Awake()
    {
        instance = this;
    }

    [Header("Magician")]
    [SerializeField] ParticleSystem _expolsion;
    [SerializeField] ParticleSystem _lightningbolt;
    [SerializeField] ParticleSystem _healing;
    [Header("Warrior")]
    [SerializeField] ParticleSystem _rush;
    [SerializeField] ParticleSystem _healthBless;
    [SerializeField] ParticleSystem _barrier;

    [Header("Monster")]
    [SerializeField] ParticleSystem _windCutter;

    //Spell Effect
    public void PlayEffect_Explosion(Vector3 pos)
    {
        _expolsion.transform.position = pos;
        if (!_expolsion.gameObject.activeSelf) _expolsion.gameObject.SetActive(true);

        _expolsion.Stop();
        _expolsion.Play();
    }

    public void PlayerEffect_LightningBolt(Vector3 pos)
    {
        _lightningbolt.transform.position = pos;
        if (!_lightningbolt.gameObject.activeSelf) _lightningbolt.gameObject.SetActive(true);

        _lightningbolt.Stop();
        _lightningbolt.Play();
    }

    public void PlayerEffect_Healing(Vector3 pos)
    {
        _healing.transform.position = pos;
        if (!_healing.gameObject.activeSelf) _healing.gameObject.SetActive(true);

        _healing.Stop();
        _healing.Play();
    }

    public void PlayerEffect_Barrier(Transform target)
    {
        _barrier.transform.position = target.transform.position;
        _barrier.transform.parent = target;

        _barrier.Stop();
        _barrier.Play();
    }

    public void PlayerEffect_Barrier_OFF()
    {
        _barrier.Stop();

        _barrier.transform.transform.position = Vector3.zero;
        _barrier.transform.parent = transform;
    }

    public void PlayerEffect_Rush(Transform target)
    {
        _rush.transform.position = target.transform.position + new Vector3(0, 1f, 0);
        _rush.transform.rotation = target.transform.rotation;
        _rush.transform.Rotate(Vector3.right * 90);

        _rush.transform.parent = target;

        _rush.Stop();
        _rush.Play();
    }

    public void PlayerEffect_Rush_OFF()
    {
        _rush.Stop();

        _rush.transform.position = Vector3.zero;
        _rush.transform.parent = transform;
    }

    public void PlayerEffect_HealthBless(Transform target)
    {
        _healthBless.transform.position = target.transform.position;

        _healthBless.transform.parent = target;

        _healthBless.Stop();
        _healthBless.Play();
    }

    public void PlayerEffect_HealthBless_OFF()
    {
        _healthBless.Stop();

        _healthBless.transform.transform.position = Vector3.zero;
        _healthBless.transform.parent = transform;
    }

    //Monster Dead Effect
    public void PlayEffect_WindCutter(Vector3 pos)
    {
        _windCutter.transform.position = pos;
        if (!_windCutter.gameObject.activeSelf) _windCutter.gameObject.SetActive(true);

        _windCutter.Stop();
        _windCutter.Play();
    }
}
