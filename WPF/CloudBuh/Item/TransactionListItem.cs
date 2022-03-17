using CloudBuh.Entities;

namespace CloudBuh.Item
{
    public class TransactionListItem
    {
        private TransactionEntity _entity;
        public TransactionListItem(TransactionEntity entity)
        {
            _entity = entity;
            DateOfTransaction = entity.DateOfTransaction.ToString("dd.MM.yyyy");
        }

        public double Amount => _entity.Value;
        public bool Plus => _entity.Plus;
        public string Description => _entity.Description;
        public string DateOfTransaction { get; set; }
        public int Id => _entity.Id;
    }
}
