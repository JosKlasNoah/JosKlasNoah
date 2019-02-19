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
       // if (transform.position.y < -1)
         //   Destroy(gameObject);
    }

    public virtual void OnItemInteract(PlayerController owningPlayer)
    {
        //als de speler geen object vast houdt
        if (owningPlayer.holdingObject == null)
        {
            //zet rigidbody collsion type
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;

            //disable box collider
            bc.enabled = false;

            //zet de parrant van dit object naar de player camera
            gameObject.transform.SetParent(owningPlayer.Cam.transform);
            //holding object staat gelijk aan het IInteractable interface van dit object
            owningPlayer.holdingObject = GetComponent<IInteractable>();
        }
        else if (transform.localPosition.z > GameManager.objectInteractDistance[0])
        {
            gameObject.transform.SetParent(null);
            owningPlayer.holdingObject = null;

            bc.enabled = true;
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

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
