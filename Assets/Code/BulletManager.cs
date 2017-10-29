using System;
using System.Collections.Generic;
using Assets.Code.Structure;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Assets.Code
{
    /// <summary>
    /// Bullet manager for spawning and tracking all of the game's bullets
    /// </summary>
    public class BulletManager : ISaveLoad
    {
        private readonly Transform _holder;

        /// <summary>
        /// Bullet prefab. Use GameObject.Instantiate with this to make a new bullet.
        /// </summary>
        private readonly Object _bullet;

        public BulletManager (Transform holder) {
            _holder = holder;
            _bullet = Resources.Load("Bullet");
        }

        // TODO fill me in
        public void ForceSpawn (Vector2 pos, Quaternion rotation, Vector2 velocity, float deathtime)
        {
            var bill = (GameObject) Object.Instantiate(_bullet, pos, rotation, _holder);
            var bulletInit = bill.GetComponent<Bullet>();
            bulletInit.Initialize(velocity, deathtime);
        }

        #region saveload

        // TODO fill me in
        public GameData OnSave ()
        {
            BulletsData bData = new BulletsData();
            bData.Bullets = new List<BulletData>();
            var bList = (Bullet[]) Object.FindObjectsOfType(typeof(Bullet));
            if (bList != null)
            {
                foreach (var bull in bList)
                {
                    var currData = new BulletData();
                    currData.Pos = new Vector2();
                    currData.Velocity = new Vector2();
                    var rb2 = bull.gameObject.GetComponent<Rigidbody2D>();
                    currData.Pos = rb2.position;
                    currData.Velocity = rb2.velocity;
                    currData.Rotation = rb2.rotation;
                    bData.Bullets.Add(currData);
                }
            } 
            return bData;
        }

        // TODO fill me in
        public void OnLoad (GameData data)
        {
            var currBulls = (Bullet[]) Object.FindObjectsOfType(typeof(Bullet));
            if (currBulls != null)
            {
                foreach (var bull in currBulls)
                {
                    Object.Destroy(bull.gameObject);
                } 
            }
            var bData = (BulletsData) data;
            if (bData.Bullets != null)
            {
                foreach (var bull in bData.Bullets)
                {
                    ForceSpawn(bull.Pos, Quaternion.Euler(0, 0, bull.Rotation), bull.Velocity,
                        Time.time + Bullet.Lifetime);
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// Save data for all bullets in game
    /// </summary>
    public class BulletsData : GameData
    {
        public List<BulletData> Bullets;
    }

    /// <summary>
    /// Save data for a single bullet
    /// </summary>
    public class BulletData
    {
        public Vector2 Pos;
        public Vector2 Velocity;
        public float Rotation;
    }
}