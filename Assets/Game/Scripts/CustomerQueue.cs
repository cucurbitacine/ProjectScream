using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CucuTools;
using DG.Tweening;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts
{
    public class CustomerQueue : MonoBehaviour
    {
        [SerializeField] private float period = 3f;
        [SerializeField] private float threshold = 3f;
        [SerializeField] private DesireSource desireSource;
        [SerializeField] private Transform exitPoint;

        [Space]
        [SerializeField] private GridController grid;

        [Space]
        [SerializeField] private GameObject customerPrefab;

        private float _timer;
        private readonly Dictionary<Customer, int> customerToTable = new Dictionary<Customer, int>();
        
        public void TryAddNewCustomer()
        {
            if (TryGetFreeTable(out var tableId))
            {
                StartCoroutine(AddCustomer(tableId));
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

        [SerializeField] private float customerSpeed = 4f; 
        [SerializeField] private float delay = 1f; 
        
        public IEnumerator AddCustomer(int tableId)
        {
            var tablePosition = grid.GetPositionByNumber(tableId);
                
            var customerObject = SmartPrefab.SmartInstantiate(customerPrefab, exitPoint.position, Quaternion.identity);
            var customer = customerObject.GetComponent<Customer>();
            
            customerToTable.Add(customer, tableId);
            customer.Completed += OnCustomerCompleted;
            
            customer.Ready();
            customer.SetDesireSource(desireSource);

            var duration = Vector2.Distance(exitPoint.position, tablePosition) / customerSpeed;
            customer.transform.DOMove(tablePosition, duration);
            yield return new WaitForSeconds(duration);
            
            yield return new WaitForSeconds(delay);
            
            customer.Activate();
        }

        public IEnumerator RemoveCustomer(Customer customer)
        {
            yield return new WaitForSeconds(delay);
            
            var duration = Vector2.Distance(exitPoint.position, customer.transform.position) / customerSpeed;
            customer.transform.DOMove(exitPoint.position, duration);
            yield return new WaitForSeconds(duration);
            
            customerToTable.Remove(customer);
            
            SmartPrefab.SmartDestroy(customer.gameObject);
        }
        
        private void OnCustomerCompleted(Customer customer)
        {
            StartCoroutine(RemoveCustomer(customer));
        }

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
