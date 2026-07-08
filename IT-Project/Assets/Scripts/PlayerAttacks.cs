using System.Reflection;
using UnityEngine;

public class PlayerAttacks : PlayerCombat
{
    public void PlayAttack(CardData card)
    {
        MethodInfo attackMethod =
            GetType().GetMethod(
                card.cardName,
                BindingFlags.Public |
                BindingFlags.Instance);

        if (attackMethod == null)
        {
            Debug.LogWarning(
                $"Keine Methode für {card.cardName} gefunden.");
            return;
        }

        attackMethod.Invoke(
            this,
            new object[] { card });
    }


    public void Combo1(CardData card)
    {
        Debug.Log("Combo1 gestartet");
        Debug.Log(card.range);

        bool result = CheckAttack(card);

        Debug.Log("CheckAttack: " + result);

        if (!result)
            return;

        Attack(card);
    }


    public void Combo2(CardData card)
    {
        Debug.Log("Combo2");

        if (!CheckAttack(card))
            return;

        // andere Animation

        Attack(card);
    }
}