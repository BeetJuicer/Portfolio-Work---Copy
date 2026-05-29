using UnityEngine;
using System.Collections.Generic;

public class MoveOnWASD : MonoBehaviour
{
    private List<Command> playerMoves = new();
    private int currentMoveIndex = -1;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) Move(transform.position + Vector3.up);
        if (Input.GetKeyDown(KeyCode.S)) Move(transform.position + Vector3.down);
        if (Input.GetKeyDown(KeyCode.A)) Move(transform.position + Vector3.left);
        if (Input.GetKeyDown(KeyCode.D)) Move(transform.position + Vector3.right);
    
        if(Input.GetKeyDown(KeyCode.Z))
            Undo();
        if(Input.GetKeyDown(KeyCode.X))
            Redo();
    }
    
    private void Move(Vector3 newPos)
    {
        //remove every command after current index
        currentMoveIndex++;
        if(currentMoveIndex >= 0)
            playerMoves.RemoveRange(currentMoveIndex, playerMoves.Count - currentMoveIndex);

        print($"{currentMoveIndex}, {playerMoves.Count}");

        //MoveCommand command = new MoveCommand(transform.position, newPos, gameObject);
        //command.Execute();
        //playerMoves.Add(command);
    }

    private void Undo()
    {
        if(currentMoveIndex < playerMoves.Count && currentMoveIndex >= 0)
        {
            Debug.Log("doing");
            playerMoves[currentMoveIndex].Undo();
            currentMoveIndex --;
        }
    }

    private void Redo()
    {
        if (currentMoveIndex + 1 < playerMoves.Count && currentMoveIndex + 1 >= 0)
        {
            playerMoves[currentMoveIndex + 1].Execute();
            currentMoveIndex++;
        }
    }
}

