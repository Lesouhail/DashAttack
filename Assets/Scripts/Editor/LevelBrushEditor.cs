
namespace DashAttack.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [CustomGridBrush(false, false, false, "Level")]
    [CreateAssetMenu]
    public class LevelBrushEditor : GridBrushBase
    {
        [SerializeField] private LevelPrefab selectedPrefab;

        [Header("Prefabs")]
        [SerializeField] private GameObject pike;
        [SerializeField] private GameObject hole;
        [SerializeField] private GameObject staticCollidable;
        [SerializeField] private GameObject patrollingCollidable;

        public override void Paint(GridLayout grid, GameObject tileMap, Vector3Int position)
        {
            if (GetObjectInCell(tileMap.transform, position) != null)
            {
                return;
            }

            GameObject prefabToPaint = null;

            switch (selectedPrefab)
            {
                case LevelPrefab.Pike:
                    PaintPikes(tileMap, position);
                    return;

                case LevelPrefab.StaticCollidable:
                    prefabToPaint = staticCollidable;
                    break;

                case LevelPrefab.PatrollingCollidable:
                    prefabToPaint = patrollingCollidable;
                    break;

                case LevelPrefab.Hole:
                    prefabToPaint = hole;
                    break;
            }

            var cellCenter = new Vector3(position.x + .5f, position.y + .5f, 0);
            Instantiate(prefabToPaint, cellCenter, Quaternion.identity, tileMap.transform);
        }

        private void PaintPikes(GameObject tileMap, Vector3Int position)
        {
            var adjacentTiles = GetAdjacentTilesDirections(tileMap, position);

            if (adjacentTiles.Count != 1)
            {
                Debug.LogError("Please be nice and place pikes near exactly 1 adjacent tile");
                return;
            }

            var direction = adjacentTiles.First();

            float rotation = 0;
            Vector3 offset = Vector3.zero;

            switch (direction)
            {
                case Direction.Up:
                    rotation = 180;
                    offset = new Vector3(.5f, 1, 0);
                    break;

                case Direction.Down:
                    offset = new Vector3(.5f, 0, 0);
                    break;

                case Direction.Right:
                    rotation = 90;
                    offset = new Vector3(1, .5f, 0);
                    break;

                case Direction.Left:
                    rotation = -90;
                    offset = new Vector3(0, .5f, 0);
                    break;
            }

            Instantiate(pike, position + offset, Quaternion.AngleAxis(rotation, Vector3.forward), tileMap.transform);
        }

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            var objectInCell = GetObjectInCell(brushTarget.transform, position);
            if (objectInCell != null)
            {
                Undo.DestroyObjectImmediate(objectInCell.gameObject);
            }
        }

        private Transform GetObjectInCell(Transform tileMap, Vector3 position)
        {
            var cellCenter = position + new Vector3(.5f, .5f, 0);
            var bounds = new Bounds(cellCenter, Vector3.one / 2);

            var obj = tileMap
                .GetComponentsInChildren<Transform>()
                .FirstOrDefault(tr => bounds.Contains(tr.position));

            if (obj == null)
            {
                return null;
            }

            var parent = obj.parent;
            while (parent != tileMap)
            {
                obj = parent;
                parent = obj.parent;
            }

            return obj;
        }

        private List<Direction> GetAdjacentTilesDirections(GameObject parent, Vector3Int position)
        {
            var result = new List<Direction>(4);

            var tileMap = parent.GetComponent<Tilemap>();

            var posDirections = new Tuple<Vector3Int, Direction>[]
            {
                new Tuple<Vector3Int, Direction>(Vector3Int.FloorToInt(position + Vector3.up), Direction.Up),
                new Tuple<Vector3Int, Direction>(Vector3Int.FloorToInt(position + Vector3.down), Direction.Down),
                new Tuple<Vector3Int, Direction>(Vector3Int.FloorToInt(position + Vector3.left), Direction.Left),
                new Tuple<Vector3Int, Direction>(Vector3Int.FloorToInt(position + Vector3.right), Direction.Right)
            };

            foreach (var item in posDirections)
            {
                if (tileMap.HasTile(item.Item1))
                {
                    result.Add(item.Item2);
                }
            }

            return result;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum LevelPrefab
    {
        StaticCollidable = 0,
        PatrollingCollidable = 1,
        Hole = 2,
        Pike = 3,
    }
}
