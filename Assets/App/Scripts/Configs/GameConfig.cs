using System.Collections.Generic;
using App.Scripts.ServiceLocator;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfig: ScriptableObject, IGameService
    {
        [SerializeField] private List<Vector2Int> _whiteUnitsStartPosition;
        [SerializeField] private List<Vector2Int> _blackUnitsStartPositions;

        public List<Vector2Int> BlackUnitsStartPositions => _blackUnitsStartPositions;
        public List<Vector2Int> WhiteUnitsStartPosition => _whiteUnitsStartPosition;
    }
}