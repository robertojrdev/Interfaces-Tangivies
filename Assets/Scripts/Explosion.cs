using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private static Explosion instance;

    [SerializeField] private List<ExplosionItem> explosions = new List<ExplosionItem>();

    [System.Serializable]
    public class ExplosionItem
    {
        public string name;
        public GameObject explosion;
        public float timer;
    }

    public static void Explode(string name, Vector3 position)
    {
        if (!instance)
            return;

        ExplosionItem explosion = instance.explosions.Find(x => x.name == name);

        if (explosion != null)
        {
            GameObject obj = Instantiate(explosion.explosion, position, Quaternion.identity);
            Destroy(obj, explosion.timer);
        }
    }

    private void Awake()
    {
        if(!instance)
            instance = this;
    }

}
