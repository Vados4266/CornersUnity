using App.Scripts.ServiceLocator;
using App.Scripts.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class UIPauseMenu: UIWidget
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _quitButton;

        private void Start()
        {
            _resumeButton.onClick.AddListener(Resume);
            _mainMenuButton.onClick.AddListener(GoToMainMenu);
            _quitButton.onClick.AddListener(AppQuit);
        }

        public override void Open()
        {
            StaticServiceLocator.Get<AppController>().PauseGame(true);
            base.Open();
        }

        public override void Close()
        {
            StaticServiceLocator.Get<AppController>().PauseGame(false);
            base.Close();
        }

        private void Resume()
        {
            Close();
            GetWidget<UIHud>().Open();
        }

        private void GoToMainMenu()
        {
            Close();
            GetWidget<UIMainMenu>().Open();
            StaticServiceLocator.Get<AppController>().DisposeGame();
        }

        private void AppQuit()
        {
            StaticServiceLocator.Get<AppController>().Quit();
        }
    }
}