using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.GameManager;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class ObjectBase : MonoBehaviour, IInteractable
{
    protected Rigidbody rb;
    protected BoxCollider bc;

    protected Bounds objBounds;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        objBounds = bc.bounds;
        gameObject.layer = 8;
    }

    protected virtual void Update()
    {
        if (transform.position.y < -1)
            Destroy(gameObject);
    }

    public virtual void OnItemInteract(PlayerController owningPlayer)
    {

        if (owningPlayer.holdingObject == null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.isKinematic = true;
            bc.enabled = false;
            rb.interpolation = RigidbodyInterpolation.None;
         

            gameObject.transform.SetParent(owningPlayer.Cam.transform);
            owningPlayer.holdingObject = GetComponent<IInteractable>();
        }
        else if (transform.localPosition.z > 0.15f)
        {
            gameObject.transform.SetParent(null);
            owningPlayer.holdingObject = null;

            bc.enabled = true;
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        }
    }


    public virtual void UpdateObjectOffset(float newPosistion)
    {
        transform.localPosition = new Vector3(0, 0, Mathf.Clamp(newPosistion - objBounds.extents.z, GameManager.objectInteractDistance[0], GameManager.objectInteractDistance[1]));
    }

    public virtual void OnItemRightMouseButton(PlayerController owningPlayer)
    {
        OnItemInteract(owningPlayer);

        rb.AddForce(owningPlayer.Cam.transform.forward * 2.5f, ForceMode.Impulse);
    }

}
