using System;
using App.Scripts.Interfaces;
using App.Scripts.Tools;
using UnityEngine;

namespace App.Scripts
{
    public class GameInput: IUpdatable
    {
        public static event Action<Vector2Int> CellSelected;
        private readonly Camera _camera;

        public GameInput()
        {
            _camera = SIContainer.Get<Camera>();
        }
        
        public void Update(float deltaTime)
        {
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) return;
            var position = RaycastCellPosition(_camera);
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