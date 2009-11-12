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
using System.Collections;
using System.Data;
using Puzzle.NPersist.Framework.Delegates;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Interfaces;
using Puzzle.NCore.Framework.Logging;
using Puzzle.NPersist.Framework.Mapping;
using Puzzle.NPath.Framework;
using Puzzle.NPersist.Framework.NPath;
using Puzzle.NPersist.Framework.Persistence;
using Puzzle.NPersist.Framework.Querying;
using Puzzle.NPersist.Framework.Validation;
#if NET2
using System.Collections.Generic;
#endif

namespace Puzzle.NPersist.Framework
{
	/// <summary>
	/// Summary description for IContext.
	/// </summary>	
	public interface IContext : IPersistenceService 
	{
		event BeginningTransactionEventHandler BeginningTransaction;
		event BegunTransactionEventHandler BegunTransaction;
		event CommittingTransactionEventHandler CommittingTransaction;
		event CommittedTransactionEventHandler CommittedTransaction;
		event RollingbackTransactionEventHandler RollingbackTransaction;
		event RolledbackTransactionEventHandler RolledbackTransaction;
		event ExecutingSqlEventHandler ExecutingSql;
		event ExecutedSqlEventHandler ExecutedSql;
		event CallingWebServiceEventHandler CallingWebService;
		event CalledWebServiceEventHandler CalledWebService;
		event CommittedEventHandler Committed;
		event CommittingEventHandler Committing;
		event CreatingObjectEventHandler CreatingObject;
		event CreatedObjectEventHandler CreatedObject;
		event InsertingObjectEventHandler InsertingObject;
		event InsertedObjectEventHandler InsertedObject;
		event DeletingObjectEventHandler DeletingObject;
		event DeletedObjectEventHandler DeletedObject;
		event RemovingObjectEventHandler RemovingObject;
		event RemovedObjectEventHandler RemovedObject;
		event CommittingObjectEventHandler CommittingObject;
		event CommittedObjectEventHandler CommittedObject;
		event UpdatingObjectEventHandler UpdatingObject;
		event UpdatedObjectEventHandler UpdatedObject;
		event GettingObjectEventHandler GettingObject;
		event GotObjectEventHandler GotObject;
		event LoadingObjectEventHandler LoadingObject;
		event LoadedObjectEventHandler LoadedObject;
		event ReadingPropertyEventHandler ReadingProperty;
		event ReadPropertyEventHandler ReadProperty;
		event WritingPropertyEventHandler WritingProperty;
		event WrotePropertyEventHandler WroteProperty;
		event LoadingPropertyEventHandler LoadingProperty;
		event LoadedPropertyEventHandler LoadedProperty;

		IInterceptor Interceptor { get; set; }

		IPersistenceManager PersistenceManager { get; set; }

		IObjectManager ObjectManager { get; set; }

		IListManager ListManager { get; set; }

		IDomainMap DomainMap { get; set; }

		IIdentityMap IdentityMap { get; set; }

		IObjectCacheManager ObjectCacheManager { get; set; }

		IReadOnlyObjectCacheManager ReadOnlyObjectCacheManager { get; set; }

		IUnitOfWork UnitOfWork { get; set; }

		IInverseManager InverseManager { get; set; }

		IEventManager EventManager { get; set; }

		IDataSourceManager DataSourceManager { get; set; }

		ISqlExecutor SqlExecutor { get; set; }

		ILogManager LogManager {get;set;}

		IPersistenceEngine PersistenceEngine { get; set; }

		IPersistenceEngineManager PersistenceEngineManager { get; set; }

		IProxyFactory ProxyFactory {get;set;}

		//ISqlEngineManager SqlEngineManager { get; set; }

		INPathEngine NPathEngine { get; set; }

		IObjectQueryEngine ObjectQueryEngine { get; set; }

		IAssemblyManager AssemblyManager { get; set; }

		IObjectFactory ObjectFactory { get; set; }

