using UnityEngine;

/// <summary>
///  Very light behaviour for a web-page pet cat.
///  • Wanders inside a radius when left alone.
///  • Lets you click-and-drag it.
///  • Randomly plays cute extra animations (lay, itch, lick, etc.).
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PetBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float idleRadius = 1.5f;  // how far from start point it may roam
    public float walkSpeed  = 1f;    // movement speed

    Rigidbody2D rb;
    Animator    anim;
    Vector3     homePos, targetPos;
    bool        dragging;

    // list of Animator trigger names for your extra clips
    readonly string[] extraTriggers =
    {
        "Laying","itch","lick1","lick2","meow",
        "run","sit","sleep1","sleep2","stretch"
    };

    void Start()
    {
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        homePos = transform.position;

        PickNewTarget();
        InvokeRepeating(nameof(PickNewTarget), 2f, 2f);   // wander
        StartCoroutine(IdleCuteness());                   // play extras
    }

    void Update()
    {
        if (dragging)
        {
            Vector3 m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m.z = 0;
            MoveTowards(m);

            if (Input.GetMouseButtonUp(0))
                dragging = false;
        }
        else
        {
            MoveTowards(targetPos);
        }

        // tell Animator whether to use Walk or Idle clip
        anim.SetBool("isWalking", rb.linearVelocity.sqrMagnitude > 0.01f);
    }

    void MoveTowards(Vector3 dest)
    {
        Vector3 dir = (dest - transform.position).normalized;
        rb.linearVelocity = dir * walkSpeed;

        // stop when close enough
        if ((dest - transform.position).sqrMagnitude < 0.02f)
            rb.linearVelocity = Vector2.zero;
    }

    void OnMouseDown() => dragging = true;

    void PickNewTarget()
    {
        if (dragging) return;                 // don’t change while dragging

        Vector2 offset = Random.insideUnitCircle * idleRadius;
        targetPos      = homePos + new Vector3(offset.x, offset.y, 0);
    }

    System.Collections.IEnumerator IdleCuteness()
    {
        // forever loop
        while (true)
        {
            // wait 6–12 seconds (random)
            yield return new WaitForSeconds(Random.Range(3f, 6f));

            // only fire if not moving or being dragged
            if (dragging) continue;
            if (rb.linearVelocity.sqrMagnitude > 0) continue;

            // choose and trigger a random extra animation
            string t = extraTriggers[Random.Range(0, extraTriggers.Length)];
            anim.SetTrigger(t);
        }
    }
}
