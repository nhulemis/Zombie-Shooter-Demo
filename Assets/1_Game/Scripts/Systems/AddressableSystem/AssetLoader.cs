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
        private Dictionary<int, object> loadedAssets = new (); // Cache for assets

        private Dictionary<int, AsyncOperationHandle>
            handleCache = new (); 
        
        public static UniTask<T> Load<T>(AssetReference key) where T : UnityEngine.Object
        {
            return Locator<AssetLoader>.Get().LoadAsset<T>(key);
        }
        
        public async UniTask<T> LoadAsset<T>(AssetReference key) where T : UnityEngine.Object
        {
            int keyHash = key.RuntimeKey.GetHashCode();
            if (loadedAssets.TryGetValue(keyHash, out var cachedAsset))
            {
                return (T)cachedAsset;
            }

            var handle = key.LoadAssetAsync<T>();
            await handle.ToUniTask(); 

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                loadedAssets[keyHash] = handle.Result;
                handleCache[keyHash] = handle;
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
                loadedAssets.Remove(keyHash);
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

            loadedAssets.Clear();
            handleCache.Clear();
            Debug.Log("Released all Addressable assets.");
        }
    }
}