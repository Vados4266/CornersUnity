using System.Collections.Generic;
using App.Scripts.Behaviours;
using App.Scripts.Configs;
using App.Scripts.Interfaces;
using App.Scripts.ServiceLocator;
using App.Scripts.Tools;
using UnityEngine;

namespace App.Scripts
{
    public class CellsHighlight: IDestroyable
    {
        private readonly List<GameObject> _objects;
        private readonly Transform _highlighters;
        
        public CellsHighlight(int max)
        {
            _highlighters = new GameObject("Highlighters").transform;
            _objects = new List<GameObject>(max);
            for (var i = 0; i < max; i++)
            {
                _objects.Add(Utils.Instantiate(StaticServiceLocator.Get<GameResources>().CellHighlighter, Vector2Int.zero, _highlighters));
            }
        }

        public void Show(List<Cell> cells)
        {
            Hide();
            
            for (var i = 0; i < cells.Count; i++)
            {
                var position = cells[i].Position;
                var go = _objects[i];
                go.transform.localPosition = new Vector3(position.x, 0.1f, position.y);
                go.SetActive(true);
            }
        }

        public void Hide()
        {
            foreach (var gameObject in _objects)
            {
                gameObject.SetActive(false);
            }
        }

        public void Destroy()
        {
            _objects.Clear();
            Object.Destroy(_highlighters.gameObject);
        }
    }
}