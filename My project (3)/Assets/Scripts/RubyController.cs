using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    [Header("Game Settings")]
    public Text scoreText;
    public Text winText;
    public Text cogText;
    public int score;
    bool gameOver;
    Rigidbody2D rigidbody2d;
    public static int gameLevel = 1;
    public int cogAmount;
    public int numCogsSpawned = 2;
    
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
    public GameObject projectileCollectible;

    [Header("Audio")]
    AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip throwSound;
    public AudioClip hurtSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    [Header("Particle System")]
    bool once;
    public GameObject collisionParticlesPrefab;
    public GameObject healthParticlesPrefab;
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        winText.gameObject.SetActive(false);
        score = 0;
        cogAmount = 3;
        updateText(cogText, cogAmount.ToString());
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (cogAmount > 0)
            {
                audioSource.PlayOneShot(throwSound);
                Launch(); 
                cogAmount = cogAmount - 1;
                updateText(cogText, cogAmount.ToString());
                
            }
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
                   if (gameOver && gameLevel == 1)
                   {
                        gameLevel = 2;
                        SceneManager.LoadScene("newScene");
                   }
                }
            }
            RaycastHit2D hitBox = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Box"));
            if (hitBox.collider != null)
            {
                ///randomCogSpawn(hitBox.collider.gameObject);
                Instantiate(projectileCollectible, hitBox.collider.gameObject.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                Destroy(hitBox.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && gameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    void updateText(Text textObject, string sentence)
    {
        textObject.text = sentence;
        textObject.gameObject.SetActive(true);
    }

    public void ChangeHealth(int amount)
    {
        GameObject particles = healthParticlesPrefab;
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
            particles = collisionParticlesPrefab;
            audioSource.PlayOneShot(hurtSound);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth); 
        UIHealthScript.instance.SetValue(currentHealth/(float)maxHealth);
        playParticleSystem(particles);

        if (currentHealth <= 0)
        {
            loseFunction();
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    public void playParticleSystem(GameObject particles)
    {
        bool once = true;
        GameObject particlesMade = Instantiate(particles, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        var particleSystem = particlesMade.GetComponent<ParticleSystem>();
        if (once)
        {
            particleSystem.Emit(10);
            once = false;
        }
    }

    public void changeScore(int amount)
    {
        score = score + amount;
        updateText(scoreText, score.ToString());
        if (score >= 6)
        {
            winFunction();
        }
    }

    void randomCogSpawn(GameObject spawnPoint)
    {
        Vector3 center = spawnPoint.transform.position;
        for (int i = 0; i < numCogsSpawned; i++)
        {
            Vector3 pos = RandomCircle(center, 5.0f);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos);
            Instantiate(projectilePrefab, pos, rot);
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    public void loseFunction()
    {
        gameOver = true;
        winText.text = "You Lose! Press R to Restart!";
        winText.gameObject.SetActive(true);
        rigidbody2d.simulated = false;
        var sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(loseSound);
    }

    public void winFunction()
    {
        if (gameLevel == 1)
        {
            gameOver = true;
            winText.text = "You fixed all the Robots! Talk to Jambi to move to level 2!";
            winText.gameObject.SetActive(true);
        }
        else if (gameLevel == 2)
        {
            gameOver = true;
            winText.text = "You win! Game Created by Logan Kilburn!";
            winText.gameObject.SetActive(true);
            audioSource.Stop();
            audioSource.PlayOneShot(winSound);
        }
    }

    public void updateCog(int amount)
    {
        cogAmount = cogAmount + amount;
        updateText(cogText, cogAmount.ToString());
    }

    
}


