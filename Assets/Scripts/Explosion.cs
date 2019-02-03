using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private static Explosion instance;

    [SerializeField] private GameObject explosion;
    [SerializeField] private float timer;

    public static void Explode(Vector3 position)
    {
        if (!instance)
            return;
        
        GameObject explosion = Instantiate(instance.explosion, position, Quaternion.identity);
        Destroy(explosion, instance.timer);
    }

    private void Awake()
    {
        if(!instance)
            instance = this;
    }

}
