namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface IBlockChain<T> where T : ITransaction
    {
        T GetTransaction(string transactionId);
        void AcceptBlock(IBlock<T> block);
        int NextBlockNumber { get; }
        void VerifyChain();
    }
}
