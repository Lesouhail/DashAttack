
namespace DashAttack.Level
{
    using System;
    using System.Linq;
    using UnityEngine;

    [ExecuteInEditMode]
    public class PatrollingObjectWayPoint : MonoBehaviour
    {
        public int Number
        {
            get
            {
                if (number <= 0)
                {
                    number = int.Parse(name.Split(' ')[1]);
                }
                return number;
            }

            set => number = value;
        }
        private int number;

        public event Action Destroyed
        {
            add
            {
                destroyed = null;
                destroyed += value;
            }
            remove
            {
                destroyed -= value;
            }
        }
        private Action destroyed;
        private void OnDestroy()
        {
            destroyed?.Invoke();
        }
    }

}
