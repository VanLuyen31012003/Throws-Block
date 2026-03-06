using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
   Dictionary<ETypeBlock, Queue<GameObject>> poolDictionary = new Dictionary<ETypeBlock, Queue<GameObject>>();
   
    public GameObject GetObject(ETypeBlock type,GameObject gamePrefap)
    {
         if (!poolDictionary.ContainsKey(type))
         {
            poolDictionary.Add(type, new Queue<GameObject>());
         }
         if (poolDictionary[type].Count > 0)
         {
              GameObject obj = poolDictionary[type].Dequeue();
              obj.SetActive(true);
              return obj;
          }
         else
         {
              GameObject obj = Instantiate(gamePrefap,this.transform);
              obj.SetActive(true);
            return obj;
        }
    }
    public  void ReturnObject(ETypeBlock type, GameObject obj)
    {
         if (!poolDictionary.ContainsKey(type))
         {
              poolDictionary[type] = new Queue<GameObject>();
         }
         obj.SetActive(false);
        obj.transform.localScale=Vector3.one;
        obj.transform.SetParent(this.transform);
        poolDictionary[type].Enqueue(obj);
    }

}
