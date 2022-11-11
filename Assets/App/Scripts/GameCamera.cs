using App.Scripts.ServiceLocator;
using UnityEngine;

namespace App.Scripts
{
    public class GameCamera: MonoBehaviour, IGameService
    {
        [field: SerializeField] public Camera Camera { get; private set; }
    }
}