using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene (tuỳ chọn)
        }
        else
        {
            Destroy(gameObject); // Xóa nếu đã có một instance khác tồn tại
        }
    }
}
