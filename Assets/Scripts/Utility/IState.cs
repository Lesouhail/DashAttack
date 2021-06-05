using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashAttack.Utility{
	
	public interface IState
	{
	    public Action StateEntered { get; set; }
	    public Action StateUpdated { get; set; }
	    public Action StateExited { get; set; }
	
	    public void OnStateEnter();
	
	    public void Update();
	
	    public void OnStateExit();
	}
}