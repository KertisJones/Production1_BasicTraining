using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{
	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;
    private object other;

    // Use this for initialization
    protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2D = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}

	protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);

		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) {
			StartCoroutine (SmoothMovement (end));
			return true;
		}

		return false;
	}

	protected IEnumerator SmoothMovement (Vector3 end)
	{
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void AttemptMove <T> (int xDir, int yDir)
		where T : Component
	{
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);

		if (hit.transform == null)
			return;

		T hitComponent = hit.transform.GetComponent<T> ();
        Enemy myEnemyComponent = GetComponent<Enemy>();
        Wall wallComponent = hit.transform.GetComponent<Wall>();
        Enemy enemyComponent = hit.transform.GetComponent<Enemy>();
        Player myComponent = GetComponent<Player>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
        if (!canMove && myEnemyComponent != null )
        {

            if (hit.transform.gameObject.tag == "Thorns")
            {
                gameObject.GetComponent<Enemy>().TakeDamage(2);
                print("hit");
                
            }
            else if ( wallComponent != null)
            {
                EnemyHitWall(wallComponent);
                    }




        }
        if (!canMove && myComponent != null && enemyComponent != null)
        {
            EnemyAttack(enemyComponent);
        }
    }

    protected abstract void EnemyAttack<T>(T component)
        where T : Component;

    protected abstract void OnCantMove <T> (T component)
		where T : Component;

    protected abstract void EnemyHitWall<T>(T component)
        where T : Component;
}

