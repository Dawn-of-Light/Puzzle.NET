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
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Interfaces;
using Puzzle.NPersist.Framework.Mapping;

namespace Puzzle.NPersist.Framework.Persistence
{
	public interface IObjectManager : IContextChild
	{

		string GetObjectIdentity(object obj);

		string GetObjectIdentity(object obj, IPropertyMap propertyMap, object value);

		void SetObjectIdentity(object obj, string identity);

		string GetObjectKey(object obj);

		string GetObjectKeyOrIdentity(object obj);

		string GetPropertyDisplayName(object obj, string propertyName);

		string GetPropertyDescription(object obj, string propertyName);

		object GetPropertyValue(object obj, string propertyName);

		void SetPropertyValue(object obj, string propertyName, object value);

		ObjectStatus GetObjectStatus(object obj);

		void SetObjectStatus(object obj, ObjectStatus value);

		PropertyStatus GetPropertyStatus(object obj, string propertyName);

		object GetOriginalPropertyValue(object obj, string propertyName);

		void SetOriginalPropertyValue(object obj, string propertyName, object value);

		void RemoveOriginalValues(object obj, string propertyName);

		bool HasOriginalValues(object obj);

		bool HasOriginalValues(object obj, string propertyName);

		bool IsDirtyProperty(object obj, string propertyName);

		bool ComparePropertyValues(object obj, string propertyName, object value1, object value2);

		bool GetNullValueStatus(object obj, string propertyName);

		void SetNullValueStatus(object obj, string propertyName, bool value);

		bool GetUpdatedStatus(object obj, string propertyName);

		void SetUpdatedStatus(object obj, string propertyName, bool value);

		void ClearUpdatedStatuses(object obj);

		void EnsurePropertyIsLoaded(object obj, string propertyName);

		void EnsurePropertyIsLoaded(object obj, IPropertyMap propertyMap);

		void InvalidateObject(object obj);

		void InvalidateProperty(object obj, string propertyName);

		long GetTimeToLive(object obj);

		long GetTimeToLive(IClassMap classMap);

		long GetTimeToLive(object obj, string propertyName);

		long GetTimeToLive(IPropertyMap propertyMap);

		TimeToLiveBehavior GetTimeToLiveBehavior(object obj);

		TimeToLiveBehavior GetTimeToLiveBehavior(IClassMap classMap);

		TimeToLiveBehavior GetTimeToLiveBehavior(object obj, string propertyName);

		TimeToLiveBehavior GetTimeToLiveBehavior(IPropertyMap propertyMap);

		LoadBehavior GetLoadBehavior(object obj);

		LoadBehavior GetLoadBehavior(Type type);

		LoadBehavior GetLoadBehavior(IClassMap classMap);

	}
}