using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityQueryServicesExt
{
	public interface ICustomsAutoDecClosing
	{
		void Send8235(DeclarationPM decPm);
	}
}
