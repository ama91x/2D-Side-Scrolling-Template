using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMonobehaviour<PoolManager>
{
    [SerializeField] private Pool[] _poolArray = null;

    private Transform _objectPoolTransform;
    private Dictionary<int, Queue<Component>> _poolDictionary = new Dictionary<int, Queue<Component>>();

    [System.Serializable]
    public struct Pool
    {
        public int PoolSize;
        public GameObject Prefab;
        public string ComponentType;
    }

    private void Start()
    {
        _objectPoolTransform = this.gameObject.transform;

        for (int i = 0; i < _poolArray.Length; i++)
        {
            CreatePool(_poolArray[i].Prefab, _poolArray[i].PoolSize, _poolArray[i].ComponentType);
        }
    }

    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        int poolKey = prefab.GetInstanceID();

        string prefabName = prefab.name;

        GameObject parentGameObject = new GameObject(prefabName + "Anchor");

        parentGameObject.transform.SetParent(_objectPoolTransform);

        if (!_poolDictionary.ContainsKey(poolKey))
        {
            _poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject;

                newObject.SetActive(false);

                _poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));
            }
        }
    }

    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (_poolDictionary.ContainsKey(poolKey))
        {
            Component componentToReuse = GetComponentFormPool(poolKey);
            ResetObject(position, rotation, componentToReuse, prefab);

            return componentToReuse;
        }
        else
        {
            Debug.Log("No Object Pool For " + prefab);
            return null;
        }
    }

    private Component GetComponentFormPool(int poolKey)
    {
        Component componentToReuse = _poolDictionary[poolKey].Dequeue();

        _poolDictionary[poolKey].Enqueue(componentToReuse);

        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false);
        }

        return componentToReuse;
    }

    private void ResetObject(Vector3 position, Quaternion rotation, Component componenetToReuse, GameObject prefab)
    {
        componenetToReuse.transform.position = position;
        componenetToReuse.transform.rotation = rotation;
        componenetToReuse.gameObject.transform.localScale = prefab.transform.localScale;
    }

}
