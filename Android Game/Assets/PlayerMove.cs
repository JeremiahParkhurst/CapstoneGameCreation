using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    //Jeremy's shit
	public float maxSpeed = 10f;
	bool facingRight = true;
	Animator anim;
	public bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 450f;

    //John's shit
    //public Projectile Projectile;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("Ground", grounded);
		
		anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
		
		float move = Input.GetAxis("Horizontal");
		
		anim.SetFloat("Speed", Mathf.Abs(move));

        // physically moving character        
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		if (move>0 && !facingRight){ //flip character directionally
			Flip();
		}
		else if(move<0 && facingRight){
			Flip();
		}
	}
	
	void Update(){
		if(grounded && Input.GetKeyDown(KeyCode.Space)){
			anim.SetBool("Ground", false);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
		}
	}
	
	void Flip(){ // flip character
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
