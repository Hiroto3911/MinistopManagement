using Infrastructure.Interfaces;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace Infrastructure.UnitOfWorks
{
    public class BaseUnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private IDbTransaction _transaction;
        private bool _disposed = false;

        public BaseUnitOfWork(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Bắt đầu transaction mới.
        /// </summary>
        public void BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            if (_context.Connection.State == ConnectionState.Closed)
                _context.Connection.Open();

            _transaction = _context.Connection.BeginTransaction(level);
            _context.Transaction = _transaction as DbTransaction;
        }

        /// <summary>
        /// Lưu thay đổi và commit transaction.
        /// </summary>
        public void Commit()
        {
            try
            {
                _context.SubmitChanges();
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        public void Rollback()
        {
            _transaction?.Rollback();
            DisposeTransaction();
        }

        private void DisposeTransaction()
        {
            _transaction?.Dispose();
            _transaction = null;
        }

        
    }
}
