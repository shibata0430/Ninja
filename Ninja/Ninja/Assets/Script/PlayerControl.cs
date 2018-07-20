using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// バグ：スペースキーを三回ほど押すと必ず、ジャンプの上昇中のアニメーション画像から遷移しなくなる。のちに修正
public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float m_maxSpeed = 5.0f;
    [SerializeField]
    private float m_jumpPower = 0.0f;
    [SerializeField]
    private GameObject bullet;

    private float m_moveSpeed = 0.0f;
    private bool m_isJump = false;
    private bool m_isGround = false;
    private bool m_isRunning = false;
    public LayerMask groundLayer;

    private Rigidbody2D m_rigidbody2D;
    private Animator m_animator;
    private State m_nowState = State.IDLE;

    public int life = 5;

    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        m_moveSpeed = Input.GetAxisRaw("Horizontal");
        m_isJump = Input.GetKeyDown(KeyCode.UpArrow);

        if (m_isJump)
            m_nowState = State.JUMP;

        ChageAnimation();
    }

    private void FixedUpdate()
    {
        if (State.DAMEAGED != m_nowState)
        {
            Move(m_moveSpeed);
            Jump();
            Attack();
        }

    }

    private void Move(float move_)
    {
        if (0 != move_)
        {
            m_rigidbody2D.velocity = new Vector2(move_ * m_maxSpeed, m_rigidbody2D.velocity.y);
            Vector2 temp = transform.localScale;                    // 反転するために一時的にスケール値を格納する変数
            temp.x = move_;
            transform.localScale = temp;
            m_nowState = State.RUN;
            m_isRunning = true;
        }
        else
        {
            m_rigidbody2D.velocity = new Vector2(0, m_rigidbody2D.velocity.y);      // 移動していなければ移動を完全に停止させる
            m_nowState = State.IDLE;
            m_isRunning = false;
        }
    }

    private void Jump()
    {
        float velY = m_rigidbody2D.velocity.y;

        if (velY < -0.1f)
            m_nowState = State.FALLING;

        // 地面に接触したかどうかの判定をし、接触していたらfalseを返す
        m_isGround = Physics2D.Linecast(transform.position, transform.position - transform.up * 1.0f, groundLayer);

        if (m_isGround && m_isJump)
        {
            m_rigidbody2D.AddForce(transform.up * m_jumpPower);
        }
    }

    private void Attack()
    {
        if (m_isGround && Input.GetKeyDown(KeyCode.Z))
            m_nowState = State.ATTACK;

        if (!m_isGround && Input.GetKeyDown(KeyCode.Z))
            m_nowState = State.JUMPATTACK;

        if (!m_isRunning && m_nowState == State.ATTACK || m_nowState == State.JUMPATTACK)
            Instantiate(bullet, transform.position + new Vector3(0.0f, 0.6f, 0.0f), transform.rotation);

        
    }

    private void ChageAnimation()
    {
        switch (m_nowState)
        {
            case State.IDLE:
                m_animator.SetBool("Run", false);
                m_animator.SetBool("jump", false);
                m_animator.SetBool("isFalling", false);
                break;
            case State.RUN:
                m_animator.SetBool("Run", true);
                m_animator.SetBool("jump", false);
                m_animator.SetBool("isFalling", false);
                break;
            case State.ATTACK:
                m_animator.SetBool("isFalling", false);
                m_animator.SetBool("jump", false);
                m_animator.SetTrigger("Attack");
                break;
            case State.JUMPATTACK:
                m_animator.SetBool("isFalling", false);
                m_animator.SetBool("jump", true);
                m_animator.SetBool("jumpAttack", true);
                break;
            case State.JUMP:
                m_animator.SetBool("Run", false);
                m_animator.SetBool("jump", true);
                m_animator.SetBool("isFalling", false);
                break;
            case State.FALLING:
                m_animator.SetBool("isFalling", true);
                m_animator.SetBool("jumpAttack", false);
                m_animator.SetBool("jump", false);
                m_animator.SetBool("Run", false);
                break;
        }

    }



    enum State
    {
        IDLE,       // 待機
        RUN,        // 移動
        ATTACK,     // 攻撃
        JUMPATTACK, // ジャンプ攻撃
        JUMP,       // ジャンプした瞬間
        FALLING,    // 落下中
        DAMEAGED,   // ダメージを受けた
    }
}
