using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Inputs
{
    [CreateAssetMenu(menuName = "Game/Inputs/Create Drag Input", fileName = "Drag Input", order = 0)]
    public class DragInput : ScriptableObject, GameInput.IDragActions
    {
        public Vector2 WorldPoint => GameInput != null ? ScreenToWorldPoint(GameInput.Drag.Point.ReadValue<Vector2>()) : Vector2.zero;

        public event Action<Vector2> WorldPointEvent; 
        public event Action<bool> ClickEvent; 
        
        private GameInput GameInput { get; set; }

        public Camera CameraMain => Camera.main;
        public Vector2 ScreenToWorldPoint(Vector2 screen) => CameraMain.ScreenToWorldPoint(screen);
        
        public void OnPoint(InputAction.CallbackContext context)
        {
            var screen = context.ReadValue<Vector2>();
            var worldPoint = ScreenToWorldPoint(screen);
            
            WorldPointEvent?.Invoke(worldPoint);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ClickEvent?.Invoke(true);
            }
            else
            {
                ClickEvent?.Invoke(false);
            }
        }

        private void OnEnable()
        {
            GameInput ??= new GameInput();
            
            GameInput.Drag.SetCallbacks(this);
            GameInput.Drag.Enable();
        }

        private void OnDisable()
        {
            GameInput.Drag.RemoveCallbacks(this);
            GameInput.Drag.Disable();
        }
    }
}