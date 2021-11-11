using UnityEngine;

namespace App.Scripts.Behaviours
{
    public class Unit: BehaviourWrapper
    {
        [SerializeField] private PlayerType _playerType;
        public PlayerType PlayerType => _playerType;
        public static Unit Selected { get; private set; }
        
        private Vector3 _normalScale, _selectedScale;
        private const float SELECTED_SCALE_FACTOR = 1.25f;
        
        private void Start()
        {
            _normalScale = transform.localScale;
            _selectedScale = _normalScale * SELECTED_SCALE_FACTOR;
        }

        public void SetPosition(Vector2Int position)
        {
            transform.localPosition = new Vector3(position.x, 0f, position.y);
        }

        public Vector2Int Position
        {
            get
            {
                var localPosition = transform.localPosition;
                return new Vector2Int((int) localPosition.x, (int) localPosition.z);
            }
        }

        public void SetSelected(bool value)
        {
            if (Selected != this)
            {
                if (Selected != null) Selected.SetSelected(false);
            }

            if (value)
            {
                Selected = this;
                transform.localScale = _selectedScale;
            }
            else
            {
                Selected = null;
                transform.localScale = _normalScale;
            }
        }
    }
}