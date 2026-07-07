using UnityEngine;

public class PlayerAttacks : PlayerCombat
{
    public void Combo1(CardData card)
    {
        if (!CheckAttack(card))
            return;

        Debug.Log("Combo1");

        // Animation
        // Teleport
        // VFX

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
