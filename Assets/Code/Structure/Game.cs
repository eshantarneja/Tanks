using UnityEngine;

namespace Assets.Code.Structure
{
    public class Game : MonoBehaviour
    {
        /// <summary>
        /// The game context.
        /// A pointer to the currently active game (so that we don't have to use something slow like "Find").
        /// </summary>
        public static Game Ctx;

        /// <summary>
        /// The class that handles serialization/deserialization
        /// </summary>
        public static SaveLoadManager Saveload;

        // 
        // all of the things that we can about saving/loading
        public static ScoreManager Score;
        public static Player Player;
        public static AsteroidManager Asteroids;
        public static BulletManager Bullets;


        internal void Start () {
            Ctx = this;

            Saveload = new SaveLoadManager();
            Score = GameObject.Find("ScoreText").GetComponent<ScoreManager>();
            Player = GameObject.Find("Player").GetComponent<Player>();
            Asteroids = GameObject.Find("Spawner").GetComponent<AsteroidManager>();
            Bullets = new BulletManager(GameObject.Find("Bullets").transform);

            _saveAxis = Platform.GetSaveAxis();
        }

        // all of this is done so that you can save/load with the Start/Back buttons
        private static string _saveAxis;
        private bool _locked;
        internal void Update () {
            float axis = Input.GetAxis(_saveAxis);
            if (_locked && Mathf.Abs(axis) < 0.1f) { _locked = false; }
            if (_locked) { return; }

            if (axis > 0.1f) {
                Saveload.Save();
                _locked = true;
            } else if (axis < -0.1f) {
                Saveload.Load();
                _locked = true;
            }
        }

        /// <summary>
        /// Take the loaded data and initialize everything appropriately
        /// </summary>
        /// <param name="data">The GameData object containing all of the loaded values</param>
        public void LoadData (SaveLoadManager.SaveData data) {
            Score.OnLoad(data.Score);
            Player.OnLoad(data.Player);
            Asteroids.OnLoad(data.Asteroids);
            Bullets.OnLoad(data.Bullets);
        }

        private static bool IsMac () {
            return Application.platform == RuntimePlatform.OSXEditor ||
                   Application.platform == RuntimePlatform.OSXPlayer;
        }
    }
}