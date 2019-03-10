using System;
using System.Collections.Generic;
using System.Linq;
using MLAgents;
using TMPro;
using UnityEngine;

public class BlockGame : Agent
{
    #region Agent

    public override void AgentReset()
    {
        ResetGame();
        StartGame();
    }

    public override void CollectObservations()
    {
        var position = _ball.transform.position;
        AddVectorObs(position.x);
        AddVectorObs(position.y);

        AddVectorObs(_ball.Velocity.x);
        AddVectorObs(_ball.Velocity.y);

        var position1 = player.transform.position;
        AddVectorObs(position1.x);
        AddVectorObs(position1.y);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        var f = vectorAction[0];
        Move(f);
    }

    #endregion


    public void Move(float size)
    {
        if(_gameOver) return;
        player.Move(size);
    }


    [SerializeField] private Color[] blockColors;

    [SerializeField] private Block blockPrefab;

    [SerializeField] private Ball ballPrefab;

    [SerializeField] private Vector2Int blocksSize;

    [SerializeField] private Player player;

    [SerializeField] private Transform blocks;

    [SerializeField] private TextMeshPro scoreText;


    private readonly List<Block> _blockList = new List<Block>();

    private Ball _ball;

    private int _currentLineNumber;

    private bool _gameOver = true;

    private int _score;

    public bool GameOver
    {
        get { return _gameOver; }
    }


    private void Start()
    {
        _ball = Instantiate(ballPrefab, blocks);
        _ball.Velocity = Vector2.zero;
        player.OnCollisionEnterBall += OnCollisionEnterBall;
    }

    private void OnCollisionEnterBall()
    {
        //AddReward
        Debug.Log("OnCollisionEnterBall");
        AddReward(1.0f);
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

        //Initialize
        _ball.Velocity = _ball.initVelocity;
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

        if(_ball.transform.position.y < player.transform.position.y - 1)
        {
            //Game Over
            _ball.Velocity = Vector2.zero;
            _ball.transform.localPosition = Vector3.zero;
            _gameOver = true;
            Done();
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
        //Destroy Block
        AddReward(0.5f);

        _score++;
        UpdateScore();

        var lineNumber = block.LineNumber;
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