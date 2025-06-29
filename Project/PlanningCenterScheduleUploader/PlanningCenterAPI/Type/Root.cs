using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlanningCenterAPI.Type
{
	public class Root
	{
		public Links Links { get; set; }
		public List<Data> Data { get; set; }
	}
}
