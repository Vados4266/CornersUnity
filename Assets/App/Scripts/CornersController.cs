using System;
using System.Collections.Generic;
using App.Scripts.Behaviours;
using UnityEngine;
using IDisposable = App.Scripts.Interfaces.IDisposable;

namespace App.Scripts
{
    public class CornersController : IDisposable
    {
        private readonly Board _board;
        private readonly CellsHighlight _highlight;
        private List<Cell> _availableToMoveCells;
        private PlayerType _activePlayer;
        private readonly SimpleAI _ai;
        private bool _pause;

        public static event Action<PlayerType> PlayerChanged;
        public static event Action<PlayerType> PlayerWin;

        public CornersController(GameMode gameMode, bool aiOpponent, Vector2Int boardSize)
        {
            _board = new Board(boardSize, gameMode);
            _highlight = new CellsHighlight(boardSize.x + boardSize.y);

            if (aiOpponent)
            {
                _ai = new SimpleAI(PlayerType.Black, _board);
            }

            GameInput.CellSelected += OnCellSelected;

            SetActivePlayer(PlayerType.White);
        }

        private void OnCellSelected(Vector2Int position)
        {
            var cell = _board.GetCell(position);
            if (cell == null) return;
            ProcessLogic(cell);
        }

        private void ProcessLogic(Cell cell)
        {
            if (_pause) return;

            if (IsAiTurn()) return;

            var unit = _board.GetUnit(cell);
            if (unit != null)
            {
                if (unit.PlayerType != _activePlayer) return;

                unit.SetSelected(true);
                _availableToMoveCells = _board.GetAvailableMoves(cell);
                HighlightAvailableToMoveCells(_availableToMoveCells);
            }
            else
            {
                if (_availableToMoveCells == null || !_availableToMoveCells.Contains(cell) || Unit.Selected == null) return;

                _highlight.Hide();
                MakeMove(cell);

                if (!IsWinner(_activePlayer))
                {
                    SwitchPlayer();
                    if (IsAiTurn()) MakeAiMove();
                }
                else
                {
                    GameInput.CellSelected -= OnCellSelected;
                    PlayerWin?.Invoke(_activePlayer);
                }
            }
        }

        private void MakeAiMove()
        {
            MakeMove(_ai.CalcTurn());
            SwitchPlayer();
        }

        private void MakeMove(Cell cell)
        {
            Unit.Selected.SetPosition(cell.Position);
            Unit.Selected.SetSelected(false);
        }

        private bool IsAiTurn() => _ai != null && _ai.Type == _activePlayer;

        private void HighlightAvailableToMoveCells(List<Cell> cells) => _highlight.Show(cells);

        private void SwitchPlayer() => SetActivePlayer(NextPlayer());

        private PlayerType NextPlayer()
        {
            return _activePlayer == PlayerType.White ? PlayerType.Black : PlayerType.White;
        }

        private void SetActivePlayer(PlayerType playerType)
        {
            _activePlayer = playerType;
            PlayerChanged?.Invoke(_activePlayer);
        }

        private bool IsWinner(PlayerType player)
        {
            return _board.IsPlayerOnOpponentPositions(player == PlayerType.Black ? PlayerType.Black : PlayerType.White);
        }

        public void Pause(bool value) => _pause = value;

        public void Dispose()
        {
            GameInput.CellSelected -= OnCellSelected;
            _board.Dispose();
            _highlight.Dispose();
        }
    }
}