using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private GameObject playerControl;
    [SerializeField]
    private int m_bulletSpeed = 10;

    private Rigidbody2D m_rigidbody2D;

	void Start ()
    {
        playerControl = GameObject.FindWithTag("Player");
        m_rigidbody2D = GetComponent<Rigidbody2D>();

        m_rigidbody2D.velocity = new Vector2(m_bulletSpeed * playerControl.transform.localScale.x, m_rigidbody2D.velocity.y);
        Vector2 temp = transform.localScale;
        temp.x = playerControl.transform.localScale.x;
        transform.localScale = temp;
    }
	
	void Update ()
    {

    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
