using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{

    public AudioClip collectibleClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {

            controller.updateCog(1);
            Destroy(gameObject);

            controller.PlaySound(collectibleClip);
            
        }
    }
}