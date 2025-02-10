#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _1_Game.Scripts.DataConfig;
using Script.GameData;

namespace _1_Game.Scripts.Util
{
   

    public static class IDGetter
    {
        public static IEnumerable GetUIAnimNames()
        {
            var animation = SafetyDatabase.SafetyDB.Get<UIAnimationConfig>().UIAnimationDatas;
            var result = animation.Select(x => x.id);
            return result;
        }

        public static IEnumerable GetCameraNames()
        {
            var camera = SafetyDatabase.SafetyDB.Get<CameraConfig>().CamerasData;
            var result = camera.Select(x => x.name);
            return result;
        }

        public static IEnumerable GetUIGroupName()
        {
            return new List<string>(){"GameView", "SplashView"};
        }
    }
}
#endif