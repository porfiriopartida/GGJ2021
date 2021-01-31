using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandComponent : MonoBehaviour
{
    public Transform GetItem()
    {
        return gameObject.transform.Find("Flag");
    }
}
