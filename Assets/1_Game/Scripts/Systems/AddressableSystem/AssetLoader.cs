using System;
using System.Collections.Generic;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UniRx;

namespace _1_Game.Scripts.Systems.AddressableSystem
{
    public class AssetLoader
    {
        private Dictionary<int, AsyncOperationHandle>
            handleCache = new (); 
        
        public static UniTask<T> Load<T>(AssetReference key) where T : UnityEngine.Object
        {
            return Locator<AssetLoader>.Get().LoadAsset<T>(key);
        }
        
        public async UniTask<T> LoadAsset<T>(AssetReference assetReference) where T : UnityEngine.Object
        {
            int keyHash = assetReference.RuntimeKey.GetHashCode();
            if (!assetReference.RuntimeKeyIsValid())
            {
                Debug.LogError("Invalid AssetReference!");
                return null;
            }

            string key = assetReference.RuntimeKey.ToString();

            if (handleCache.TryGetValue(keyHash, out AsyncOperationHandle existingHandle))
            {
                if (existingHandle.IsValid() && existingHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log($"Using cached asset: {key}");
                    return (T)existingHandle.Result;
                }
            }
            
            AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();
            handleCache[keyHash] = handle;

            await handle.ToUniTask(); 

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Asset loaded successfully: {key}");
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load asset: {key}");
                return null;
            }
        }

        public void ReleaseAsset(AssetReference key)
        {
            int keyHash = key.RuntimeKey.GetHashCode();
            if (handleCache.TryGetValue(keyHash, out var handle))
            {
                Addressables.Release(handle);
                handleCache.Remove(keyHash);
                Debug.Log($"Released asset: {key}");
            }
        }

        public void ReleaseAll()
        {
            foreach (var handle in handleCache.Values)
            {
                Addressables.Release(handle);
            }
            handleCache.Clear();
            Debug.Log("Released all Addressable assets.");
        }
    }
}