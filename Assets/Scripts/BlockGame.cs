﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BlockGame : MonoBehaviour
{
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

    public void Move(float size)
    {
        if(_gameOver) return;
        player.Move(size);
    }

    private void Update()
    {
        if(_gameOver) return;

        if(_ball.transform.position.y < player.transform.position.y)
        {
            _gameOver = true;
            Destroy(_ball.gameObject);
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