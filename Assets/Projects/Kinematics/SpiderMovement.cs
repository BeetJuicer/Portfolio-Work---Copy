using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private float rayLength = 0.3f;
    [SerializeField] Vector3 topRayOffset = new Vector3(0f, 0.3f, 0f);
    [SerializeField] private LayerMask whatIsGround;
    private Vector3 gizmosDir;

    [SerializeField] Transform fl;
    [SerializeField] Transform fr;
    [SerializeField] Transform bl;
    [SerializeField] Transform br;
    // Update is called once per frame
    void Update()
    {
        float x =  Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, 0, z);
        gizmosDir = dir;

        bool bottomHit = Physics.Raycast(transform.position, dir, out RaycastHit bottomInfo, rayLength, whatIsGround);
        bool topHit = Physics.Raycast(transform.position + topRayOffset, dir, out RaycastHit topInfo, rayLength, whatIsGround);

        //move forward if nothing in the way.
        if ((!topHit && !bottomHit))
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
        //something in the way
        else
        {
            // if wall, climb by sertting that as the fabrik's target.
            // do a raycast from the top spot going toward our moveDirection to check if there's a wall to climb.

            //else if it's steppable, then step on it 
            // do a raycast from top->moveDir->down to check if there's something to step on.
            if(Physics.Raycast(transform.position + topRayOffset + dir * 1f, Vector3.down, out RaycastHit stepInfo, rayLength + 1, whatIsGround))
            {

            }
        }
    }

    private void OnDrawGizmos()
    {

    }
}
