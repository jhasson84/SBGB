using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider boxCollider;
	private Rigidbody rb;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider> ();
		rb = GetComponent<Rigidbody> ();
		inverseMoveTime = 1f / moveTime;
	}

	//Move true if able to move, false if not
	protected bool Move (int xDir, int zDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, zDir);

		boxCollider.enabled = false; // So raycast doesn't hit own boxCollider
		hit = Physics2D.Linecast(start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null)
		{
			//If nothing hit, start SmoothMove toward endpoint
			StartCoroutine(SmoothMovement (end));
			return true;
		}
		//Something hit, return false
		return false;
	}


	protected IEnumerator SmoothMovement (Vector3 end)
	{
		//Remaining distance to be moved
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		//While that distance is greater than nearly 0 (Epsilon)
		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards(rb.position, end, inverseMoveTime * Time.deltaTime);
			rb.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null; //wait a frame before re-evaluating loop
		}
	}

	protected virtual void AttemptMove<T> (int xDir, int zDir)
		where T: Component
	{
		//Linecast in Move
		RaycastHit2D hit;
		bool canMove = Move (xDir, zDir, out hit);
		
		if(hit.transform == null)
			return; // Nothing hit by linecast

		//Component reference to whatever was hit
		T hitComponent = hit.transform.GetComponent<T> ();

		//if Can'tmove and SOMETHING is hit
		if (!canMove && hitComponent != null)
			OnCantMove (hitComponent); //Call the function that you can't move
	}

	//Override what you do when you can't move, given the component you can't move into
	protected abstract void OnCantMove <T> (T component)
		where T : Component;

}
