
using System;
using UniRx;

namespace Game.UI
{
    public class LoadingSceneProvider : IDisposable
    {
        private ReactiveProperty<float> progress = new ReactiveProperty<float>();
        public ReactiveProperty<float> RxProgress => progress;
        
        public float Progress
        {
            get => progress.Value;
            set => progress.Value = value;
        }
        
        public void Dispose()
        {
        }

    }
}
