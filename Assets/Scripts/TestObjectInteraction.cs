using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider),typeof(Rigidbody))]
public class TestObjectInteraction : MonoBehaviour,IInteractable
{
    public void OnItemInteract(PlayerController owningPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnItemRightMouseButton(PlayerController owningPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateObjectOffset(float newPosistion)
    {
        throw new System.NotImplementedException();
    }

    private void Reset()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
    }
}
