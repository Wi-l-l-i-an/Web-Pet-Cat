using UnityEngine;

public class PlaceGround : MonoBehaviour
{
    public GameObject groundObject;

    void Start()
    {
        // Get the bottom-left and bottom-right corners in world coordinates
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));

        float width = bottomRight.x - bottomLeft.x;

        // Set the position to the center of the screen bottom
        Vector3 center = new Vector3((bottomLeft.x + bottomRight.x) / 2, bottomLeft.y, 0);
        groundObject.transform.position = center;

        // Resize the collider to match screen width
        BoxCollider2D col = groundObject.GetComponent<BoxCollider2D>();
        if (col != null)
        {
            col.size = new Vector2(width, 0.2f); // Thin horizontal line
        }
    }
}
