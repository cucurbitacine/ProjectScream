using System;

namespace Game.Scripts
{
    public class Wallet
    {
        private static Wallet _instance;

        public static Wallet Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = new Wallet();

                return _instance;
            }
        }
        
        public int Amount { get; private set; } = 0;

        public event Action<int> AmountChanged; 
        
        public bool Contains(int value)
        {
            return value <= Amount;
        }
        
        public bool Get(int value)
        {
            if (Contains(value))
            {
                if (value > 0)
                {
                    Amount -= value;
                    AmountChanged?.Invoke(Amount);
                }
                
                return true;
            }

            return false;
        }

        public void Add(int value)
        {
            if (value > 0)
            {
                Amount += value;
                AmountChanged?.Invoke(Amount);
            }
        }
    }
}
