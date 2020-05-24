using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPerk: MonoBehaviour
{
    private ActivatableObject activatableObject;
    private InteractableObject interactableObject;

    private int ReviveAmount = 0;
    public int PerkCost = 500;

    private bool RevivePerkBrought = false;

    public Image Perk1;
    public Image Perk2;

    //private GameObject Player = GameObject.Find("Player With Gun");
    //Entity entity = Player.GetComponent<Entity>();
    // Start is called before the first frame update
    void Start()
    {
        Perk1.enabled = false;
        Perk2.enabled = false;

        activatableObject = GetComponent<ActivatableObject>();
        interactableObject = GetComponent<InteractableObject>();
    }

    private void Update()
    {   
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().Healthperk)
        {
            interactableObject.Interactable = true;
            Perk1.enabled = false;
        }
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().Reviveperk)
        {
            interactableObject.Interactable = true;
            Perk2.enabled = false;
        }
    }

    public void BuyHealth()
    {
        PointsSystem TempPoint = FindObjectOfType<PointsSystem>();
        PlayersPoints CurrentPoints = FindObjectOfType<PlayersPoints>();
        if (CurrentPoints.PointsAquired >= PerkCost)
        {
            if (activatableObject.Activated)
            {
                TempPoint.GetPlayerPoints().RemovePoints(PerkCost);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().Healthperk = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().MaxHealth += 20;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().Health = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().MaxHealth;

                Perk1.enabled = true;

                interactableObject.Interactable = false;
            }
        }
    }

    public void BuyRevive()
    {
        PointsSystem TempPoint = FindObjectOfType<PointsSystem>();
        PlayersPoints CurrentPoints = FindObjectOfType<PlayersPoints>();
        ++ReviveAmount;
        if (CurrentPoints.PointsAquired >= PerkCost)
        {
            if (activatableObject.Activated)
            {
                Perk2.enabled = true;
                switch (ReviveAmount)
                {
                    case 1:
                        TempPoint.GetPlayerPoints().RemovePoints(PerkCost);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().Reviveperk = true;

                        interactableObject.Interactable = false;
                        break;

                    case 2:
                        TempPoint.GetPlayerPoints().RemovePoints(PerkCost);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().Reviveperk = true;

                        interactableObject.Interactable = false;
                        break;

                    case 3:
                        TempPoint.GetPlayerPoints().RemovePoints(PerkCost);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().Reviveperk = true;

                        interactableObject.Interactable = false;
                        break;

                    case 4:
                        interactableObject.Interactable = false;
                        break;
                }
            }
        }
    }

}
