using App.Scripts.Configs;
using App.Scripts.ServiceLocator;
using App.Scripts.UI;
using UnityEngine;

namespace App.Scripts
{
    public class AppController : MonoBehaviour, IGameService
    {
        [SerializeField] private GameResources _gameResources;
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private GameCamera _camera;
        [SerializeField] private Vector2Int _boardSize;
        
        [Header("Debug")]
        [SerializeField] private GameMode _debugGameMode;
        [SerializeField] private bool _debugAutoStart;
        [SerializeField] private bool _debugAi;
        
        private GameInput _gameInput;
        private CornersController _cornersController;
        
        private void Awake()
        {
            StaticServiceLocator.Add(this);
            StaticServiceLocator.Add(_camera);
            StaticServiceLocator.Add(_gameConfig);
            StaticServiceLocator.Add(_gameResources);
        }

        private void Start()
        {
            InitCamera();
            _gameInput = new GameInput();
            
            if (_debugAutoStart)
            {
                UIManager.CloseAll();
                StartGame(_debugGameMode, _debugAi);
            }
        }

        public void StartGame(GameMode gameMode, bool aiOpponent)
        {
            _cornersController = new CornersController(gameMode, aiOpponent, _boardSize);
        }
        
        public void PauseGame(bool value) => _cornersController.Pause(value);
        
        private void Update()
        {
            _gameInput.Update(Time.deltaTime); //TODO: add container (collection)
        }

        private void InitCamera()
        {
            var cameraTransform = _camera.transform;
            var position = cameraTransform.position;
            cameraTransform.position = new Vector3((_boardSize.x / 2f) - .5f, position.y, (_boardSize.y / 2f) - .5f);
        }
        
        public void DisposeGame()
        {
            _cornersController.Destroy();
        }
        
        public void Quit()
        {
#if UNITY_EDITOR
            Debug.LogError("UNITY_EDITOR");            
#else
            DisposeGame();
            Application.Quit();
#endif 
        }

        
    }
}
