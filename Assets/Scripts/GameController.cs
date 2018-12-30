using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;

    [SerializeField] private BlockGame _blockGame;

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            _blockGame.Move(-playerSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            _blockGame.Move(playerSpeed * Time.deltaTime);
        }

        if(_blockGame.GameOver && Input.GetKey(KeyCode.Return))
        {
            _blockGame.ResetGame();
            _blockGame.StartGame();
        }
    }
}
