﻿using UnityEngine;
using System.Collections;

// Calculates where the pulse should be instantiated
// Need to deactivate pulse game object after a certain period of time
// Need to invoke repeating

public class PulseManager : MonoBehaviour
{
    private Transform player;
    [SerializeField]
    private Transform target;
    public bool activePulse = true;
    Camera camera;
    private float x;
    private float y;
    private Vector3 lastPosition = Vector3.zero;
    
	void Start()
    {
	   camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
       player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Update()  
    {
       // Find the position to place the pulse, as an intersection of the camera bounds and the player-target vector
       CalculatePosition();
       this.transform.position = new Vector3(x, y, 0f);
    }
    
    void CalculatePosition()
    {
        if (player == null) 
        {
           // Debug.Log("Please set a player for the Pulse Manager");
           return; 
        }
        
          if (target == null)
       {
           // Debug.Log("Please set a target for the Pulse Manager");
           return;
       }
       
        // Get the bounds of the camera's screen
        Vector3 botLeft = new Vector2(player.transform.position.x - 16, player.transform.position.y-6);
        Vector3 topLeft = new Vector2(player.transform.position.x - 16, player.transform.position.y + 6);
        Vector3 botRight = new Vector2(player.transform.position.x + 16, player.transform.position.y - 6);
        Vector3 topRight = new Vector2(player.transform.position.x + 16, player.transform.position.y + 16);
        
        // Find the closest two camera points to the target 
        Vector2 camPoint1 = botLeft;
        Vector2 camPoint2 = botRight;
        
        // Target is on the left of the camera bounds
        if (botLeft.x >= target.position.x)
        {
            Vector2 intersection = lineIntersection(camPoint1, camPoint2);
            // Intersection is between botLeft and botRight
            if (intersection.y <= botLeft.y + 1 && intersection.y >= botLeft.y - 1 && intersection.x >= botLeft.x && intersection.x <= botRight.x)
            {
                //Debug.Log("Intersection is at the bottom \n x-value: " + intersection.x + " y-value: " + intersection.y);
                x = intersection.x;
                y = intersection.y;
                return;
            }
            else
            {
                // Intersection wasn't between botLeft and botRight, check botLeft and topLeft
                // Debug.Log("Intersection is at the left side \n x-value: " + intersection.x + " y-value: " + intersection.y);
                camPoint2 = topLeft;
                intersection = lineIntersection(camPoint1, camPoint2);
                x = intersection.x;
                y = intersection.y;
                return;    
            }
        }
        // Target is on the right of the camera bounds
        else if (botRight.x <= target.position.x)
        {
            // Debug.Log("botRight: " + botRight + " topRight: " + topRight + "\n botLeft: " + botLeft + " topLeft: " + topLeft);
            Vector2 intersection = lineIntersection(camPoint1, camPoint2);
            // Intersection is between botLeft and botRight
            if (intersection.y <= botLeft.y + 1 && intersection.y >= botLeft.y - 1 && intersection.x >= botLeft.x && intersection.x <= botRight.x)
            {
                // Debug.Log("Intersection is at the bottom \n x-value: " + intersection.x + " y-value: " + intersection.y);
                x = intersection.x;
                y = intersection.y;
                return;
            }
            else
            {
                // Intersection wasn't between botLeft and botRight, check botRight and topRight
                // Debug.Log("Intersection is at the right \n x-value: " + intersection.x + " y-value: " + intersection.y);
                camPoint2 = topRight;
                intersection = lineIntersection(camPoint1, camPoint2);
                x = intersection.x;
                y = intersection.y;
                return;
            }
        }
        // Intersection is between the left and right points
        else 
        {
            Vector2 intersection = lineIntersection(camPoint1, camPoint2);
            x = intersection.x;
            y = intersection.y;
            // Debug.Log("Intersection is at the right \n x-value: " + intersection.x + " y-value: " + intersection.y);
            return;
        }
    }
    
    Vector2 lineIntersection(Vector2 camPoint1, Vector2 camPoint2)
    {
        // Get A,B,C of first line - points : target to player
        float A1 = player.transform.position.y - target.position.y;
        float B1 = target.position.x - player.transform.position.x;
        float C1 = A1 * target.position.x + B1 * target.position.y;
        
        // Get A,B,C of second line - points : camPoint1 to camPoint2
        float A2 = camPoint2.y - camPoint1.y;
        float B2 = camPoint1.x - camPoint2.x;
        float C2 = A2 * camPoint1.x + B2 * camPoint1.y;
        
        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        if(delta == 0) 
        {
            throw new System.Exception("Lines are parallel");
        }
        
        // now return the Vector2 intersection point
        return new Vector2((B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta);
    }
}