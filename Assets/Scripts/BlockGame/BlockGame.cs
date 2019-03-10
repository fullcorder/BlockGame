using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BlockGame : MonoBehaviour
{
    # region Observation

    public class Observations
    {
        public float PlayerPosition;

        public List<Vector3> Blocks;

        public Vector2 BallVelocity;

        public Vector2 BallPosition;
    }

    public Observations GetObservations()
    {
        _blockPositionList.Clear();

        for(var i = 0; i < 36; i++)
        {
            if(i < _blockList.Count)
            {
                var localPosition = _blockList[i].transform.localPosition;
                _blockPositionList.Add(localPosition);
            }
            else
            {
                _blockPositionList.Add(Vector3.zero);
            }
        }

        return new Observations()
        {
            PlayerPosition = player.transform.localPosition.x,
            BallPosition = _ball.transform.localPosition,
            BallVelocity = _ball.Velocity,
            Blocks = _blockPositionList,
        };
    }

    #endregion


    public void Move(float size)
    {
        if(_gameOver) return;
        player.Move(size);
    }

    public Action OnScoreUpdate;

    public Action OnLostBall;


    [SerializeField] private Color[] blockColors;

    [SerializeField] private Block blockPrefab;

    [SerializeField] private Ball ballPrefab;

    [SerializeField] private Vector2Int blocksSize;

    [SerializeField] private Player player;

    [SerializeField] private Transform blocks;

    [SerializeField] private TextMeshPro scoreText;


    private readonly List<Block> _blockList = new List<Block>();

    private readonly List<Vector3> _blockPositionList = new List<Vector3>(36);

    private Ball _ball;

    private int _currentLineNumber;

    private bool _gameOver = true;

    private int _score;

    public bool GameOver
    {
        get { return _gameOver; }
    }

    public void StartGame()
    {
        if(!_gameOver) return;
        _gameOver = false;

        UpdateScore();

        int lineNumber;
        for(lineNumber = 0; lineNumber < blocksSize.y; lineNumber++)
        {
            ScrollBlocks();
            CreateLine(lineNumber);
        }

        _currentLineNumber = lineNumber;

        _ball = Instantiate(ballPrefab, blocks);
    }

    public void ResetGame()
    {
        foreach(var block in _blockList) Destroy(block.gameObject);
        _blockList.Clear();

        _score = 0;
    }

    private void Update()
    {
        if(_gameOver) return;

        if(_ball.transform.position.y < player.transform.position.y)
        {
            _gameOver = true;
            Destroy(_ball.gameObject);
            OnLostBall?.Invoke();
        }
    }

    private void CreateLine(int lineNumber)
    {
        var unitPosition = new Vector2(-2.5f, 3.9f);
        for(var x = 0; x < blocksSize.x; x++)
        {
            var block = Instantiate(blockPrefab, blocks);
            block.OnDestroy += OnDestroyBlock;
            block.transform.localPosition = new Vector3(unitPosition.x + x, unitPosition.y);
            block.LineNumber = lineNumber;
            block.SetColor(blockColors[lineNumber % blockColors.Length]);

            _blockList.Add(block);
        }
    }

    private void OnDestroyBlock(Block block)
    {
        _score++;
        UpdateScore();

        OnScoreUpdate?.Invoke();

        var lineNumber = block.LineNumber;
        block.OnDestroy -= OnDestroyBlock;
        _blockList.Remove(block);

        if(_blockList.All(b => b.LineNumber != lineNumber))
        {
            ScrollBlocks();
            CreateLine(++_currentLineNumber);
        }
    }

    private void ScrollBlocks()
    {
        _blockList.ForEach(block =>
        {
            var blockTransform = block.transform;
            var localPosition = blockTransform.localPosition;
            localPosition.y -= 0.2f;
            blockTransform.localPosition = localPosition;
        });
    }

    private void UpdateScore()
    {
        scoreText.text = $"SCORE {_score}";
    }
}