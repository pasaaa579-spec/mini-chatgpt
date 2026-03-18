using System;

namespace Lib.Batching;

public class TokenBatchProvider : IBatchProvider
{
    private readonly int[] _tokens;

    public TokenBatchProvider(ITokenStream tokenStream)
    {
        _tokens = tokenStream.GetTokens();
    }

    public Batch GetBatch(int batchSize, int blockSize, Random? rng = null)
    {
        rng ??= new Random();
    
        if (_tokens.Length <= blockSize)
        {
            throw new InvalidOperationException("Потік токенів занадто малий для заданого розміру блоку.");
        }

        int[][] inputs = new int[batchSize][];
        int[] targets = new int[batchSize];

        for (int i = 0; i < batchSize; i++)
        {
            int startIndex = rng.Next(0, _tokens.Length - blockSize);

            inputs[i] = new int[blockSize];
            Array.Copy(_tokens, startIndex, inputs[i], 0, blockSize);
            
            targets[i] = _tokens[startIndex + blockSize];
        }

        return new Batch(inputs, targets);
    }
}