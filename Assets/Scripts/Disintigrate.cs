using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disintigrate : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particlePrefab;
    
    private void OnDestroy()
    {
        ParticleSystem _particles = Instantiate(_particlePrefab);
        _particles.Play();
    }
}
