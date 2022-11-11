using System.Collections.Generic;
using System.Linq;
using App.Scripts.Behaviours;
using App.Scripts.Configs;
using App.Scripts.Interfaces;
using App.Scripts.ServiceLocator;
using App.Scripts.Tools;
using UnityEngine;

namespace App.Scripts
{
    public class Board : IDestroyable
    {
        private readonly List<Cell> _cells = new List<Cell>();
        private readonly List<Unit> _units = new List<Unit>();
        private readonly List<Unit> _blackUnits = new List<Unit>();
        private readonly List<Unit> _whiteUnits = new List<Unit>();

        private readonly GameResources _resources;
        private readonly GameConfig _config;
        private readonly GameMode _gameMode;
        
        private readonly Vector2Int _size;
        private Transform _boardTransform;

        public Board(Vector2Int size, GameMode gameMode)
        {
            _size = size;
            _gameMode = gameMode;
            _resources = StaticServiceLocator.Get<GameResources>();
            _config = StaticServiceLocator.Get<GameConfig>();

            Init();
        }

        public void Destroy()
        {
            Object.Destroy(_boardTransform.gameObject);
        }

        private void Init()
        {
            _boardTransform = new GameObject("Board").transform;
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    var prefab = (x + y) % 2 == 1 ? _resources.CellWhite : _resources.CellBlack;
                    var cell = Utils.Instantiate(prefab, new Vector2Int(x, y), _boardTransform);
                    cell.SetPosition(x, y);
                    _cells.Add(cell);
                }
            }

            foreach (var position in _config.BlackUnitsStartPositions)
            {
                _blackUnits.Add(InstantiateUnit(PlayerType.Black, position));
            }

            foreach (var position in _config.WhiteUnitsStartPosition)
            {
                _whiteUnits.Add(InstantiateUnit(PlayerType.White, position));
            }

            _units.AddRange(_blackUnits);
            _units.AddRange(_whiteUnits);
        }

        private Unit InstantiateUnit(PlayerType type, Vector2Int position)
        {
            var prefab = type == PlayerType.Black ? _resources.UnitBlack : _resources.UnitWhite;
            var unit = Utils.Instantiate(prefab, position, _boardTransform);
            unit.SetPosition(position);
            return unit;
        }

        public Unit GetUnit(Cell cell)
        {
            return _units.FirstOrDefault(u => u.Position == cell.Position);
        }

        public bool IsPlayerOnOpponentPositions(PlayerType playerType)
        {
            return playerType switch
            {
                PlayerType.Black => _blackUnits.All(unit => _config.WhiteUnitsStartPosition.Contains(unit.Position)),
                PlayerType.White => _whiteUnits.All(unit => _config.BlackUnitsStartPositions.Contains(unit.Position)),
                _ => false
            };
        }

        public Cell GetCell(Vector2Int position)
        {
            return _cells.FirstOrDefault(c => c.Position == position);
        }

        private bool IsCellEmpty(Cell cell)
        {
            return GetUnit(cell) == null;
        }

        private bool IsCellEmpty(Vector2Int position)
        {
            var cell = GetCell(position);
            return IsCellEmpty(cell);
        }

        private bool IsPositionOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.x >= _size.x || position.y < 0 || position.y >= _size.y;
        }

        public Unit GetRandomUnit(PlayerType playerType)
        {
            return playerType == PlayerType.Black
                ? _blackUnits[Random.Range(0, _blackUnits.Count)]
                : _whiteUnits[Random.Range(0, _whiteUnits.Count)];
        }

        public Cell GetRandomStartCell(PlayerType playerType)
        {
            return GetCell(playerType == PlayerType.Black
                ? _config.BlackUnitsStartPositions[Random.Range(0, _config.BlackUnitsStartPositions.Count)]
                : _config.WhiteUnitsStartPosition[Random.Range(0, _config.WhiteUnitsStartPosition.Count)]);
        }

        public List<Cell> GetEmptyStartPositions(PlayerType playerType)
        {
            var result = new List<Cell>();
            result.AddRange(playerType == PlayerType.Black
                ? _config.BlackUnitsStartPositions.Select(GetCell).Where(cell => GetUnit(cell) == null)
                : _config.WhiteUnitsStartPosition.Select(GetCell).Where(cell => GetUnit(cell) == null));

            return result;
        }

        public List<Cell> GetAvailableMoves(Cell cell)
        {
            var stack = new Stack<Cell>();
            var result = new List<Cell>();
            stack.Push(cell);
            var skipEmpty = false;

            while (stack.Count > 0)
            {
                cell = stack.Pop();

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        
                        switch (_gameMode)
                        {
                            case GameMode.VerticalHorizontal when Mathf.Abs(x) == Mathf.Abs(y):
                            case GameMode.Diagonal when Mathf.Abs(x) != Mathf.Abs(y):
                                continue;
                        }

                        var nextPosition = new Vector2Int(cell.Position.x + x, cell.Position.y + y);
                        if (IsPositionOutOfBounds(nextPosition)) continue;
                        if (IsCellEmpty(nextPosition))
                        {
                            if (!skipEmpty) result.Add(GetCell(nextPosition));
                        }
                        else
                        {
                            if (_gameMode == GameMode.Normal) continue;
                            
                            nextPosition = new Vector2Int(cell.Position.x + 2 * x, cell.Position.y + 2 * y);
                            if (IsPositionOutOfBounds(nextPosition)) continue;
                            if (!IsCellEmpty(nextPosition)) continue;
                            var c1 = GetCell(nextPosition);
                            if (result.Contains(c1)) continue;
                            result.Add(c1);
                            stack.Push(c1);
                        }
                    }
                }

                skipEmpty = true;
            }

            return result;
        }
    }
}