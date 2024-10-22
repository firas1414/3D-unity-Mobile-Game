using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/SpeedBoost")] // THIS LINE SAYS: HEY UNUITY I KNOW I CAN'T INSTANSIATE THIS CLASS, BUT I WANT TO BE ABLE TO MAKE AN INSTANCE OF THIS CLASS IN THE ASSET
public class SpeedBoostAbility : Ability
{
    [Header("SpeedBoost")]
    [SerializeField] float boostAmt = 20f;
    [SerializeField] float boostDuration = 2f;

    Player Player;

    public override void ActivateAbility()
    {
        if (!CommitAbility()) return;

        Player = AbilityComp.GetComponent<Player>(); // RETRIEVES THE GAME OBJECT THAT HAS AbilityComponent ATTACHED TO IT(which in our case will be the player)
        Player.AddMoveSpeed(boostAmt);
        AbilityComp.StartCoroutine(RestSpeed());
    }

    IEnumerator RestSpeed()
    {
        yield return new WaitForSeconds(boostDuration);
        Player.AddMoveSpeed(-boostAmt);
    }
}
