using System.Collections.Generic;
using System.Linq;
using App.Scripts.Behaviours;
using UnityEngine;

namespace App.Scripts
{
    public class SimpleAI
    {
        public PlayerType Type { get; }
        private readonly Board _board;
        
        public SimpleAI(PlayerType type, Board board)
        {
            Type = type;
            _board = board;
        }

        private Cell GetNearestToTargetCell(Cell target, List<Cell> cells)
        {
            var result = cells.FirstOrDefault();
            float minDistance = float.MaxValue;
            foreach (var cell in cells)
            {
                var distance = Vector2Int.Distance(target.Position, cell.Position);
                if (!(distance < minDistance)) continue;
                minDistance = distance;
                result = cell;
            }

            return result;
        }
        
        private void SelectUnit()
        {
            var unit = _board.GetRandomUnit(Type);
            unit.SetSelected(true);
        }
        
        public Cell CalcTurn()
        {
            var availableMoves = new List<Cell>();
            while (availableMoves.Count <= 0)
            {
                SelectUnit();
                var unitCell = _board.GetCell(Unit.Selected.Position);
                availableMoves = _board.GetAvailableMoves(unitCell);
            }

            //var targetCell = _board.GetRandomStartCell(Type == PlayerType.Black ? PlayerType.White : PlayerType.Black);
            var targets = _board.GetEmptyStartPositions(Type == PlayerType.Black ? PlayerType.White : PlayerType.Black);
            var targetCell = targets[Random.Range(0, targets.Count)];
            return GetNearestToTargetCell(targetCell, availableMoves);
        }
    }
}