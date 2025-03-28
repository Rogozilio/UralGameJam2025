using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControll : MonoBehaviour
{
    public GameObject mentosMove;
    public GameObject mentosFall;
    public GameObject mentosGun;
    public GameObject toaster;
    public GameObject cockroach;
    public GameObject plant;
    public GameObject bee;
    public GameObject computer;

    public void EnableMentosMove()
    {
        DisableAll();
        mentosMove.SetActive(true);
    }

    public void EnableMentosFall()
    {
        DisableAll();
        mentosFall.SetActive(true);
    }

    public void EnableMentosGun()
    {
        DisableAll();
        mentosGun.SetActive(true);
    }

    public void EnableToaster()
    {
        DisableAll();
        toaster.SetActive(true);
    }

    public void EnableCockroach()
    {
        DisableAll();
        cockroach.SetActive(true);
    }

    public void EnablePlant()
    {
        DisableAll();
        plant.SetActive(true);
    }

    public void EnableBee()
    {
        DisableAll();
        bee.SetActive(true);
    }

    public void EnableComputer()
    {
        DisableAll();
        computer.SetActive(true);
    }

    public void DisableAll()
    {
        mentosMove.SetActive(false);
        mentosFall.SetActive(false);
        mentosGun.SetActive(false);
        toaster.SetActive(false);
        cockroach.SetActive(false);
        plant.SetActive(false);
        bee.SetActive(false);
        computer.SetActive(false);
    }
}