		IObjectCloner ObjectCloner { get; set; }

		IObjectValidator ObjectValidator { get; set; }

		Notification Notification { get; set; }

		bool AutoTransactions { get; set; }

		string DomainKey { get; set; }

        long ParamCounter { get; set; }

        long GetNextParamNr();


		ObjectStatus GetObjectStatus(object obj);

		PropertyStatus GetPropertyStatus(object obj, string propertyName);

		void LoadProperty(object obj, string propertyName);

        object ExecuteScalar(IQuery query);

        object ExecuteScalar(IQuery query, IDataSource dataSource);

        object ExecuteScalar(string npath, Type type);

        object ExecuteScalarByNPath(string npath, Type type);

        object ExecuteScalarByNPath(string npath, Type type, IList parameters);

        object ExecuteScalarByNPath(string npath, Type type, IList parameters, IDataSource dataSource);

		object ExecuteScalarBySql(string sql);

		object ExecuteScalarBySql(string sql, IList parameters);

		object ExecuteScalarBySql(string sql, IList parameters, IDataSource dataSource);

		void RegisterObject(object obj, ObjectStatus objectStatus);

		void RegisterObject(object obj);

		void CommitObject(object obj);

		void CommitObject(object obj, int exceptionLimit);

		void DeleteObjects(IList objects);

		void PersistAll();

		void Commit(int exceptionLimit);

		void RefreshObject(object obj);

		void RefreshObject(object obj, RefreshBehaviorType refreshBehavior);

		void RefreshObjects(IList objects);

		void RefreshObjects(IList objects, RefreshBehaviorType refreshBehavior);

		void RefreshProperty(object obj, string propertyName);

		void RefreshProperty(object obj, string propertyName, RefreshBehaviorType refreshBehavior);

		void UnloadObject(object obj);

		void UnloadObjects(IList objects);

		void AddObserver(IObserver observer);

		void AddObserver(IObserver observer, ObserverTarget observerTarget);

		void AddObserver(IObserver observer, Type type);

		void AddObserver(IObserver observer, object obj);

		void AddObserver(IObserver observer, params object[] targets);

		IList GetAllObservers();

		IList GetObservers();

		IList GetObservers(ObserverTarget observerTarget);

		IList GetObservers(Type type);

		IList GetObservers(object obj);

		IDbConnection GetConnection();

		IDbConnection GetConnection(string sourceName);

		IDbConnection GetConnection(ISourceMap sourceMap);

		void SetConnection(IDbConnection value);

		void SetConnection(IDbConnection value, string sourceName);

		void SetConnection(IDbConnection value, ISourceMap sourceMap);

		string GetConnectionString();

		string GetConnectionString(string sourceName);

		string GetConnectionString(ISourceMap sourceMap);

		void SetConnectionString(string value);

		void SetConnectionString(string value, string sourceName);

		void SetConnectionString(string value, ISourceMap sourceMap);

		IDataSource GetDataSource();

		IDataSource GetDataSource(string sourceName);

		IDataSource GetDataSource(ISourceMap sourceMap);

		ISourceMap GetSourceMap();

		ISourceMap GetSourceMap(string sourceName);

		ITransaction BeginTransaction(IsolationLevel iso);

		ITransaction BeginTransaction(bool autoPersistAllOnCommmit);

		ITransaction BeginTransaction(IsolationLevel iso, bool autoPersistAllOnCommmit);

		ITransaction BeginTransaction(IDataSource dataSource);

		ITransaction BeginTransaction(IDataSource dataSource, IsolationLevel iso);

		ITransaction BeginTransaction(IDataSource dataSource, bool autoPersistAllOnCommmit);

		ITransaction BeginTransaction(IDataSource dataSource, IsolationLevel iso, bool autoPersistAllOnCommmit);

		bool HasTransactionPending(IDataSource dataSource);

		bool HasTransactionPending();

		void OnTransactionComplete(ITransaction transaction);

		ITransaction GetTransaction(IDbConnection connection);

