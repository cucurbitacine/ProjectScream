using System;

namespace Game.Scripts
{
    public class Reputation
    {
        private static Reputation _instance;

        public static Reputation Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = new Reputation();

                return _instance;
            }
        }
        
        public int Level { get; private set; } = 0;

        public event Action<int> LevelChanged; 
        
        public bool Contains(int value)
        {
            return value <= Level;
        }
        
        public bool Get(int value)
        {
            if (Contains(value))
            {
                if (value > 0)
                {
                    Level -= value;
                    LevelChanged?.Invoke(Level);
                }
                
                return true;
            }

            return false;
        }

        public void Add(int value)
        {
            if (value > 0)
            {
                Level += value;
                LevelChanged?.Invoke(Level);
            }
        }
    }
}