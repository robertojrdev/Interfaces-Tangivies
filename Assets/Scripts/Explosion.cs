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
        if (instance)
            instance.StartCoroutine(instance.Instantiate_Routine(position));
    }

    private void Awake()
    {
        if(!instance)
            instance = this;
    }

    IEnumerator Instantiate_Routine(Vector3 position)
    {
        GameObject explosion = Instantiate(this.explosion, position, Quaternion.identity);
        yield return new WaitForSeconds(timer);
        Destroy(explosion);
    }

}
