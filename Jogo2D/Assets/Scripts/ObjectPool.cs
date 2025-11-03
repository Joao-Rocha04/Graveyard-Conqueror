using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] inimigos; // Array com prefabs diferentes
    public int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            // Sorteia aleatoriamente um prefab
            GameObject prefabSorteado = inimigos[Random.Range(0, inimigos.Length)];

            GameObject obj = Instantiate(prefabSorteado);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject RequestObjectFromPool()
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.SetActive(true);
        }
        else
        {
            GameObject prefabSorteado = inimigos[Random.Range(0, inimigos.Length)];
            obj = Instantiate(prefabSorteado);
        }

        return obj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
