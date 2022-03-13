using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private static GameObject player;
    private static AudioSource ExplosionAudio;

    [SerializeField]
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    public static void staticSetup(GameObject newPlayer, AudioSource newAudio){
        player = newPlayer;
        ExplosionAudio = newAudio;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player) {
            ExplosionAudio.Play();
            DashboardController.hp -= 5;
            GameObject clone = Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(this.gameObject, 0f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var impulse = (1f * Mathf.Deg2Rad) * rb.inertia;
        rb.AddTorque(impulse, ForceMode2D.Impulse);
    }
}
