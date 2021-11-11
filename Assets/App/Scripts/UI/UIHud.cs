using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class UIHud: UIWidget, IDisposable
    {
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private Button _pauseButton;

        protected override void Awake()
        {
            base.Awake();
            _pauseButton.onClick.AddListener(ShowPause);
            CornersController.PlayerChanged += CornersControllerOnPlayerChanged;
            CornersController.PlayerWin += CornersControllerOnPlayerWin;
        }

        private void CornersControllerOnPlayerWin(PlayerType playerType)
        {
            ShowInfo($"Player win: {playerType}!!!");
        }

        private void CornersControllerOnPlayerChanged(PlayerType playerType)
        {
            ShowInfo($"Player turn: {playerType}");
        }

        private void ShowPause()
        {
            Close();
            GetWidget<UIPauseMenu>().Open();
        }

        private void ShowInfo(string message)
        {
            _infoText.text = message;
        }

        public void Dispose()
        {
            _pauseButton.onClick.RemoveListener(ShowPause);
            CornersController.PlayerChanged -= CornersControllerOnPlayerChanged;
            CornersController.PlayerWin -= CornersControllerOnPlayerWin;
        }
    }
}