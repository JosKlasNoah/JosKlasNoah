using UnityEngine;
using UnityEditor;
using System;

public interface IInteractable
{
    void OnItemInteract(PlayerController owningPlayer);
    void OnItemRightMouseButton(PlayerController owningPlayer);
    void UpdateObjectOffset(float newPosistion);
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    const float groundDistanceAllowed = .1f;

    [SerializeField]
    float _moveSpeed = 20, _jumpHeight = 15000;
    [SerializeField]
    int _maxJumpCount =1;

    Camera _cam;
    Rigidbody _rb;

    [SerializeField]
    Vector3 _moveInput;

    float mouseLookRotation;
    float mouseLookUp;
    int _currentJumpCount =0;
    float _jumpDelay;

    private void OnValidate()
    {
        _rb = GetComponent<Rigidbody>();
        // _rb.isKinematic = true;

    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = GetComponentInChildren<Camera>();
    }

    Vector3 oldmove;

    private void Update()
    {
        DrawJumpRay();

        #region Movement
       // Vector3 currentVelocity = _rb.velocity;

        _moveInput = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        _moveInput = Vector3.Normalize(_moveInput)* Time.deltaTime * _moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        _rb.AddForce(_moveInput - _rb.velocity);

        if (oldmove.magnitude - _rb.velocity.magnitude < .1f)
        {
            oldmove = _rb.velocity;
            //print(_rb.velocity);
        }

        #endregion

        #region LookAround

        //float mouseLookRotation will be set equal to the x value of the mousemovement multiplied by the sensitivity divined in the gameManager
        mouseLookRotation = Input.GetAxis("Mouse X") * 1;
        //float mouseLookUp will be set equal to its old value + the current Y axis of the mouse multiplied by the sensitivity. the result wil be between -80 and 80
        mouseLookUp = Mathf.Clamp(mouseLookUp - (Input.GetAxis("Mouse Y") * 1), -80, 80);

        _rb.MoveRotation(Quaternion.Euler(0, mouseLookRotation, 0) * _rb.rotation);
        _cam.gameObject.transform.localRotation = Quaternion.Euler(mouseLookUp, 0, 0);

        #endregion
    }

    void Jump()
    {
        if (IsGrounded())
        {
            _currentJumpCount = 0;
            print("test");
        }

        if (Time.time < _jumpDelay && _currentJumpCount > _maxJumpCount)
        {
            
            return;
        }
        else
        Debug.Log((Time.time < _jumpDelay) +":"+ (_currentJumpCount <= _maxJumpCount));


        _rb.AddForce(Vector3.up * _jumpHeight * Time.deltaTime);
        _jumpDelay = Time.time +.5f;
        _currentJumpCount++;
    }

    void DrawJumpRay()
    {
        Vector3 rayStartPos = transform.position - Vector3.up * (GetComponent<CapsuleCollider>().bounds.extents.y);
        Debug.DrawRay(rayStartPos, Vector3.down * groundDistanceAllowed, Color.red, 10);
    }

    bool IsGrounded()
    {
        Vector3 rayStartPos = transform.position - Vector3.up * (GetComponent<CapsuleCollider>().bounds.extents.y);

        return Physics.Raycast(rayStartPos, Vector3.down, groundDistanceAllowed);
    }



}
/*
public float movementSpeed = 20f;
public Camera owningCamera;

public LayerMask interactableLayers;

public IInteractable holdingObject;
public IInteractable hitObjectInterface;

//rigid body van de speler
Rigidbody rb;

Vector3 move;
float mouseLookRotation;
float mouseLookUp;



// Use this for initialization
void Awake()
{
    rb = GetComponent<Rigidbody>();
}

// Update is called once per frame
void Update()
{
    SwitchRigidBodyState(false);

    // Move right will be the x as from the rigid body multiplied by the horizontal axis (input keys)
    Vector3 mRight = rb.transform.right * Input.GetAxis("Horizontal");
    // Move Forward will be the z as from the rigid body multiplied by the horizontal axis (input keys)
    Vector3 mForward = rb.transform.forward * Input.GetAxis("Vertical");

    //move will be equal to the center of moveforward + moveright
    move = Vector3.Normalize(mForward + mRight);

    //float mouseLookRotation will be set equal to the x value of the mousemovement multiplied by the sensitivity divined in the gameManager
    mouseLookRotation = Input.GetAxis("Mouse X") * 1;
    //float mouseLookUp will be set equal to its old value + the current Y axis of the mouse multiplied by the sensitivity. the result wil be between -80 and 80
    mouseLookUp = Mathf.Clamp(mouseLookUp - (Input.GetAxis("Mouse Y") * 1), -80, 80);

    RayCheck();

    if (Input.GetMouseButtonDown(0))
    {
        if (holdingObject != null)
        {
            holdingObject.OnItemInteract(this);
        }
        else
        {
            if (hitObjectInterface != null)
                hitObjectInterface.OnItemInteract(this);
        }

    }
    else if (Input.GetMouseButtonDown(1))
    {
        if (holdingObject != null)
            holdingObject.OnItemRightMouseButton(this);
    }

    rb.velocity = move * movementSpeed;
    rb.MoveRotation(Quaternion.Euler(0, mouseLookRotation, 0) * rb.rotation);

    owningCamera.gameObject.transform.localRotation = Quaternion.Euler(mouseLookUp, 0, 0);

}

private void OnValidate()
{

}

void SwitchRigidBodyState(bool active)
{
    if (rb.isKinematic != active)
        rb.isKinematic = active;
}

void RayCheck()
{
    //visible line in editor
    Debug.DrawRay(owningCamera.gameObject.transform.position, owningCamera.gameObject.transform.forward * 2f, Color.green, 5);

    //create a new raycasthit
    RaycastHit hit = new RaycastHit();

    //shoot a ray forward , and only check the layers specified in the editor
    //IF we hit something
    if (Physics.Raycast(owningCamera.gameObject.transform.position, owningCamera.gameObject.transform.forward, out hit, 2.2f, interactableLayers))
    {
        //If the hit object has a rigidbody
        if (hit.rigidbody != null)
        {
            //hitobject is equal to the IInteractable from the hit object (if it has any),other wise its null 
            hitObjectInterface = hit.rigidbody.gameObject.GetComponent<IInteractable>();
        }
        else if (holdingObject != null)
        {
            holdingObject.UpdateObjectOffset(hit.distance);
        }
        else
            hitObjectInterface = null;
    }
    //IF we did not hit something with the ray
    else
    {
        //clear the hitobject
        hitObjectInterface = null;
        if (holdingObject != null)
        {
            holdingObject.UpdateObjectOffset(1500);
        }
    }
}
[ContextMenu("GameObject/Project/CreatePlayer",false,0)]
public static void CreatePlayer()
{

}

}
*/
