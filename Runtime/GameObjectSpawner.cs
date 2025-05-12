

using UnityEngine; // Thư viện Unity Engine, cung cấp các API cơ bản của Unity

namespace Demo.GameObjectConfig // Không gian tên để tổ chức mã nguồn
{
    public class GameObjectSpawner : MonoBehaviour // Lớp kế thừa từ MonoBehaviour, cho phép gắn vào GameObject trong Unity
    {
        public GameObject playerPrefab; // Biến công khai lưu trữ prefab của người chơi
        public GameObject planePrefab; // Biến công khai lưu trữ prefab của máy bay
        public GameObject obstaclePrefab; // Biến công khai lưu trữ prefab của chướng ngại vật

        public int numPlayers = 1; // Số lượng người chơi, mặc định là 1
        public int numPlanes = 1; // Số lượng máy bay, mặc định là 1
        public int numObstacles = 5; // Số lượng chướng ngại vật, mặc định là 5

        public void Spawn() // Phương thức để sinh các đối tượng trong cảnh
        {
            // Vòng lặp để sinh các đối tượng người chơi
            for (int i = 0; i < numPlayers; i++)
                Instantiate(playerPrefab, new Vector3(i * 2, 1, 0), Quaternion.identity); 
                // Tạo một bản sao của playerPrefab tại vị trí (i * 2, 1, 0) với hướng mặc định (Quaternion.identity)

            // Vòng lặp để sinh các đối tượng máy bay
            for (int i = 0; i < numPlanes; i++)
                Instantiate(planePrefab, new Vector3(i * 5, 0, 0), Quaternion.identity); 
                // Tạo một bản sao của planePrefab tại vị trí (i * 5, 0, 0) với hướng mặc định

            // Vòng lặp để sinh các đối tượng chướng ngại vật
            for (int i = 0; i < numObstacles; i++)
                Instantiate(obstaclePrefab, new Vector3(i * 1.5f, 0, 3), Quaternion.identity); 
                // Tạo một bản sao của obstaclePrefab tại vị trí (i * 1.5, 0, 3) với hướng mặc định
        }
    }
}