using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    GameObject rubyControllerObject;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rubyControllerObject = GameObject.FindWithTag("RubyController");
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Easy"))
        {
            enemyController e = other.collider.GetComponent<enemyController>();
            if (e != null)
            {
                e.Fix();
                if (rubyControllerObject != null)
                {
                    var rubyControl = rubyControllerObject.GetComponent<RubyController>();
                    rubyControl.changeScore(1);
                }
            } 
        }
        else if (other.gameObject.CompareTag("Hard"))
        {
            hardEnemyController e = other.collider.GetComponent<hardEnemyController>();
            if (e != null)
            {
                e.Fix();
                if (rubyControllerObject != null)
                {
                    var rubyControl = rubyControllerObject.GetComponent<RubyController>();
                    rubyControl.changeScore(1);
                }
            } 
        }
        else if (other.gameObject.CompareTag("RubyController") && !(rigidbody2d.velocity.magnitude > 0.01f))
        {
            RubyController e = rubyControllerObject.GetComponent<RubyController>();
            if (rubyControllerObject != null)
            {
                e.updateCog(1);
            }
        }
        
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }
}
