using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceShipController : MonoBehaviour
{
    GameObject ruby;
    RubyController rubyController;
    public GameObject laserPrefab;
    public Transform laserParent;
    AudioSource audioPlayer;
    public AudioClip shootLaser;

    public int laserSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireBullet());
        ruby = GameObject.FindWithTag("RubyController");
        rubyController = ruby.GetComponent<RubyController>();
    }

    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = shootLaser;
    }

    // Update is called once per frame
    void Update()
    {
        if (rubyController.score >= 6)
        {
            StopCoroutine(FireBullet());
        }
    }

    public IEnumerator FireBullet()
    {
        while(true)
        {
           GameObject newLaser = Instantiate(laserPrefab, transform.position, transform.rotation);

            newLaser.transform.parent = laserParent;

            laserController e = newLaser.GetComponent<laserController>();

            e.Launch(transform.right * laserSpeed, 60f);
            audioPlayer.PlayOneShot(shootLaser);

            yield return new WaitForSeconds(2.0f); 
        }
        
    }
}
