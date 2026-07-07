using UnityEngine;
public class Player_Attacks : PlayerCombat
{

    public void Combo1(CardData card)
    {
        Debug.Log("Combo1 gestartet");

        bool canAttack = CheckAttack(card);

        Debug.Log("CheckAttack: " + canAttack);

        if (!canAttack)
            return;

        Debug.Log("Greife an");

        Attack(card);
    }


    public void Combo2(CardData card)
    {
        if (!CheckAttack(card))
            return;

        Debug.Log("Combo2");

        // andere Animation

        Attack(card);
    }
}