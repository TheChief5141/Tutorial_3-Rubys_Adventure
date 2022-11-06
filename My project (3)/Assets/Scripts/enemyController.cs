using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    RubyController player;
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    float timer;
    int direction = 1;
    Animator animator;
    Rigidbody2D rigidbody2d;
    public ParticleSystem smokeEffect;
    AudioSource audioSource;
    public AudioClip fixClip;
    bool broken = true;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("moveX", direction);
            animator.SetFloat("moveY", 0);
        }
        
        
        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        player = other.gameObject.GetComponent<RubyController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(fixClip);
    }
}
