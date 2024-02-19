using UnityEngine;

public class CopController : MonoBehaviour
{
    [SerializeField] private CharacterController character;
    [SerializeField] private float copSpeed;


    public void FixedUpdate()
    {
        if(character.speed != 0)
        copSpeed = character.speed-4f;

        Vector3 sideOfCharacter = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z-1); 

        transform.position = Vector3.Slerp(transform.position, sideOfCharacter, copSpeed * Time.deltaTime);
    }
}
