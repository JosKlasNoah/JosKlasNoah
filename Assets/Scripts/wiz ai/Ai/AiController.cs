using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AiController : MonoBehaviour {

    [SerializeField]
    private float _minIdleTime = 2f;

    [SerializeField]
    private float _maxIdleTime = 10f;

    [SerializeField]
    private float _randomWanderDistance = 50f;

    [SerializeField, Range(0, 10)]
    private float _eyeSight = 45f;

    [SerializeField]
    private float _eyeHeight = 10f;

    [SerializeField]
    private float _lookDepth = 20f;

    [SerializeField, Range(0,360)]
    private float _lookRotation = 45f;

    [SerializeField, Range(0,3)]
    private float _lookRotationSpeed = 1f;

    [SerializeField]
    private float _attackRange = 15f;

    private PlayerController _target;
    private Vector3 _lastKnownTargetLocation;
    private Vector3 _startingLocation;

    public float MinIdleTime => _minIdleTime;

    public float MaxIdleTime => _maxIdleTime;

    public float RandomWanderDistance => _randomWanderDistance;

    public float EyeHeight => _eyeHeight;

    public float EyeSight => _eyeSight;

    public float LookDepth => _lookDepth;

    public float LookRotation => _lookRotation;

    public float LookRotationSpeed => _lookRotationSpeed;

    public float AttackRange => _attackRange;

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
