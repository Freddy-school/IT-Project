using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public enum AttackType
    {
        SingleTarget,
        Area
    }

    [Header("Combat")]
    public LayerMask enemyLayer;

    protected bool CheckAttack(CardData card)
    {
        switch (card.attackType)
        {
            case AttackType.SingleTarget:
                return CheckSingleTarget(card.range);

            case AttackType.Area:
                return CheckArea(card.range);

            default:
                return false;
        }
    }

    protected void Attack(CardData card)
    {
        switch (card.attackType)
        {
            case AttackType.SingleTarget:
                SingleTargetAttack(card);
                break;

            case AttackType.Area:
                AreaAttack(card);
                break;
        }
    }

    // -----------------------
    // CHECKS
    // -----------------------

    protected bool CheckSingleTarget(float range)
    {

        Debug.Log("CheckSingleTarget wird ausgeführt");
        Debug.Log("Range: " + range);

        Vector3 center =
            transform.position +
            transform.forward * (range * 0.5f);

        Vector3 halfExtents =
            new Vector3(
                1.5f,
                1f,
                range * 0.5f);


        Collider[] hits =
            Physics.OverlapBox(
                center,
                halfExtents,
                transform.rotation,
                enemyLayer);
        Debug.Log("Hits: " + hits.Length);

        return hits.Length > 0;
    }

    protected bool CheckArea(float range)
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                range,
                enemyLayer);

        foreach (Collider hit in hits)
        {
            Vector3 dir =
                (hit.transform.position -
                 transform.position).normalized;

            float angle =
                Vector3.Angle(
                    transform.forward,
                    dir);

            if (angle <= 45f)
                return true;
        }

        return false;
    }

    // -----------------------
    // ANGRIFFE
    // -----------------------

    protected void SingleTargetAttack(CardData card)
    {
        Vector3 center =
            transform.position +
            transform.forward * (card.range * 0.5f);

        Vector3 halfExtents =
            new Vector3(
                1.5f,
                1f,
                card.range * 0.5f);

        Collider[] hits =
            Physics.OverlapBox(
                center,
                halfExtents,
                transform.rotation,
                enemyLayer);

        foreach (Collider hit in hits)
        {
            Enemy_Behavior enemy =
                hit.GetComponent<Enemy_Behavior>();

            if (enemy != null)
            {
                enemy.TakeDamage(card.damage);

                // Nur erster Gegner
                break;
            }
        }
    }

    protected void AreaAttack(CardData card)
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                card.range,
                enemyLayer);

        foreach (Collider hit in hits)
        {
            Vector3 dir =
                (hit.transform.position -
                 transform.position).normalized;

            float angle =
                Vector3.Angle(
                    transform.forward,
                    dir);

            if (angle > 45f)
                continue;

            Enemy_Behavior enemy =
                hit.GetComponent<Enemy_Behavior>();

            if (enemy != null)
            {
                enemy.TakeDamage(card.damage);
            }
        }
    }
}