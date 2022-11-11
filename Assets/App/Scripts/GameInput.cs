using System;
using App.Scripts.Interfaces;
using App.Scripts.ServiceLocator;
using App.Scripts.Tools;
using UnityEngine;

namespace App.Scripts
{
    public class GameInput: IUpdatable
    {
        public static event Action<Vector2Int> CellSelected;
        private readonly GameCamera _camera;

        public GameInput()
        {
            _camera = StaticServiceLocator.Get<GameCamera>();
        }
        
        public void Update(float deltaTime)
        {
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) return;
            var position = RaycastCellPosition(_camera.Camera);
            if (!position.HasValue) return;
            CellSelected?.Invoke(position.Value);
        }
        
        private static Vector2Int? RaycastCellPosition(Camera camera)
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return null;
            var position = hit.collider.transform.position;
            return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        }
    }
}