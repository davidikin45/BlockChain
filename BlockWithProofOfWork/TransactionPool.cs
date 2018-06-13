using System.Collections.Generic;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public class TransactionPool
    {
        private readonly Queue<IAddressTransaction> _queue;

        public TransactionPool()
        {
            _queue = new Queue<IAddressTransaction>();
        }

        public void AddTransaction(IAddressTransaction transaction)
        {
            _queue.Enqueue(transaction);
        }

        public IAddressTransaction GetTransaction()
        {
            return _queue.Dequeue();
        }
    }
}
