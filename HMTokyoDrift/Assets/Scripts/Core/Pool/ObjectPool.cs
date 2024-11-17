using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Core.Pool
{
    public class ObjectPool<T> where T : Component, IPoolable
    {
        private readonly T prefab;
        private readonly Transform container;
        private readonly Stack<T> availableObjects;
        private readonly HashSet<T> inUseObjects;
        private readonly int expandSize;
    
        private int totalObjectCount;
        private readonly int maxPoolSize;
        private bool isInitialized;

        public ObjectPool(T prefab, Transform container, int initialSize, int maxSize = 20, int expandSize = 5)
        {
            this.prefab = prefab;
            this.container = container;
            this.expandSize = expandSize;
            this.maxPoolSize = maxSize;
        
            availableObjects = new Stack<T>(initialSize);
            inUseObjects = new HashSet<T>();
        
            Initialize(initialSize);
        }

        private void Initialize(int size)
        {
            if (isInitialized) return;
        
            PreWarm(size);
            isInitialized = true;
        }

        private void PreWarm(int size)
        {
            for (int i = 0; i < size; i++)
            {
                if (totalObjectCount < maxPoolSize)
                {
                    CreateNewObject();
                }
            }
        }

        private T CreateNewObject()
        {
            T obj = GameObject.Instantiate(prefab, container);
            obj.gameObject.SetActive(false);
            availableObjects.Push(obj);
            totalObjectCount++;
            return obj;
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            T obj = GetObject();
            if (obj != null)
            {
                Transform objTransform = obj.transform;
                objTransform.SetPositionAndRotation(position, rotation);
                obj.gameObject.SetActive(true);
                obj.OnSpawn();
                inUseObjects.Add(obj);
            }
            return obj;
        }

        private T GetObject()
        {
            if (availableObjects.Count == 0 && totalObjectCount < maxPoolSize)
            {
                ExpandPool();
            }

            if (availableObjects.Count > 0)
            {
                return availableObjects.Pop();
            }

            return null;
        }

        private void ExpandPool()
        {
            int expandCount = Mathf.Min(expandSize, maxPoolSize - totalObjectCount);
            PreWarm(expandCount);
        }

        public void Return(T obj)
        {
            if (obj != null && inUseObjects.Remove(obj))
            {
                obj.OnDespawn();
                obj.gameObject.SetActive(false);
                availableObjects.Push(obj);
            }
        }

        public void ReturnAll()
        {
            if (inUseObjects.Count == 0) return;

            foreach (T obj in inUseObjects)
            {
                obj.OnDespawn();
                obj.gameObject.SetActive(false);
                availableObjects.Push(obj);
            }
            inUseObjects.Clear();
        }

        public void DestroyPool()
        {
            foreach (T obj in availableObjects)
            {
                if (obj != null)
                {
                    GameObject.Destroy(obj.gameObject);
                }
            }
        
            foreach (T obj in inUseObjects)
            {
                if (obj != null)
                {
                    GameObject.Destroy(obj.gameObject);
                }
            }

            availableObjects.Clear();
            inUseObjects.Clear();
            totalObjectCount = 0;
            isInitialized = false;
        }
    }
}