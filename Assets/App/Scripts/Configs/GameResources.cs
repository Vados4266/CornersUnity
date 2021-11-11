using App.Scripts.Behaviours;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameResources", menuName = "ScriptableObjects/GameResources", order = 1)]
    public class GameResources : ScriptableObject
    {
        [Header("Cells")]
        [SerializeField] private Cell _cellBlack;
        [SerializeField] private Cell _cellWhite;
        
        [Header("Units")]
        [SerializeField] private Unit _unitBlack;
        [SerializeField] private Unit _unitWhite;

        [Header("Staff")] [SerializeField] private GameObject _cellHighlighter;
        
        public Cell CellBlack => _cellBlack;
        public Cell CellWhite => _cellWhite;
        
        public Unit UnitBlack => _unitBlack;
        public Unit UnitWhite => _unitWhite;

        public GameObject CellHighlighter => _cellHighlighter;
    }
}