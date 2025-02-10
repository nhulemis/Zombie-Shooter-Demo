using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;

namespace Script.GameData
{
    public class CameraConfig : BaseConfig
    {
        public List<CameraData> CamerasData = new List<CameraData>();
        
        public CameraData GetCameraData(string cameraName)
        {
            return CamerasData.Find(data => data.name == cameraName);
        }
        
        public IEnumerable GetCameraNames()
        {
            return CamerasData.Select(x => x.name);
        }
    }

    public class CameraData
    {
        public string name;
        public Vector3 focusPoint;
        public float duration;
        public Ease ease;
    }
}