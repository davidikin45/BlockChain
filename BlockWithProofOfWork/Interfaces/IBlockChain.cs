namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface IBlockChain<T> where T : ITransaction
    {
        void AcceptBlock(IBlock<T> block);
        int NextBlockNumber { get; }
        void VerifyChain();
    }
}
