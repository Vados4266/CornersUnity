using UnityEngine;

namespace App.Scripts.Behaviours
{
    public class Cell: BehaviourWrapper
    {
        [SerializeField] private TMPro.TMP_Text _debugText;
        public Vector2Int Position { get; private set; }
        public void SetCoords(int x, int y)
        {
            Position = new Vector2Int(x, y);
            _debugText.text = $"x: {Position.x} | y: {Position.y}";
        }
    }
}