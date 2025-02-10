using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace _1_Game.Scripts.Systems
{
    public class InitDatabaseJob : ICommand
    {
        public async UniTask Execute()
        {
            var database = Resources.Load("DataBase", typeof(GameDataBase)) as GameDataBase;
            Assert.IsNotNull(database, "DataBase is null");
            Locator<GameDataBase>.Set(database);
            await UniTask.CompletedTask;
        }
    }
}