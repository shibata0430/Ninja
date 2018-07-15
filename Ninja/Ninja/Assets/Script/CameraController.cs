using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 現状前にしか進めないので後ろにもカメラが動くように変更する
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_mainCamera;
	
	void Update ()
    {
        if (transform.position.x > m_mainCamera.transform.position.x - 4) // 4は暫定数字。のちに変数に置き換える
        {
            Vector3 cameraPos = m_mainCamera.transform.position;
            cameraPos.x = transform.position.x + 4;
            m_mainCamera.transform.position = cameraPos;
        }
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 playerPos = transform.position;
        playerPos.x = Mathf.Clamp(playerPos.x, min.x + 0.5f, max.x);
        transform.position = playerPos;
	}
}
