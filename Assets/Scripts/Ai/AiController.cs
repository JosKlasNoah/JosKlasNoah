using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AiController : MonoBehaviour {

    [SerializeField]
    private float _minIdleTime = 2;

    [SerializeField]
    private float _maxIdleTime = 10;

    [SerializeField]
    private float _randomWanderDistance = 50;

    [SerializeField]
    private float _loseTargetDistance = 26;

    [SerializeField]
    private Vector2 _eyeSight = new Vector2( 1, 1 );

    [SerializeField]
    private Vector2 _rays = new Vector2( 6, 6 );

    [SerializeField]
    private float _eyeHeight = 10;

    [SerializeField]
    private float _lookDepth = 20;

    [SerializeField, Range(0,360)]
    private float _lookRotation = 45;

    [SerializeField, Range(0,3)]
    private float _lookRotationSpeed = 1;

    [SerializeField]
    private float _attackRange = 19;

    [SerializeField, Range(0.5f, 5)]
    private float _cooldown = 1.5f;

    private PlayerController _target;
    private Vector3 _lastKnownTargetLocation;
    private Vector3 _startingLocation;

    public float MinIdleTime => _minIdleTime;

    public float MaxIdleTime => _maxIdleTime;

    public float RandomWanderDistance => _randomWanderDistance;

    public float LoseTargetDistance => _loseTargetDistance;

    public float EyeHeight => _eyeHeight;

    public Vector2 EyeSight => _eyeSight;

    public Vector2 Rays => _rays;

    public float LookDepth => _lookDepth;

    public float LookRotation => _lookRotation;

    public float LookRotationSpeed => _lookRotationSpeed;

    public float AttackRange => _attackRange;

    public float Cooldown => _cooldown;

    public Vector3 StartingLocation => _startingLocation;

    public PlayerController Target {
        get { return _target; }
        set {
            _target = value;
            GetComponent<Animator>().SetBool("Target", true);
        }
    }

    public void LostTargetActor() {
        _lastKnownTargetLocation = _target.transform.position;
        _target = null;
        GetComponent<Animator>().SetBool("HasLastTargetLocation", true);
        GetComponent<Animator>().SetBool("Target", false);
        Debug.Log("lost target");
    }

    public Vector3 LastTargetLocation {
        get { return _lastKnownTargetLocation; }

        private set {
            _lastKnownTargetLocation = new Vector3() + value;
        }
    }

    private void Awake() {
        _startingLocation = transform.position;
    }
}
