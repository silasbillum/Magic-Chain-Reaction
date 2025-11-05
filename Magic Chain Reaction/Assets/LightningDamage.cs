using UnityEngine;
using System.Collections;

public class LightningDamage : MonoBehaviour
{
    public int damagePerTick = 1;
    public float tickInterval = 0.25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Target target = other.GetComponent<Target>();
        if (target != null)
            StartCoroutine(DamageOverTime(target));
    }

    private IEnumerator DamageOverTime(Target target)
    {
        float timer = 0f;
        while (timer < 1f) // lasts 1 second total
        {
            if (target == null) yield break;

            target.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
            timer += tickInterval;
        }
    }
}
