using System;
using System.Collections;
using System.Collections.Generic;
using CucuTools;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using Game.Scripts.Effects;
using UnityEngine;

namespace Game.Scripts
{
    public class Pot : MonoBehaviour, IClickable, IFilter, IHighlightable
    {
        [SerializeField] private bool isCrafting;
        [SerializeField] private BookRecipe bookRecipe;

        [Space]
        [SerializeField] private ItemConfig failResult;
        [SerializeField] private Transform defaultSpawnPoint;

        [Space]
        [SerializeField] private AudioSfx clickSfx;
        [SerializeField] private AudioSfx highlightSfx;
        [SerializeField] private AudioSfx craftingSfx;
        
        [Space]
        [SerializeField] private GameObject destination;

        private IInventory _potInventory;
        private Coroutine _crafting;

        public event Action<float> ProgressChanged;

        public void Craft()
        {
            if (isCrafting) return;

            if (_crafting != null) StopCoroutine(_crafting);
            _crafting = StartCoroutine(Crafting());
        }

        public bool Filter(GameObject target)
        {
            return !isCrafting;
        }

        private Ingredient GetResult()
        {
            var stack = new List<Ingredient>();

            foreach (var item in _potInventory.GetItems())
            {
                var amount = _potInventory.CountItems(item);

                var itemStack = new Ingredient()
                {
                    amount = amount,
                    item = item,
                };

                stack.Add(itemStack);
            }

            if (bookRecipe && bookRecipe.FindRecipe(out var bestRecipe, stack))
            {
                return bestRecipe.GetResult();
            }

            if (failResult)
            {
                return new Ingredient()
                {
                    item = failResult,
                    amount = _potInventory.CountItems(),
                };
            }

            return default;
        }

        private IEnumerator Progress(float duration)
        {
            var timer = 0f;
            while (timer < duration)
            {
                ProgressChanged?.Invoke(timer / duration);

                timer += Time.deltaTime;
                yield return null;
            }

            ProgressChanged?.Invoke(1f);
        }

        private IEnumerator Crafting()
        {
            isCrafting = true;
            if (craftingSfx) gameObject.PlaySafe(craftingSfx.AudioClips);
            
            var result = GetResult();
            if (result.amount == 0)
            {
                isCrafting = false;
                gameObject.StopSafe();
                yield break;
            }

            if (destination &&
                destination.TryGetComponent(out IInventory destinationInventory) &&
                destinationInventory.CanPut(result.item, result.amount))
            {
                _potInventory.Clear();
                if (result.item is IDurationSource untilPutSomewhere && untilPutSomewhere.GetDuration() > 0f)
                {
                    yield return Progress(untilPutSomewhere.GetDuration());
                }

                destinationInventory.Put(result.item, result.amount);

                gameObject.Shake();
                isCrafting = false;
                gameObject.StopSafe();
                yield break;
            }

            _potInventory.Clear();
            if (result.item is IDurationSource untilPutOnScene && untilPutOnScene.GetDuration() > 0f)
            {
                yield return Progress(untilPutOnScene.GetDuration());
            }

            if (result.item is IPrefabSource prefabSource)
            {
                for (var i = 0; i < result.amount; i++)
                {
                    SmartPrefab.SmartInstantiate(prefabSource.GetPrefab(), defaultSpawnPoint.position, Quaternion.identity);
                }
            }

            gameObject.Shake();
            gameObject.StopSafe();
            isCrafting = false;
        }

        private void OnInventoryUpdated(IInventory inv, ISlot slt)
        {
            gameObject.Shake();
        }

        private void Awake()
        {
            TryGetComponent(out _potInventory);
        }

        private void OnEnable()
        {
            _potInventory.InventoryUpdated += OnInventoryUpdated;
        }

        private void OnDisable()
        {
            _potInventory.InventoryUpdated -= OnInventoryUpdated;
        }

        public bool CanBeClicked(GameObject actor)
        {
            return !isCrafting;
        }

        public void Click(GameObject actor)
        {
            Craft();

            gameObject.Shake();
            
            if (clickSfx) gameObject.PlayOneShot(clickSfx.AudioClips);
        }

        public void Highlight(bool value)
        {
            if (value && !isCrafting)
            {
                gameObject.Shake(0.5f);

                if (highlightSfx) gameObject.PlayOneShot(highlightSfx.AudioClips);
            }
        }
    }
}