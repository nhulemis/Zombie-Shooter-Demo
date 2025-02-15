using System.Collections.Generic;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace _1_Game.Scripts.Systems.AddressableSystem
{
    public class AssetLoader
    {
        private Dictionary<string, object> loadedAssets = new ();
        private Dictionary<string, AsyncOperationHandle>
            handleCache = new (); 
        
        public static UniTask<T> Load<T>(AssetReference key) where T : UnityEngine.Object
        {
            return Locator<AssetLoader>.Get().LoadAsset<T>(key);
        }
        
        public async UniTask<T> LoadAsset<T>(AssetReference assetReference) where T : UnityEngine.Object
        {
            if (!assetReference.RuntimeKeyIsValid())
            {
                Debug.LogError("Invalid AssetReference!");
                return null;
            }

            string key = assetReference.RuntimeKey.ToString();
            
            

            if (handleCache.TryGetValue(key, out AsyncOperationHandle existingHandle))
            {
                if (existingHandle.IsValid() && existingHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    return (T)existingHandle.Result;
                }
            }
            
            if(loadedAssets.TryGetValue(key, out object existingAsset))
            {
                return (T)existingAsset;
            }
            
            AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();
            handleCache[key] = handle;

            await handle.ToUniTask(); 

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Asset loaded successfully: {key}");
                loadedAssets[key] = handle.Result;
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
            string keyHash = key.RuntimeKey.ToString();
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
        
        public static async UniTask<GameObject> Instantiate(AssetReference key, Transform parent = null)
        {
            var prefab = await AssetLoader.Load<GameObject>(key);
            if (prefab == null)
            {
                Log.Debug("Failed to load prefab");
                return null;
            }
            
            var go = Object.Instantiate(prefab);
            if(parent != null)
            {
                go.transform.SetParent(parent);
                return go;
            }
            string targetSceneName = "GamePlay";
            Scene targetScene = SceneManager.GetSceneByName(targetSceneName);

            if (!targetScene.isLoaded)
            {
                Debug.LogError($"Scene {targetSceneName} is not loaded!");
                return null;
            }
            // 🔹 Move to the target scene
            SceneManager.MoveGameObjectToScene(go, targetScene);
            Debug.Log($"Instantiated {prefab.name} in {targetSceneName}");
            return go;
        }

        public static async UniTask<GameObject> Instantiate(AssetReference key, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            var go = await Instantiate(key, parent);
            if (go == null)
            {
                Log.Debug("Failed to instantiate prefab");
                return null;
            }

            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
        }
        
        public static async UniTask<GameObject> Instantiate(Object prefab, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            var go = Object.Instantiate(prefab).GameObject();
            
            if (parent != null)
            {
                go.transform.position = position;
                go.transform.rotation = rotation;
                go.transform.SetParent(parent);
                return go;
            }
            
            string targetSceneName = "GamePlay";
            Scene targetScene = SceneManager.GetSceneByName(targetSceneName);

            if (!targetScene.isLoaded)
            {
                Debug.LogError($"Scene {targetSceneName} is not loaded!");
                return null;
            }
            SceneManager.MoveGameObjectToScene(go, targetScene);
            go.transform.position = position;
            go.transform.rotation = rotation;
            Debug.Log($"Instantiated {prefab.name} in {targetSceneName}");
            return go;
            
            return go;
        }
    }
}