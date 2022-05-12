using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    Vector3 camera_position = Vector3.zero;//camera world position
    
    [SerializeField]
    private float speed;//how fast camera should move
    [SerializeField]
    private Vector2 cameraMovementLimit;//what is the border limit
    [SerializeField]
    float edgeSize = 30f;//from where screen edge starts
    private void Start()
    {
        cameraMovementLimit = LevelManager.Instance.CurrentLevel.MaxLimitForCamera;//set movement borders
    }
    void Update()
    {
        //check for input
        if (Input.GetMouseButton(0))
        {
            Touch touch = Input.GetTouch(0);//get touch
            camera_position = transform.position;//get camera current position
            if (touch.position.y >= Screen.height - edgeSize)//check for up edge
            {
                camera_position.z += speed * Time.deltaTime;//move up
            }
            if (touch.position.y <= edgeSize)//check for bottom edge
            {
                camera_position.z -= speed * Time.deltaTime;//move down
            }
            if (touch.position.x >= Screen.width - edgeSize)//check for right edge
            {
                camera_position.x += speed * Time.deltaTime;//move to right
            }
            if (touch.position.x <= edgeSize)//check for left edge
            {
                camera_position.x -= speed * Time.deltaTime;//move to left
            }
            //set camera border limits 
            camera_position.x = Mathf.Clamp(camera_position.x, -cameraMovementLimit.x, cameraMovementLimit.x);
            camera_position.z = Mathf.Clamp(camera_position.z, -cameraMovementLimit.y, cameraMovementLimit.y);
            //move camera
            transform.position = camera_position;
            if (LevelManager.Instance.TutorialIsGoing.Equals(true))
            {
                if (TutorialManager.Instance.IsTextWritingOver.Equals(true))
                {
                    if (TutorialManager.Instance.Tutorials[1])
                    {
                        TutorialManager.Instance.Tutorials[1].SetContainsAction(false);
                        TutorialManager.Instance.Tutorials[1].CheckOrder();
                    }
                }
            }
        }

        //check for touch counts
        if (Input.GetMouseButton(1))
        {
            //get second touch
            Touch touchPosition = Input.GetTouch(1);
            //rotate camera`s parent
            transform.parent.Rotate(0, touchPosition.deltaPosition.x * Time.deltaTime, 0);
            if (LevelManager.Instance.TutorialIsGoing.Equals(true))
            {
                if (TutorialManager.Instance.IsTextWritingOver.Equals(true))
                {
                    if (TutorialManager.Instance.Tutorials[2])
                    {
                        TutorialManager.Instance.Tutorials[2].SetContainsAction(false);
                        TutorialManager.Instance.Tutorials[2].CheckOrder();
                    }
                }
            }
        }

    }
    
    
}