		void SetTransaction(IDbConnection connection, ITransaction transaction);

		object AttachObject(object obj);

		object AttachObject(object obj, MergeBehaviorType mergeBehavior);

		IList AttachObjects(IList objects);

		IList AttachObjects(IList objects, MergeBehaviorType mergeBehavior);

		IIdentityGenerator GetIdentityGenerator(string name);

		OptimisticConcurrencyMode OptimisticConcurrencyMode { get; set; } 

		Hashtable IdentityGenerators { get; }

		bool IsDisposed { get; }

		bool IsDirty { get; }

		bool IsEditing { get; }

		void BeginEdit();

		void CancelEdit();

		void EndEdit();

		ValidationMode ValidationMode { get; set; }

		bool ValidateBeforeCommit { get; set; }

		bool IsValidCache();

		void ValidateCache();

		void ValidateCache(IList exceptions);

		bool IsValidUnitOfWork();

		void ValidateUnitOfWork();

		void ValidateUnitOfWork(IList exceptions);

		bool IsValid(object obj);

		bool IsValid(object obj, string propertyName);

		void ValidateObject(object obj);

		void ValidateObject(object obj, IList exceptions);
		
		void ValidateProperty(object obj, string propertyName);

		void ValidateProperty(object obj, string propertyName, IList exceptions);

		NPathQuery GetLoadObjectNPathQuery(object obj, RefreshBehaviorType refreshBehavior);

		NPathQuery GetLoadObjectNPathQuery(object obj, string span);

		NPathQuery GetLoadObjectNPathQuery(object obj, string span, RefreshBehaviorType refreshBehavior);

		/// <summary>
		/// The timeout value in milliseconds that specifies how long this context will wait for a lock on the data source
		/// </summary>
		int Timeout { get; set; }

		IObjectCache GetObjectCache();

		long TimeToLive { get; set; }

		TimeToLiveBehavior TimeToLiveBehavior { get; set; }

		LoadBehavior LoadBehavior { get; set; }



        object TryGetObject(object identity, Type type);

        object TryGetObject(IQuery query);

        object TryGetObject(IQuery query, Type type);

        object TryGetObject(IQuery query, Type type, IList parameters);

        object TryGetObjectByNPath(string npathQuery, Type type, IList parameters, RefreshBehaviorType refreshBehavior);

        object TryGetObjectBySql(SqlQuery sqlQuery);

        object TryGetObjectBySql(string sqlQuery, Type type);

        object TryGetObjectBySql(string sqlQuery, Type type, IList parameters);

        object TryGetObjectBySql(string sqlQuery, Type type, IList parameters, RefreshBehaviorType refreshBehavior);

        object TryGetObjectByQuery(IQuery query);

        object GetObject(object identity, Type type);

        object GetObject(IQuery query);

        object GetObject(IQuery query, Type type);

        object GetObject(IQuery query, Type type, IList parameters);

        object GetObjectById(object identity, Type type, bool lazy);

        object GetObjectById(object identity, Type type, RefreshBehaviorType refreshBehavior); //Obs, special handling - needs to be converted into a query!

        object GetObjectByQuery(IQuery query);

        object GetObjectByNPath(string npathQuery, Type type, IList parameters, RefreshBehaviorType refreshBehavior);

        object GetObjectBySql(SqlQuery sqlQuery);

        object GetObjectBySql(string sqlQuery, Type type);

        object GetObjectBySql(string sqlQuery, Type type, IList parameters);

        object GetObjectBySql(string sqlQuery, Type type, IList parameters, RefreshBehaviorType refreshBehavior);

        IList GetObjectsBySql(SqlQuery sqlQuery);

        IList GetObjectsBySql(SqlQuery sqlQuery, IList listToFill);

        IList GetObjectsBySql(string sqlQuery, Type type);

        IList GetObjectsBySql(string sqlQuery, Type type, IList parameters);

