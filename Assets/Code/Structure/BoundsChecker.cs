using UnityEngine;

namespace Assets.Code.Structure
{
    /// <inheritdoc />
    /// <summary>
    /// Utility class that moves things from one side of the screen to the other.
    /// It's an Asteroids clone!
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class BoundsChecker : MonoBehaviour
    {
        private static Vector4 _bounds;

        private Rigidbody2D _rb;
        internal void Start () {
            _rb = GetComponent<Rigidbody2D>();

            var cam = Camera.main;
            var bottomleft = cam.ViewportToWorldPoint(Vector3.zero);
            var topright = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

            _bounds.x = bottomleft.x;
            _bounds.y = bottomleft.y;
            _bounds.z = topright.x;
            _bounds.w = topright.y;
        }

        internal void Update () {
            var pos = _rb.position;

            if (pos.x < _bounds.x) {
                pos.x = _bounds.z;
            } else if (pos.x > _bounds.z) {
                pos.x = _bounds.x;
            }

            if (pos.y < _bounds.y) {
                pos.y = _bounds.w;
            } else if (pos.y > _bounds.w) {
                pos.y = _bounds.y;
            }

            _rb.position = pos;
        }

        /// <summary>
        /// Returns a random valid screen position.
        /// Does _not_ check to see if anything is there.
        /// </summary>
        public static Vector2 GetRandomPos () {
            float x = Random.Range(_bounds.x, _bounds.z);
            float y = Random.Range(_bounds.y, _bounds.w);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns a random Vec2
        /// </summary>
        /// <param name="intensity">The scaling of the generated Vector2 (in both the x and the y direction)</param>
        /// <returns></returns>
        public static Vector2 GetRandomVelocity (float intensity = 1f) {
            return intensity * new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        /// <summary>
        /// Returns a random Angular Velocity (in degrees per second)
        /// </summary>
        /// <returns></returns>
        public static float GetRandomAngularVelocity () {
            return Random.Range(25, 75) * (Random.value > 0.5f ? -1f : 1f);
        }
    }
}
