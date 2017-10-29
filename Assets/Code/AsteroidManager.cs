using System;
using System.Collections.Generic;
using Assets.Code.Structure;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Code
{
    /// <inheritdoc><cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Manager class for spawning and tracking all of the game's asteroids
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AsteroidManager : MonoBehaviour, ISaveLoad
    {
        private const float SpawnTime = 3f;
        private const int MaxAsteroidCount = 8;
        private static Object _asteroidPrefab;
        private float _lastspawn;
        private Transform _holder;

        // ReSharper disable once UnusedMember.Global
        internal void Start () {
            _asteroidPrefab = Resources.Load("Asteroid");
            _holder = transform;
            Asteroid.Manager = this;
        }

        // ReSharper disable once UnusedMember.Global
        internal void Update () {
            if ((Time.time - _lastspawn) < SpawnTime) return;
            _lastspawn = Time.time;
            Spawn();
        }

        private void Spawn () {
            if (_holder.childCount >= MaxAsteroidCount) { return; }

            var pos = BoundsChecker.GetRandomPos();
            var vel = BoundsChecker.GetRandomVelocity();
            int size = Random.Range(2, Asteroid.AsteroidTypes); // don't spawn tinies

            ForceSpawn(pos, vel, size);
        }

        // TODO fill me in
        public void ForceSpawn (Vector2 pos, Vector2 velocity, int size, Quaternion rotation = new Quaternion())
        {
            var ast = (GameObject) Object.Instantiate(_asteroidPrefab, pos, rotation, _holder);
            var astInit = ast.GetComponent<Asteroid>();
            astInit.Initialize(velocity, size);
        }

        #region saveload

        // TODO fill me in
        public GameData OnSave () {
            var aData = new AsteroidsData();
            var aList = (Asteroid[]) Object.FindObjectsOfType(typeof(Asteroid));
            if (aList != null)
            {
                foreach (var ast in aList)
                {
                    var currData = new AsteroidData();
                    var rb2 = ast.GetComponent<Rigidbody2D>();
                    currData.Pos = rb2.position;
                    currData.Velocity = rb2.velocity;
                    currData.Size = ast.GetComponent<Asteroid>().Size;
                    aData.Asteroids.Add(currData);
                }
            }
            return aData; 
        }

        // TODO fill me in
        public void OnLoad (GameData data) { 
            var currAsts = (Asteroid[]) Object.FindObjectsOfType(typeof(Asteroid));
            if (currAsts != null)
            {
                foreach(var destAst in currAsts) 
                {
                    GameObject.Destroy(destAst.gameObject);
                }
            }
            var aData = (AsteroidsData) data;
            if(aData.Asteroids != null)
            {
                foreach (var ast in aData.Asteroids)
                {
                    ForceSpawn(ast.Pos, ast.Velocity, ast.Size);
                }
            } 
        }

        #endregion
    }

    /// <summary>
    /// The save data for all the asteroids
    /// </summary>
    public class AsteroidsData : GameData
    {
        public List<AsteroidData> Asteroids = new List<AsteroidData>();
    }

    /// <summary>
    /// The save data for one asteroid
    /// </summary>
    public class AsteroidData
    {
        public int Size;
        public Vector2 Pos;
        public Vector2 Velocity;
    }
}
