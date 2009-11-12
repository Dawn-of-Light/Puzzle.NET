// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
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
using Puzzle.NPersist.Framework.Exceptions;
using Puzzle.NPersist.Framework.Interfaces;

namespace Puzzle.NPersist.Framework.BaseClasses
{
	public class ListInterceptor : IListInterceptor
	{
		
		public Notification Notification
		{
			get { return this.interceptable.GetInterceptor().Notification; }
		}


		#region Property PROPERTYNAME
		private string propertyName;
		public virtual string PropertyName
		{
			get
			{
				return propertyName;
			}
			set
			{
				propertyName = value;
			}
		}
		#endregion

		#region Property INTERCEPTABLE
		private IInterceptable interceptable;
		public virtual IInterceptable Interceptable
		{
			get
			{
				return interceptable;
			}
			set
			{
				interceptable = value;
			}
		}
		#endregion

		#region Property MUTENOTIFY
		private bool muteNotify;
		public virtual bool MuteNotify
		{
			get
			{
				return muteNotify;
			}
			set
			{
				muteNotify = value;
			}
		}
		#endregion

		#region Property LIST
		private IList list;
		public virtual IList List
		{
			get
			{
				return list;
			}
			set
			{
				list = value;
			}
		}
		#endregion

		#region Property OldList
		private IList oldList;
		public virtual IList OldList
		{
			get
			{
				return oldList;
			}
			set
			{
				oldList = value;
			}
		}
		#endregion

		#region Method NotifyBefore
		protected virtual void NotifyBefore()
		{
			if (Notification == Notification.Disabled) { return; }
			if (MuteNotify)
			{
				return;
			}
			if (Interceptable != null)
			{
				if (PropertyName.Length > 0)
				{				
					bool cancel=false;
					object newList = List;
					Interceptable.GetInterceptor().NotifyPropertySet(Interceptable, PropertyName, ref newList,OldList,ref cancel);
					if (cancel)
						RollBack();
				}
				else
				{
					throw new NPersistException("Managed list has not been associated with a property of the holder object!"); // do not localize
				}
			}
			else
			{
				throw new NPersistException("Managed list has not been associated with a holder object!"); // do not localize
			}
		}
		#endregion

		#region Method NotifyAfter
		protected virtual void NotifyAfter()
		{
			if (Notification != Notification.Full) { return; }
			if (MuteNotify)
			{
				return;
			}
			if (Interceptable != null)
			{
				if (PropertyName.Length > 0)
				{
					Interceptable.GetInterceptor().NotifyWroteProperty(Interceptable, PropertyName, List, OldList);
				}
				else
				{
					throw new NPersistException("Managed list has not been associated with a property of the holder object!"); // do not localize
				}
			}
			else
			{
				throw new NPersistException("Managed list has not been associated with a holder object!"); // do not localize
			}
		}
		#endregion

		//rollback the old data
		private void RollBack()
		{
			bool stackMute = MuteNotify;
			MuteNotify = true;
			this.List.Clear() ;
			foreach (object o in OldList)
				this.List.Add(o);
			MuteNotify = stackMute;
		}
	
		//called by the proxylist before executing the call to the base
		int stackLevel=0;
		public void BeforeCall()
		{
			stackLevel ++;



			if (MuteNotify)
			{
				return;
			}

			if (stackLevel == 1)
				OldList = CloneList();						
		}

		//called by the proxylist after executing the call to the base
		public void AfterCall()
		{
			stackLevel --;

			if (stackLevel > 0)
			{
				return;
			}

			if (MuteNotify)
				return;

			NotifyBefore ();
			NotifyAfter ();
			OldList = null;
			
		}

		//clones the current list
		private IList CloneList()
		{
			bool stackMute = MuteNotify;
			MuteNotify = true;
			ArrayList copy = new ArrayList();
			copy.AddRange(List) ;
			MuteNotify = stackMute;
			return copy;
		}
	}
}