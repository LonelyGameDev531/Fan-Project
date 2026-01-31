using UnityEngine;
using System.Collections.Generic;

public class UnitMovement : MonoBehaviour
{
    public float speed = 5f;

    List<Vector3> path;
    int index;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveTo(Vector3 destination)
    {
        Debug.Log($"[UnitMovement] MoveTo called: {destination}");

        Vector3 start = transform.position;

        path = Pathfinder.Instance.FindPath(start, destination);
        index = 0;

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("[UnitMovement] Path is null or empty!");
            return;
        }

        Debug.Log($"[UnitMovement] Path received with {path.Count} nodes");
    }

    void FixedUpdate()
    {
        if (path == null || index >= path.Count) return;

        Vector3 target = path[index];
        Vector3 dir = target - rb.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f)
        {
            index++;
            return;
        }

        rb.MovePosition(rb.position + dir.normalized * speed * Time.fixedDeltaTime);
    }

    void OnDrawGizmos()
    {
        if (path == null || path.Count == 0) return;
        if (index >= path.Count) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(path[index], 0.2f);

        Gizmos.color = Color.yellow;
        for (int i = 0; i < path.Count - 1; i++)
        {
            Gizmos.DrawLine(path[i], path[i + 1]);
            Gizmos.DrawSphere(path[i], 0.1f);
        }
    }
}
