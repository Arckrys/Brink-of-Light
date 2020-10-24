using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionScript : MonoBehaviour
{
    private GameObject parent;

    public void SetParentGameObject(GameObject parent)
    {
        this.parent = parent;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(parent);
    }
}
