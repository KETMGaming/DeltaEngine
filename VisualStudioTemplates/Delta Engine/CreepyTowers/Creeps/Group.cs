using System.Collections.Generic;

namespace $safeprojectname$.Creeps
{
	public class Group
	{
		public string Name
		{
			get;
			set;
		}

		public List<string> CreepList
		{
			get;
			set;
		}

		public float CreepSpawnTimeInterval
		{
			get;
			set;
		}
	}
}