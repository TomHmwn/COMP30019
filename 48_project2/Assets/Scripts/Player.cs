using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : Alive
{

	// VARIABLES

	public float speed = 5;
	public float AIMTHRESHOLDISTFROMGUN = 1;

	// REFERENCES
	Camera viewCamera;
	PlayerController controller;
	GunController gunController;

	Transform aimRadius;

	public Crosshair crossHair;

	float scale = 0.2f;

	// animation references
	Animator animator;
	Transform cam;
	Vector3 camForward;
	Vector3 move;
	Vector3 moveInput;

	float forwardAmount;
	float turnAmount;

	//dissolver animation
	Material playerMat;


	protected override void Start()
	{
		base.Start();
		controller = GetComponent<PlayerController>();
		gunController = GetComponent<GunController>();

		playerMat = GetComponentInChildren<Renderer>().material;

		aimRadius = gameObject.transform.Find("radius");
		aimRadius.localScale = new Vector3(AIMTHRESHOLDISTFROMGUN, AIMTHRESHOLDISTFROMGUN, AIMTHRESHOLDISTFROMGUN) * scale;
		SetupAnimator();


		viewCamera = Camera.main;
		cam = viewCamera.transform;
	}

	void Update()
	{

		float dissolveVal = playerMat.GetFloat("_Amount");
		dissolveVal -= 0.01f;
		playerMat.SetFloat("_Amount", dissolveVal);

		// Movement in the direction we are facing
		// recieving inputs for moving horizontally and vertically
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		Vector3 input = new Vector3(horizontal, 0, vertical);


		// animator.SetFloat("Forward", horizontal);
		// animator.SetFloat("Turn", vertical);
		if (cam != null)
		{
			camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
			move = horizontal * camForward + vertical * cam.right;
		}
		else
		{
			move = horizontal * Vector3.forward + vertical * Vector3.right;
		}
		if (move.magnitude > 1)
		{
			move.Normalize();
		}
		Move(move);


		Vector3 velocity = input.normalized * speed;
		velocity = Quaternion.Euler(0, viewCamera.gameObject.transform.eulerAngles.y, 0) * velocity;
		controller.Move(velocity);

		// rotation with mouse
		//check if position of mouse intersects the player's ground

		Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
		Plane ground = new Plane(Vector3.up, Vector3.up * gunController.getGunHeight);

		// (45-48) if we want character to shoot straight only
		// float rayDistance;

		// if (ground.Raycast(ray,out rayDistance)) {
		// 	Vector3 point = ray.GetPoint(rayDistance);

		if (Physics.Raycast(ray, out RaycastHit HitPoint, maxDistance: 300f))
		{

			Vector3 point = HitPoint.point;
			// Debug.DrawLine(ray.origin,point,Color.red);
			controller.LookAt(point);
			crossHair.transform.position = point + HitPoint.normal * 1e-2f;
			crossHair.transform.LookAt(point + HitPoint.normal);
			crossHair.gottenTarget(ray);
			if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > AIMTHRESHOLDISTFROMGUN)
			{
				gunController.Aim(point);
			}
			else
			{
				point.y = gunController.getGunHeight;
				gunController.Aim(point);
			}
		}

		// Weapon inputs
		if (Input.GetMouseButton(0))
		{
			gunController.onTriggerHold();
		}
		if (Input.GetMouseButtonUp(0))
		{
			gunController.onTriggerRelease();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			gunController.Reload();
		}

		aimRadius.transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
		if (transform.position.y < -50)
		{
			TakeDamage(health);
		}


	}
	void SetupAnimator()
	{
		//ref to animator
		animator = GetComponent<Animator>();
		// use avatar from child animator component if present
		// for easy swap of character model
		foreach (var childAnimator in GetComponentsInChildren<Animator>())
		{
			if (childAnimator != animator)
			{
				animator.avatar = childAnimator.avatar;
				Destroy(childAnimator);
				break;// stop searching for more animator 
			}
		}
	}
	void Move(Vector3 move)
	{
		if (move.magnitude > 1)
		{
			move.Normalize();
		}
		this.moveInput = move;
		ConvertMoveInput();
		UpdateAnimator();
	}

	//fix animation where camera is pointing
	void ConvertMoveInput()
	{
		Vector3 localMove = transform.TransformDirection(moveInput);
		turnAmount = localMove.x;
		forwardAmount = localMove.z;
	}
	void UpdateAnimator()
	{
		animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
	}
	public override void Die()
	{
		AudioManager.instance.PlaySound("Player Death", transform.position);
		base.Die();

	}
	private void OnTriggerEnter(Collider other) {
		print(other.gameObject.name);
		if(other.gameObject.name.Contains("Health Item")){
			this.health = startingHealth;
			healthBar.SetHealth(health);
			Destroy(other.gameObject);

		}

	}

}
