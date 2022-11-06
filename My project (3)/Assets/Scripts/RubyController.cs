using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    float horizontal; 
    float vertical;
    public float speed;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    [Header("Health")]
    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;

    [Header("Invincible")]
    public float timeInvincible = 2.0f;
    bool isInvincible = false;
    float invincibleTimer;

    [Header("Projectile")]
    public GameObject projectilePrefab;

    AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip throwSound;
    public AudioClip hurtSound;

    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        //Shoots off of leftShift
        if (Input.GetKeyDown(KeyCode.C))
        {
            audioSource.PlayOneShot(throwSound);
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacterScript character = hit.collider.gameObject.GetComponent<NonPlayerCharacterScript>();
                if (character != null)
                {
                   character.DisplayDialogue(); 
                }
                
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
            audioSource.PlayOneShot(hurtSound);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth); 
        UIHealthScript.instance.SetValue(currentHealth/(float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
}
