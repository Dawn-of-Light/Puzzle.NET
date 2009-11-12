// *
// * Copyright (C) 2005 Mats Helander : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System;
using System.Data;
using Puzzle.NPersist.Framework.BaseClasses;
using Puzzle.NPersist.Framework.EventArguments;
using Puzzle.NPersist.Framework.Interfaces;

namespace Puzzle.NPersist.Framework.Persistence
{
	public class Transaction : ContextChild, ITransaction
	{
		private IDbTransaction m_DbTransaction;
		private IDataSource m_DataSource;
		private bool m_AutoPersistAllOnCommit = true;
		private bool m_OriginalKeepOpen;

		public Transaction(IDbTransaction dbTransaction, IDataSource dataSource, IContext ctx) : base(ctx)
		{
			m_DbTransaction = dbTransaction;
			m_DataSource = dataSource;
			m_OriginalKeepOpen = m_DataSource.KeepConnectionOpen;
			m_DataSource.KeepConnectionOpen = true;
		}

		public virtual IDbTransaction DbTransaction
		{
			get { return m_DbTransaction; }
			set { m_DbTransaction = value; }
		}

		public virtual void Commit()
		{
			this.Context.LogManager.Info(this, "Committing local transaction", "Data source: " + m_DataSource.Name + ", " + "Auto persist: " + m_AutoPersistAllOnCommit.ToString()  ); // do not localize	

			TransactionCancelEventArgs e = new TransactionCancelEventArgs(this, m_DataSource, this.IsolationLevel, m_AutoPersistAllOnCommit);
			this.Context.EventManager.OnCommittingTransaction(this, e);
			if (e.Cancel)
			{
				return;
			}
			m_AutoPersistAllOnCommit = e.AutoPersistAllOnCommit;			

			if (m_AutoPersistAllOnCommit)
			{
				this.Context.Commit();
			}
			m_DbTransaction.Commit();
			this.Context.OnTransactionComplete(this);
			m_DataSource.KeepConnectionOpen = m_OriginalKeepOpen;

			m_DataSource.ReturnConnection();

			TransactionEventArgs e2 = new TransactionEventArgs(this, m_DataSource, m_AutoPersistAllOnCommit);
			this.Context.EventManager.OnCommittedTransaction(this, e2);
		}

		public virtual IDbConnection Connection
		{
			get { return m_DbTransaction.Connection; }
		}

		public virtual IsolationLevel IsolationLevel
		{
			get { return m_DbTransaction.IsolationLevel; }
		}

		public virtual void Rollback()
		{
			this.Context.LogManager.Info(this, "Rolling back local transaction", "Data source: " + m_DataSource.Name + ", " + "Auto persist: " + m_AutoPersistAllOnCommit.ToString()  ); // do not localize	

			TransactionCancelEventArgs e = new TransactionCancelEventArgs(this, m_DataSource, m_AutoPersistAllOnCommit);
			this.Context.EventManager.OnRollingbackTransaction(this, e);
			if (e.Cancel)
			{
				return;
			}
			m_AutoPersistAllOnCommit = e.AutoPersistAllOnCommit;			
			m_DbTransaction.Rollback();
			this.Context.OnTransactionComplete(this);
			m_DataSource.KeepConnectionOpen = m_OriginalKeepOpen;
			m_DataSource.ReturnConnection();

			TransactionEventArgs e2 = new TransactionEventArgs(this, m_DataSource, m_AutoPersistAllOnCommit);
			this.Context.EventManager.OnRolledbackTransaction(this, e2);
		}

		public virtual void Dispose()
		{
			m_DbTransaction.Dispose();
			GC.SuppressFinalize(this);
		}

		public virtual bool AutoPersistAllOnCommit
		{
			get { return m_AutoPersistAllOnCommit; }
			set { m_AutoPersistAllOnCommit = value; }
		}

		public virtual IDataSource DataSource
		{
			get { return m_DataSource; }
			set { m_DataSource = value; }
		}
	}
}