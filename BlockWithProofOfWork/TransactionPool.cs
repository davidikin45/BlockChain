using System.Collections.Generic;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public class TransactionPool
    {
        private readonly Queue<IClaimTransaction> _queue;

        public TransactionPool()
        {
            _queue = new Queue<IClaimTransaction>();
        }

        public void AddTransaction(IClaimTransaction transaction)
        {
            _queue.Enqueue(transaction);
        }

        public IClaimTransaction GetTransaction()
        {
            return _queue.Dequeue();
        }
    }
}
