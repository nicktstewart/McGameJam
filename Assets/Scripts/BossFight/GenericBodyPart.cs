using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBodyPart : MonoBehaviour
{
    [SerializeField]
    public Sprite fossilSprite;

    [SerializeField]
    public GameObject drill;

    [SerializeField]
    public AudioSource hitSound;

    [SerializeField]
    public GenericBodyPart complement = null;

    public int partHP = 1;
    private bool boned = false;

    // Start is called before the first frame update
    void Start()
    {
        hitSound.Stop();
    }

    public void setToBone()
    {
        if (!boned) {
            this.GetComponent<SpriteRenderer>().sprite = fossilSprite;
            boned = true;
            BossController.partsAlive--;
            if (complement != null) complement.setToBone();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == drill) {
            partHP--;
            if (!boned && partHP <= 0) {
                setToBone();
                if (BossController.partsAlive > 0) hitSound.Play();
            }
        }
    }

    void BreakJoints()
    {
        HingeJoint2D hinge = GetComponent<HingeJoint2D>();
        if (hinge != null) hinge.enabled = false;
    }

}
