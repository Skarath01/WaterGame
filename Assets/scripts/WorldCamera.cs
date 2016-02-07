using UnityEngine;
using System.Collections;

//*Class to Control the camera within the game world.
// Camera will move up and down left and right when the user hits the side of the screen in 2D space.
// Camera will check desired location and will stop if over limit.
// Camera can also be controlled by W,A,S,D, keys will call the same movement as mouse event


public class WorldCamera : MonoBehaviour {
	
	#region structs

	//box limits struct
	public struct BoxLimit{
		public float LeftLimit;
		public float RightLimit;
		public float TopLimit;
		public float BottomLimit;
	}
	#endregion

	#region class variables

	public static BoxLimit cameraLimits       = new BoxLimit();
	public static BoxLimit mouseScrollLimits  = new BoxLimit();
	public static WorldCamera Instance;

	private float cameraMoveSpeed = 60f;
	private float shiftBonus      = 45f;
	private float mouseBoundary   = 25f;

	private float mouseX;
	private float mouseY;

	private bool VerticalRotationEnabled = false;
	private float VerticalRotationMin     = 0f;//in degrees
	private float VerticalRotationMax = 65f; //in degrees

	#endregion


	void Awake()
	{
		Instance = this;
	}


	void Start () {

		//Declare camera limits
		cameraLimits.LeftLimit = 10.0f;
		cameraLimits.RightLimit = 480.0f;
		cameraLimits.TopLimit = 408.0f;
		cameraLimits.BottomLimit = -20.0f;

		//Declare Mouse Scroll Limits
		mouseScrollLimits.LeftLimit   = mouseBoundary;
		mouseScrollLimits.RightLimit  = mouseBoundary;
		mouseScrollLimits.TopLimit    = mouseBoundary;
		mouseScrollLimits.BottomLimit = mouseBoundary;
	
	}

	void LateUpdate () {

		HandelMouseRotation ();

		if(checkIfUserCameraInput())
		{
			Vector3 cameraDesireMove = GetDesiredTranslation();

			if (! isDesiredpositionOverBoundaries(cameraDesireMove)) 
			{
				this.transform.Translate(cameraDesireMove);
			}
		}

		mouseX = Input.mousePosition.x;
		mouseY = Input.mousePosition.y;
	}

	//Handels the mouse rotation vertically and horizontally
	public void HandelMouseRotation()
	{
		var easeFactor = 10f;
		if (Input.GetMouseButton (1) && Input.GetKey (KeyCode.LeftControl)) 
		{
			//Horizontal rotation
			if(Input.mousePosition.x != mouseX)
			{
				var cameraRotationY = (Input.mousePosition.x - mouseX) * easeFactor * Time.deltaTime;
				this.transform.Rotate (0, cameraRotationY, 0);
			} 

			//Vertical Rotation
			if (VerticalRotationEnabled && Input.mousePosition.y != mouseY)
			{
				GameObject MainCamera = this.gameObject.transform.FindChild("Main Camera").gameObject;

				var cameraRotationX = (mouseY - Input.mousePosition.y) * easeFactor * Time.deltaTime;
				var desiredRotationX = MainCamera.transform.eulerAngles.x + cameraRotationX;

				if(desiredRotationX >= VerticalRotationMin && desiredRotationX <= VerticalRotationMax)
				{
					MainCamera.transform.Rotate (cameraRotationX, 0, 0);
				}
			}

		}

	}

	//Check if the user is inputting commands for the camera to move 
	public bool checkIfUserCameraInput()
	{
		bool keyboardMove;
		bool mouseMove;
		bool canMove;

		//Check keyboard
		if (WorldCamera.AreCameraKeyboardButtonsPressed()) {
			keyboardMove = true;
		} else {
			keyboardMove = false;
		}

		//Check mouse Position
		if (WorldCamera.IsMousepositionWithinboundaries())
			mouseMove = true;
		else
			mouseMove = false;
		if (keyboardMove || mouseMove)
			canMove = true;
		else
			canMove = false;
		
		return canMove;

	}



	//Work out the cameras desired location depending on the players input
	public Vector3 GetDesiredTranslation()
	{
		float moveSpeed = 0f;
		float desiredX  = 0f;
		float desiredZ  = 0f;

		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
			moveSpeed = (cameraMoveSpeed + shiftBonus) * Time.deltaTime;
		else
			moveSpeed = cameraMoveSpeed * Time.deltaTime;

		//Move Via Keyboard
		if(Input.GetKey(KeyCode.W))
					desiredZ = moveSpeed;

		if(Input.GetKey(KeyCode.S))
					desiredZ = moveSpeed * -1;

		if(Input.GetKey(KeyCode.A))
					desiredX = moveSpeed * -1;

		if(Input.GetKey(KeyCode.D))
					desiredX = moveSpeed;



		//Move Via Mouse
				if(Input.mousePosition.x < mouseScrollLimits.LeftLimit){
					desiredX = moveSpeed * -1;
				}

		if(Input.mousePosition.x > (Screen.width - mouseScrollLimits.RightLimit)){
					desiredX = moveSpeed;
				}
				
				if(Input.mousePosition.y < mouseScrollLimits.BottomLimit){
					desiredZ = moveSpeed * -1;
				}

		if(Input.mousePosition.y > (Screen.height - mouseScrollLimits.TopLimit)){
					desiredZ = moveSpeed;
				}
				return new Vector3(desiredX,0,desiredZ);
	}




	//check if desired position crosses Boundaries
	public bool isDesiredpositionOverBoundaries(Vector3 desiredPosition)
	{
		bool overBoundaries = false;
		//check boundaries
		if ((this.transform.position.x + desiredPosition.x) < cameraLimits.LeftLimit)
			overBoundaries = true;
		
		if ((this.transform.position.x + desiredPosition.x) < cameraLimits.RightLimit)
			overBoundaries = true;

		if ((this.transform.position.z + desiredPosition.z) < cameraLimits.TopLimit)
			overBoundaries = true;

		if ((this.transform.position.z + desiredPosition.z) < cameraLimits.BottomLimit)
			overBoundaries = true;
		
		return overBoundaries;
	}





	#region Helper functions

	public static bool AreCameraKeyboardButtonsPressed()
	{
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D))
			return true;
		else
			return false;
	}

	public static bool IsMousepositionWithinboundaries()
	{
		if(
			(Input.mousePosition.x < mouseScrollLimits.LeftLimit && Input.mousePosition.x > -5) ||
			(Input.mousePosition.x > (Screen.width - mouseScrollLimits.RightLimit) && Input.mousePosition.x < (Screen.width + 5)) ||
			(Input.mousePosition.y < mouseScrollLimits.BottomLimit && Input.mousePosition.y > -5) ||
			(Input.mousePosition.y > (Screen.height - mouseScrollLimits.TopLimit) && Input.mousePosition.y < (Screen.height + 5))
		)
				return true;
		else return false;
	}
	#endregion
}