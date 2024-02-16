using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public GameObject objectToPool;
    public int amountToPool;

    private List<GameObject> pooledObjects;
    [HideInInspector] public List<GameObject> objectsBelow = new List<GameObject>();
    
    private void Awake()
    {
        StartCoroutine(UpdateChildrenListEverySecond());

        SharedInstance = this;
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
            tmp.transform.SetParent(transform); 
        }
    }
    private IEnumerator UpdateChildrenListEverySecond()
    {
        while (true)
        {
            objectsBelow.Clear(); 

            for (int i = 0; i < transform.childCount; i++)
            {
                objectsBelow.Add(transform.GetChild(i).gameObject);
            }

            yield return new WaitForSeconds(1f);
        }
    }
    public GameObject GetPooledObject(bool expandPoolIfNeeded)
    {
        foreach (var pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy)
            {
                return pooledObject;
            }
        }

        if (expandPoolIfNeeded)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(true);
            pooledObjects.Add(obj);
            return obj;
        }

        return null; 
    }
    private void ClearPool()
    {
        foreach (var pooledObject in pooledObjects)
        {
            Destroy(pooledObject); 
        }
        pooledObjects.Clear(); 
    }
}
