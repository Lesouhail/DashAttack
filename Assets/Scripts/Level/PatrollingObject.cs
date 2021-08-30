namespace DashAttack.Level
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public class PatrollingObject : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float waitTime;
        private WayPointSpawner Spawner { get; set; }
        public bool IsWaiting { get; private set; }

        private void Start()
        {
            Spawner = GetComponentInParent<WayPointSpawner>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (Spawner.WayPointCount <= 1 || IsWaiting)
            {
                return;
            }

            var nextPosition = Vector3.MoveTowards(
                transform.position,
                Spawner.CurrentWayPoint.transform.position,
                moveSpeed * Time.deltaTime
            );

            var distanceToWayPoint = Vector2.Distance(transform.position, Spawner.CurrentWayPoint.transform.position);
            var distanceToNextPosition = Vector2.Distance(transform.position, nextPosition);

            var hasReachedWayPoint = distanceToNextPosition >= distanceToWayPoint;

            if (hasReachedWayPoint)
            {
                transform.position = Spawner.CurrentWayPoint.transform.position;
                Spawner.MoveNext();
                StartCoroutine(Wait());
            }

            transform.position = nextPosition;
        }

        private IEnumerator Wait()
        {
            IsWaiting = true;
            yield return new WaitForSeconds(waitTime);
            IsWaiting = false;
        }
    }
}
