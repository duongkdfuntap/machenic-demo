using UnityEditor; // Thư viện Unity Editor, cung cấp các API để tạo giao diện tùy chỉnh trong Unity Editor
using UnityEngine; // Thư viện Unity Engine, cung cấp các API cơ bản của Unity
using Demo.GameObjectConfig; // Tham chiếu namespace chứa GameObjectSpawner
using System.IO; // Thư viện hỗ trợ thao tác với hệ thống tệp
using System.Linq; // Thư viện LINQ, cung cấp các phương thức mở rộng như Select

namespace Demo.GameObjectConfig // Không gian tên để tổ chức mã nguồn
{
    public class GameObjectConfigWindow : EditorWindow // Lớp kế thừa từ EditorWindow để tạo cửa sổ tùy chỉnh trong Unity Editor
    {
        private string[] prefabPaths; // Mảng lưu trữ đường dẫn đến các prefab
        private GameObject[] prefabs; // Mảng lưu trữ các đối tượng prefab đã tải

        private int playerIndex = 0; // Chỉ số của prefab người chơi được chọn
        private int planeIndex = 0; // Chỉ số của prefab máy bay được chọn
        private int obstacleIndex = 0; // Chỉ số của prefab chướng ngại vật được chọn

        private int numPlayers = 1; // Số lượng người chơi, mặc định là 1
        private int numPlanes = 1; // Số lượng máy bay, mặc định là 1
        private int numObstacles = 5; // Số lượng chướng ngại vật, mặc định là 5

        [MenuItem("Tools/GameObject Config")] // Thêm mục "GameObject Config" vào menu "Tools" trong Unity Editor
        public static void ShowWindow() // Phương thức tĩnh để hiển thị cửa sổ
        {
            GetWindow<GameObjectConfigWindow>("GameObject Config"); // Mở hoặc tạo cửa sổ với tiêu đề "GameObject Config"
        }

        private void OnEnable() // Phương thức được gọi khi cửa sổ được kích hoạt
        {
            LoadPrefabs(); // Gọi phương thức để tải danh sách các prefab
        }

        void LoadPrefabs() // Phương thức để tải danh sách các prefab từ thư mục chỉ định
        {
            string prefabDir = "Packages/com.demo.gameobjectconfig/Prefabs"; // Đường dẫn đến thư mục chứa prefab
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabDir }); 
            // Tìm tất cả các asset có kiểu "Prefab" trong thư mục chỉ định, trả về danh sách GUID

            prefabPaths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray(); 
            // Chuyển đổi GUID thành đường dẫn asset và lưu vào mảng prefabPaths
            prefabs = prefabPaths.Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path)).ToArray(); 
            // Tải các prefab từ đường dẫn và lưu vào mảng prefabs
        }

        private void OnGUI() // Phương thức vẽ giao diện người dùng trong cửa sổ
        {
            GUILayout.Label("Select Prefabs", EditorStyles.boldLabel); // Hiển thị nhãn "Select Prefabs" với kiểu chữ đậm

            if (prefabs.Length == 0) // Kiểm tra nếu không có prefab nào được tìm thấy
            {
                EditorGUILayout.HelpBox("No prefabs found in Prefabs folder.", MessageType.Warning); 
                // Hiển thị thông báo cảnh báo nếu không tìm thấy prefab
                return; // Kết thúc phương thức nếu không có prefab
            }

            string[] prefabNames = prefabs.Select(p => p.name).ToArray(); 
            // Lấy tên của các prefab và lưu vào mảng prefabNames

            playerIndex = EditorGUILayout.Popup("Player", playerIndex, prefabNames); 
            // Tạo menu thả xuống để chọn prefab người chơi
            planeIndex = EditorGUILayout.Popup("Plane", planeIndex, prefabNames); 
            // Tạo menu thả xuống để chọn prefab máy bay
            obstacleIndex = EditorGUILayout.Popup("Obstacle", obstacleIndex, prefabNames); 
            // Tạo menu thả xuống để chọn prefab chướng ngại vật

            numPlayers = EditorGUILayout.IntField("Number of Players", numPlayers); 
            // Tạo trường nhập số lượng người chơi
            numPlanes = EditorGUILayout.IntField("Number of Planes", numPlanes); 
            // Tạo trường nhập số lượng máy bay
            numObstacles = EditorGUILayout.IntField("Number of Obstacles", numObstacles); 
            // Tạo trường nhập số lượng chướng ngại vật

            if (GUILayout.Button("Generate")) // Tạo nút "Generate", nếu được nhấn thì thực hiện khối lệnh bên dưới
            {
                var go = new GameObject("Spawner"); // Tạo một đối tượng mới trong cảnh với tên "Spawner"
                var spawner = go.AddComponent<GameObjectSpawner>(); // Thêm thành phần GameObjectSpawner vào đối tượng "Spawner"
                spawner.playerPrefab = prefabs[playerIndex]; // Gán prefab người chơi được chọn vào spawner
                spawner.planePrefab = prefabs[planeIndex]; // Gán prefab máy bay được chọn vào spawner
                spawner.obstaclePrefab = prefabs[obstacleIndex]; // Gán prefab chướng ngại vật được chọn vào spawner
                spawner.numPlayers = numPlayers; // Gán số lượng người chơi vào spawner
                spawner.numPlanes = numPlanes; // Gán số lượng máy bay vào spawner
                spawner.numObstacles = numObstacles; // Gán số lượng chướng ngại vật vào spawner
                spawner.Spawn(); // Gọi phương thức Spawn để sinh các đối tượng trong cảnh
            }
        }
    }
}