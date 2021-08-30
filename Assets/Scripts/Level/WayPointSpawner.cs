namespace DashAttack.Level
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [ExecuteInEditMode]
    public class WayPointSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject wayPointPrefab;
        [SerializeField] private PatrollingMode patrollingMode;
        public int WayPointCount { get; private set; }
        private bool MovingBackWards { get; set; } = true;
        public PatrollingObjectWayPoint CurrentWayPoint => CurrentWayPointNode.Value;
        private LinkedListNode<PatrollingObjectWayPoint> CurrentWayPointNode { get; set; }
        private GameObject WayPointsGroup => transform.Find("WayPoints").gameObject;
        private LinkedList<PatrollingObjectWayPoint> WayPoints
        {
            get
            {
                if (wayPoints == null || !wayPoints.Any())
                {
                    WayPointCount = 0;
                    var wp = WayPointsGroup.GetComponentsInChildren<PatrollingObjectWayPoint>()
                        .OrderBy(w => w.Number);

                    wayPoints = new LinkedList<PatrollingObjectWayPoint>();
                    foreach (var item in wp)
                    {
                        RegisterDestroy(item);
                        wayPoints.AddLast(item);
                        WayPointCount++;
                    }
                }

                return wayPoints;
            }
        }
        private LinkedList<PatrollingObjectWayPoint> wayPoints;

        public void MoveNext()
        {
            switch (patrollingMode)
            {
                case PatrollingMode.BackAndForth:
                    bool isLastWayPoint = CurrentWayPoint.Number == 1 || CurrentWayPoint.Number == WayPointCount;
                    if (isLastWayPoint)
                    {
                        MovingBackWards = !MovingBackWards;
                    }

                    CurrentWayPointNode = MovingBackWards
                        ? CurrentWayPointNode.Previous
                        : CurrentWayPointNode.Next;

                    break;

                case PatrollingMode.Cycling:
                    if (CurrentWayPoint.Number + 1 > WayPointCount)
                    {
                        CurrentWayPointNode = WayPoints.First;
                    }
                    else
                    {
                        CurrentWayPointNode = CurrentWayPointNode.Next;
                    }
                    break;
            }
        }

        private void Start()
        {
            if (WayPoints.Any())
            {
                CurrentWayPointNode = WayPoints.First;
                WayPointCount = WayPoints.Count;
            }
        }

        private void RegisterDestroy(PatrollingObjectWayPoint wayPoint)
        {
            wayPoint.Destroyed += () =>
            {
                var next = WayPoints.Find(wayPoint).Next;
                while (next != null)
                {
                    var value = next.Value;
                    value.Number--;
                    value.gameObject.name = $"WayPoint {value.Number}";
                    next = next.Next;
                }

                WayPoints.Remove(wayPoint);
                WayPointCount -= 1;
            };
        }

#if UNITY_EDITOR
        public void AddWayPoint()
        {
            var wayPoint = Instantiate(wayPointPrefab, transform.position, Quaternion.identity, WayPointsGroup.transform)
                .GetComponent<PatrollingObjectWayPoint>();

            wayPoint.Number = ++WayPointCount;
            wayPoint.gameObject.name = $"WayPoint {wayPoint.Number}";
            RegisterDestroy(wayPoint);

            WayPoints.AddLast(wayPoint);
        }
#endif
    }

    public enum PatrollingMode
    {
        BackAndForth = 0,
        Cycling = 1
    }

}