using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMovement2 : NetworkBehaviour
{
    [SerializeField]
    private Transform _playerTransform;

    [SerializeField]
    private SpriteRenderer _playerSpriteRenderer;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Light2D _atackHighlight;

    // [SerializeField]
    // private HealthBar _healthBar;

    public float maxHealth = 100f;
    public float currentHealth;
    public bool isPlayerA = true;
    public float moveSpeed = 2f;
    public float maxSpeed = 10f;
    public float atackHighlightIntensity = 2f;
    public float atackHighlightInnerAngle = 0f;
    public float atackHighlightOuterAngle = 50f;
    public float atackHighlightOuterRadius = 3f;
    private Rigidbody2D _rb;

    private Vector2 _mousePosition;
    private Vector2 _moveVelocity;

    private bool facingRight = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log($"Jogador {OwnerClientId} foi atingido por um projétil.");

            // Verifica se o projétil é do jogador atual
            if (other.GetComponent<NetworkObject>().OwnerClientId == OwnerClientId)
            {
                Debug.Log("Projétil é do jogador atual. Ignorando.");
                return;
            }

            // Dano, animação, etc
            Damage(10);

            // Destroi o projétil no servidor
            NetworkObject projectileNetObj = other.GetComponent<NetworkObject>();
            if (projectileNetObj != null)
            {
                projectileNetObj.Despawn(true);
            }
        }
    }

    private void Start()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator component not assigned in the inspector.");
        }

        if (_playerTransform == null)
        {
            Debug.LogError("Player transform not assigned in the inspector.");
        }

        if (_rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the player object.");
        }

        if (_atackHighlight == null)
        {
            Debug.LogError("FOV transform not assigned in the inspector.");
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the player object.");
        }

        // if (!IsOwner)
        //     return;
        _playerTransform = transform;

        _playerTransform.tag = IsOwner ? "PlayerA" : "PlayerB";

        // _healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        // _healthBar.SetHealth(currentHealth);

        AtackHighligthSetup();
    }

    private void Update()
    {
        if (!IsOwner)
            return;
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _playerTransform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;
        Movement();

        _animator.SetBool("isPlayerA", IsOwner);

        RotateAtackHighlight();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(10f);
        }
    }

    private void Movement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _moveVelocity = moveInput.normalized * 1f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _moveVelocity *= maxSpeed;
        }
        else
        {
            _moveVelocity *= moveSpeed;
        }

        _rb.velocity = new Vector2(_moveVelocity.x, _moveVelocity.y);

        if (_moveVelocity != Vector2.zero)
        {
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }

        Vector2 direction = _mousePosition - _rb.position;
        direction.Normalize();

        if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = _playerTransform.localScale;
        theScale.x *= -1;
        _playerTransform.localScale = theScale;
    }

    private void AtackHighligthSetup()
    {
        _atackHighlight.intensity = atackHighlightIntensity;
        _atackHighlight.pointLightInnerAngle = atackHighlightInnerAngle;
        _atackHighlight.pointLightOuterAngle = atackHighlightOuterAngle;
        _atackHighlight.pointLightOuterRadius = atackHighlightOuterRadius;
    }

    private void RotateAtackHighlight()
    {
        Vector2 _fovPosition = _atackHighlight.gameObject.transform.position;
        Vector2 direction = _mousePosition - _fovPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _atackHighlight.gameObject.transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, angle - 90)
        );
    }

    private void HasEnemyInSight()
    {
        Vector2 _fovPosition = _atackHighlight.gameObject.transform.position;
        Vector2 dir = _mousePosition - _fovPosition;
        float angle = Vector3.Angle(dir, _atackHighlight.gameObject.transform.up);
        RaycastHit2D r = Physics2D.Raycast(
            _fovPosition,
            dir,
            _atackHighlight.pointLightOuterRadius
        );
        if (angle < _atackHighlight.pointLightOuterAngle / 2)
        {
            print(r.collider.CompareTag(isPlayerA ? "PlayerB" : "PlayerA"));
            if (r.collider.CompareTag(isPlayerA ? "PlayerB" : "PlayerA"))
            {
                print("SEEN!");
                Debug.DrawRay(_fovPosition, dir, Color.red);
            }
            else
            {
                print("Dont see");
            }
        }
    }

    private void Damage(float damage)
    {
        currentHealth -= damage;
        // _healthBar.SetHealth(currentHealth);

        _playerSpriteRenderer.color = Color.red;

        if (currentHealth > 0)
        {
            StartCoroutine("ResetColor");
        }
        else
        {
            _playerSpriteRenderer.color = Color.white;
            _animator.SetTrigger("Die");
            // _healthBar.gameObject.SetActive(false);

            Destroy(gameObject, 2f);
        }
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.1f);
        _playerSpriteRenderer.color = Color.white;
    }
}
