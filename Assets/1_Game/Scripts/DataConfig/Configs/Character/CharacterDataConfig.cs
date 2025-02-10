using System;

namespace Script.GameData
{
    [Serializable]
    public class CharacterDataConfig : BaseRecord
    {
        public float Mass = 2f;
        public float MoveSpeed = 4f;
        public float JumpForce = 2f;
    }
}