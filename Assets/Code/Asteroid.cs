using Assets.Code.Structure;
using UnityEngine;

namespace Assets.Code
{
    /// <summary>
    /// Code for an on-screen asteroid
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Asteroid : MonoBehaviour
    {
        public const int AsteroidTypes = 6;
        public static AsteroidManager Manager;

        public int Size;

        private Transform _sizes;
        private bool Small { get { return Size < 2; } }

        public void Initialize (Vector2 velocity, int size) {
            Size = size;
            _sizes = transform.Find("Sizes");
            SetSize();

            var rb = GetComponent<Rigidbody2D>();

            rb.velocity = velocity;
            rb.angularVelocity = BoundsChecker.GetRandomAngularVelocity();
        }

        private void SetSize () {
            for (int i = 0, count = _sizes.childCount; i < count; i++) {
                _sizes.GetChild(i).gameObject.SetActive(false); // turn them all off
            }
            _sizes.GetChild(Size).gameObject.SetActive(true); // let the right one in
        }

        // TODO fill me in
        internal void OnCollisionEnter2D (Collision2D other)
        {
            var collidedObj = other.gameObject;
            if (collidedObj.GetComponent<Bullet>())
            {
                HitBullet();
            }else if (collidedObj.GetComponent<Player>())
            {
                HitPlayer();
            }

        }

        private void HitPlayer () {
            Game.Score.AddScore(-2);
            Split();
        }

        private void HitBullet () {
            Game.Score.AddScore(1);
            Split();
        }

        private void Split () {
            if (Small) {
                Destroy(gameObject);
                return;
            }
            Size = Size - 2; // decrease to the next size down
            SetSize();
            var rb = GetComponent<Rigidbody2D>();
            rb.velocity = Quaternion.Euler(0f, -45f, 0f) * rb.velocity;
            Manager.ForceSpawn(rb.position + Vector2.one, -rb.velocity, Size);
        }
    }
}
