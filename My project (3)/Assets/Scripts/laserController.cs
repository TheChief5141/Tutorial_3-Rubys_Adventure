using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserController : MonoBehaviour
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
        if (other.gameObject.CompareTag("RubyController"))
        {
            RubyController e = rubyControllerObject.GetComponent<RubyController>();
            if (rubyControllerObject != null)
            {
                e.ChangeHealth(-1);
            }
        }
        
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 2000.0f)
        {
            Destroy(gameObject);
        }
    }
}
