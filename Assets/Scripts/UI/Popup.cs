using System.Collections;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public TextMeshProUGUI popupTxt;
    public CanvasGroup canvasGroup;

    public float speed;

    public float duration = 1f; // Thời gian fade in/out

    private void Start()
    {
        canvasGroup.alpha = 0; // Ẩn popup ban đầu
    }

    public void ShowPopup(string txt)
    {
        popupTxt.text = txt; // Gán nội dung hiển thị
        StartCoroutine(ShowAndFadePopup()); // Gọi coroutine đúng
    }

    private IEnumerator ShowAndFadePopup()
    {
        Vector3 startPosition = transform.position; // Lưu vị trí ban đầu
        Vector3 targetPosition = startPosition + new Vector3(0, 100, 0); // Dịch chuyển lên trên (50 pixel)

        float time = 0;

        // Hiệu ứng fade in + bay lên
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
        transform.position = targetPosition;


        // Hiệu ứng fade out + tiếp tục bay lên
        time = 0;
        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(0, 100, 0); // Tiếp tục bay lên

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        Destroy(gameObject);
    }
}