        IList GetObjectsBySql(string sqlQuery, Type type, IList parameters, RefreshBehaviorType refreshBehavior);

        IList GetObjectsBySql(string sqlQuery, Type type, IList idColumns, IList typeColumns, Hashtable propertyColumnMap);

        IList GetObjectsBySql(string sqlQuery, Type type, IList idColumns, IList typeColumns, Hashtable propertyColumnMap, IList parameters);

        IList GetObjectsBySql(string sqlQuery, Type type, IList idColumns, IList typeColumns, Hashtable propertyColumnMap, IList parameters, RefreshBehaviorType refreshBehavior);

        IList GetObjectsBySql(string sqlQuery, Type type, IList idColumns, IList typeColumns, Hashtable propertyColumnMap, IList parameters, RefreshBehaviorType refreshBehavior, IList listToFill);

        IList GetObjects(Type type);

        IList GetObjects(Type type, IList listToFill);

        IList GetObjects(Type type, RefreshBehaviorType refreshBehavior);

        IList GetObjects(Type type, RefreshBehaviorType refreshBehavior, IList listToFill);

        IList GetObjects(IQuery query, Type type);

        IList GetObjects(IQuery query, Type type, IList parameters);

        IList GetObjects(IQuery query, Type type, IList parameters, RefreshBehaviorType refreshBehavior);

        IList GetObjectsByQuery(IQuery query);

        IList GetObjectsByQuery(IQuery query, IList listToFill);

        IList FilterObjects(IList objects, NPathQuery query);

        IList FilterObjects(IList objects, string npath, Type type);

        IList FilterObjects(IList objects, string npath, Type type, IList parameters);

        IList FilterObjects(NPathQuery query);

        IList FilterObjects(string npath, Type type);

        IList FilterObjects(string npath, Type type, IList parameters);

        DataTable FilterIntoDataTable(IList objects, NPathQuery query);

        DataTable FilterIntoDataTable(IList objects, string npath, Type type);

        DataTable FilterIntoDataTable(IList objects, string npath, Type type, IList parameters);

        DataTable FilterIntoDataTable(NPathQuery query);

        DataTable FilterIntoDataTable(string npath, Type type);

        DataTable FilterIntoDataTable(string npath, Type type, IList parameters);

        DataTable GetDataTable(NPathQuery query);

        DataTable GetDataTable(string npath, Type type);

        DataTable GetDataTable(string npath, Type type, IList parameters);

       


        #region .NET 2.0 Specific Code
#if NET2

        T TryGetObjectById<T> (object identity);
        T GetObjectById<T>(object identity);

        T TryGetObjectByNPath<T>(string npathQuery);
        T GetObjectByNPath<T>(string npathQuery);

        T TryGetObjectByNPath<T>(string npathQuery,params QueryParameter[] parameters);
        T GetObjectByNPath<T>(string npathQuery, params QueryParameter[] parameters);

        T TryGetObjectByNPath<T>(string npathQuery, IList parameters);
        T GetObjectByNPath<T>(string npathQuery, IList parameters);
        
        T CreateObject<T>(params object[] ctorArgs);

        IList<T> GetObjects<T>();
        IList<T> GetObjectsByNPath<T>(string npathQuery, params QueryParameter[] parameters);
        IList<T> GetObjectsByNPath<T>(string npathQuery, IList parameters);

        T[] GetArrayByNPath<T>(string npathQuery);
        T[] GetArrayByNPath<T>(string npathQuery, params QueryParameter[] parameters);
        T[] GetArrayByNPath<T>(string npathQuery, IList parameters);

        IList<snapT> GetSnapshotObjectsByNPath<snapT, sourceT>(string npathQuery);
        IList<snapT> GetSnapshotObjectsByNPath<snapT, sourceT>(string npathQuery, params QueryParameter[] parameters);
        IList<snapT> GetSnapshotObjectsByNPath<snapT, sourceT>(string npathQuery, IList parameters);
 
#endif
        #endregion
    }
}
