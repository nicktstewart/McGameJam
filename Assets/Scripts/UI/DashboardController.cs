using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DashboardController : MonoBehaviour
{
    public static int hasSkull;
    public static int hasLeg;
    public static int hasArm;
    public static int hp;

    public GameObject Skull;
    public GameObject Leg1;
    public GameObject Leg2;
    public GameObject Arm1;
    public GameObject Arm2;

    void FixedUpdate()
    {
        if(hp > 0)
            HP.playerHp = hp;
        else
            HP.playerHp = 0;

        if (hasSkull >= 1)
        {
            Skull.SetActive(true);
        }
        if (hasLeg >= 1)
        {
            Leg1.SetActive(true);
        }
        if (hasLeg >= 2)
        {
            Leg2.SetActive(true);
        }
        if (hasArm >= 1)
        {
            Arm1.SetActive(true);
        }
        if (hasArm >= 2)
        {
            Arm2.SetActive(true);
        }

        if(hasSkull >= 1 && hasArm >= 2 && hasLeg >= 2)
        {
            StartBossFight();
        }
    }

    void StartBossFight()
    {
        SceneManager.LoadScene("BossFight");
    }
}
