using System.Collections;
using System.Reflection;

namespace Puzzle.NAspect.Framework
{
	public class CallInfo
	{
		public MethodBase Method;
		public IList Interceptors;
		public string MethodId;
#if NET2
        public FastInvokeHandler Handler;


        public CallInfo(string methodId,MethodBase method, IList interceptors, FastInvokeHandler handler)
        {
			MethodId = methodId;
            Method = method;
            Interceptors = interceptors;
            Handler = handler;
        }
    
#else
		public CallInfo(string methodId,MethodBase method, IList interceptors)
		{
			MethodId = methodId;
			Method = method;
			Interceptors = interceptors;
		}
#endif
	}
}