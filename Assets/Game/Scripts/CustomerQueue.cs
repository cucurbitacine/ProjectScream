using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CucuTools;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts
{
    public class CustomerQueue : MonoBehaviour
    {
        [SerializeField] private float period = 3f;
        [SerializeField] private float threshold = 3f;
        [SerializeField] private DesireSource desireSource;

        [Space]
        [SerializeField] private GridController grid;

        [Space]
        [SerializeField] private GameObject customerPrefab;

        private readonly Dictionary<Customer, int> customerToTable = new Dictionary<Customer, int>();
        
        public void TryAddNewCustomer()
        {
            if (TryGetFreeTable(out var tableId))
            {
                AddCustomer(tableId);
            }
        }

        public bool TryGetFreeTable(out int tableId)
        {
            for (var id = 0; id < grid.Length; id++)
            {
                if (customerToTable.All(pair => pair.Value != id))
                {
                    tableId = id;
                    return true;
                }
            }

            tableId = -1;
            return false;
        }

        public async void AddCustomer(int tableId)
        {
            var tablePosition = grid.GetPositionByNumber(tableId);
                
            var customerObject = SmartPrefab.SmartInstantiate(customerPrefab, tablePosition, Quaternion.identity);
            var customer = customerObject.GetComponent<Customer>();
            
            customerToTable.Add(customer, tableId);
            customer.Completed += OnCustomerCompleted;
            
            customer.Ready();
            customer.SetDesireSource(desireSource);
            
            // Wait
            await Task.Delay(Random.Range(1000, 3000));
            
            customer.Activate();
        }

        public async void RemoveCustomer(Customer customer)
        {
            // Wait
            await Task.Delay(Random.Range(1000, 3000));
            
            customerToTable.Remove(customer);
            
            SmartPrefab.SmartDestroy(customer.gameObject);
        }
        
        private void OnCustomerCompleted(Customer customer)
        {
            RemoveCustomer(customer);
        }

        private float _timer;

        private void Awake()
        {
            _timer = period;
        }

        private void Update()
        {
            if (_timer < 0)
            {
                _timer = period * Random.value * threshold;
                
                TryAddNewCustomer();
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }
    } 
}
