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
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Interfaces;
using Puzzle.NPersist.Framework.Mapping;

namespace Puzzle.NPersist.Framework.Persistence
{
	public interface IPersistenceManager : IContextChild
	{
		object GetObject(string identity, Type type);

		object GetObject(string identity, Type type, bool lazy);

		object GetObject(string identity, Type type, bool lazy, bool ignoreObjectNotFound);

		object GetObjectByKey(string keyPropertyName, object keyValue, Type type);

		object GetObjectByKey(string keyPropertyName, object keyValue, Type type, bool ignoreObjectNotFound);

		void LoadProperty(object obj, string propertyName);

		void CreateObject(object obj);

		void CommitObject(object obj, int exceptionLimit);

		void DeleteObject(object obj);

		void Commit(int exceptionLimit);

		RefreshBehaviorType RefreshBehavior { get; set; }

		MergeBehaviorType MergeBehavior { get; set; }

		MergeBehaviorType GetMergeBehavior(MergeBehaviorType mergeBehavior, IClassMap classMap, IPropertyMap propertyMap);
		
		RefreshBehaviorType GetRefreshBehavior(RefreshBehaviorType refreshBehavior, IClassMap classMap, IPropertyMap propertyMap);

		OptimisticConcurrencyBehaviorType UpdateOptimisticConcurrencyBehavior { get; set; }

		OptimisticConcurrencyBehaviorType DeleteOptimisticConcurrencyBehavior { get; set; }

		object ManageLoadedValue(object obj, IPropertyMap propertyMap, object value);

		object ManageLoadedValue(object obj, IPropertyMap propertyMap, object value, object discriminator);

		object ManageNullValue(object obj, IPropertyMap propertyMap, object value);

		object ManageReferenceValue(object obj, string propertyName, object value);

		object ManageReferenceValue(object obj, IPropertyMap propertyMap, object value);

		object ManageReferenceValue(object obj, string propertyName, object value, object discriminator);

		object ManageReferenceValue(object obj, IPropertyMap propertyMap, object value, object discriminator);

		void SetupNullValueStatuses(object obj);

		void MergeObjects(object obj, object existing, MergeBehaviorType mergeBehavior);

		void AttachObject(object obj, Hashtable visited, Hashtable merge);

		void InitializeObject(object obj);

		void SetupObject(object obj);

		OptimisticConcurrencyBehaviorType GetUpdateOptimisticConcurrencyBehavior(OptimisticConcurrencyBehaviorType optimisticConcurrencyBehavior, IClassMap classMap);

		OptimisticConcurrencyBehaviorType GetUpdateOptimisticConcurrencyBehavior(OptimisticConcurrencyBehaviorType optimisticConcurrencyBehavior, IPropertyMap propertyMap);

		OptimisticConcurrencyBehaviorType GetDeleteOptimisticConcurrencyBehavior(OptimisticConcurrencyBehaviorType optimisticConcurrencyBehavior, IClassMap classMap);

		OptimisticConcurrencyBehaviorType GetDeleteOptimisticConcurrencyBehavior(OptimisticConcurrencyBehaviorType optimisticConcurrencyBehavior, IPropertyMap propertyMap);
	}
}