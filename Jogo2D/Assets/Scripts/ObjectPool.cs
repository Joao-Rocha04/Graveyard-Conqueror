using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	public GameObject prefab; // O prefab que será instanciado
	public int poolSize = 10; // Tamanho do pool
	private Queue<GameObject> pool; // Fila
	
	void Start()
	{
			CreatePool();
	}
	
	// Cria os objetos e adiciona ao pool
	private void CreatePool()
	{
	    pool = new Queue<GameObject>();
	
	    for (int i = 0; i < poolSize; i++)
	    {
	        GameObject obj = Instantiate(prefab);
	        obj.SetActive(false); // Começa desativado
	        pool.Enqueue(obj);
	    }
	}
	
  // Requisita um objeto do pool
	public GameObject RequestObjectFromPool()
	{
      GameObject obj;
	    if (pool.Count > 0)
	    {
	        obj = pool.Dequeue();
	        obj.SetActive(true);
	        return obj;
	    }
	    
      // Se o pool estiver vazio, crie um novo objeto ou retornar null
      obj = Instantiate(prefab);
      return obj;
	}
	
  // Retorna um objeto ao pool
	public void ReturnObjectToPool(GameObject obj)
	{
	    obj.SetActive(false);
	    pool.Enqueue(obj);
	}
}