using System;
using System.Linq;
using App.Scripts.ServiceLocator;
using App.Scripts.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class UIMainMenu: UIWidget
    {
        [SerializeField] private TMP_Dropdown _modeDropdown;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Toggle _toggle;
        
        private GameMode _gameMode = GameMode.Normal;
        private bool _aiOpponent;
        private AppController _app => StaticServiceLocator.Get<AppController>();
        
        private void Start()
        {
            _modeDropdown.AddOptions(Enum.GetNames(typeof(GameMode)).ToList());
            _modeDropdown.onValueChanged.AddListener(ChangeGameMode);
            _playButton.onClick.AddListener(StartGame);
            _quitButton.onClick.AddListener(AppQuit);
            _toggle.onValueChanged.AddListener(SetOpponent);
        }

        private void AppQuit()
        {
            _app.Quit();
        }

        private void StartGame()
        {
            GetWidget<UIHud>().Open();
            _app.StartGame(_gameMode, _aiOpponent);
            Close();
        }

        private void SetOpponent(bool ai)
        {
            _aiOpponent = ai;
        }
        
        private void ChangeGameMode(int val)
        {
            _gameMode = (GameMode) val;
        }
    }
}