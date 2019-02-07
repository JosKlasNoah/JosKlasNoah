using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public void Remove(GameObject removeWhat)
    {
        removeWhat.SetActive(false);
    }
}
