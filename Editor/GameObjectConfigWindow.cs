using UnityEditor; // Thư viện Unity Editor, cung cấp các API để tạo giao diện tùy chỉnh trong Unity Editor
using UnityEngine; // Thư viện Unity Engine, cung cấp các API cơ bản của Unity
using Demo.GameObjectConfig; // Tham chiếu namespace chứa GameObjectSpawner
using System.IO;
using System.Linq;

namespace Demo.GameObjectConfig
{
    public class GameObjectConfigWindow : EditorWindow
    {
        private string[] prefabPaths;
        private GameObject[] prefabs;

        private int playerIndex = 0;
        private int planeIndex = 0;
        private int obstacleIndex = 0;

        private int numPlayers = 1;
        private int numPlanes = 1;
        private int numObstacles = 5;

        [MenuItem("Tools/GameObject Config")]
        public static void ShowWindow()
        {
            GetWindow<GameObjectConfigWindow>("GameObject Config");
        }

        private void OnEnable()
        {
            LoadPrefabs();
        }

        void LoadPrefabs()
        {
            string prefabDir = "Packages/com.demo.gameobjectconfig/Prefabs";
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabDir });

            prefabPaths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            prefabs = prefabPaths.Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path)).ToArray();
        }

        private void OnGUI()
        {
            GUILayout.Label("Select Prefabs", EditorStyles.boldLabel);

            if (prefabs.Length == 0)
            {
                EditorGUILayout.HelpBox("No prefabs found in Prefabs folder.", MessageType.Warning);
                return;
            }

            string[] prefabNames = prefabs.Select(p => p.name).ToArray();

            playerIndex = EditorGUILayout.Popup("Player", playerIndex, prefabNames);
            planeIndex = EditorGUILayout.Popup("Plane", planeIndex, prefabNames);
            obstacleIndex = EditorGUILayout.Popup("Obstacle", obstacleIndex, prefabNames);

            numPlayers = EditorGUILayout.IntField("Number of Players", numPlayers);
            numPlanes = EditorGUILayout.IntField("Number of Planes", numPlanes);
            numObstacles = EditorGUILayout.IntField("Number of Obstacles", numObstacles);

            if (GUILayout.Button("Generate"))
            {
                var go = new GameObject("Spawner");
                var spawner = go.AddComponent<GameObjectSpawner>();
                spawner.playerPrefab = prefabs[playerIndex];
                spawner.planePrefab = prefabs[planeIndex];
                spawner.obstaclePrefab = prefabs[obstacleIndex];
                spawner.numPlayers = numPlayers;
                spawner.numPlanes = numPlanes;
                spawner.numObstacles = numObstacles;
                spawner.Spawn();
            }
        }
    }
}
