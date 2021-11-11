using UnityEngine;

namespace App.Scripts.Behaviours
{
    public class BehaviourWrapper: MonoBehaviour
    {
        private Transform _transform;

        public Transform Transform => transform;

        public new Transform transform
        {
            get
            {
                if (_transform != null)
                {
                    return _transform;
                }

                _transform = base.transform;
                return _transform;
            }
        }
    }
}