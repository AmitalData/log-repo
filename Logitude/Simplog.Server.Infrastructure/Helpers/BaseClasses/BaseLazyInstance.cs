using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Helpers.BaseClasses
{
	public abstract class BaseInstance<T> where T : new()
	{
		private static readonly Lazy<T> _instance = new Lazy<T>(() => new T(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);



		public static T Instance
		{
			get
			{
				return _instance.Value;
			}
		}
	}
}
