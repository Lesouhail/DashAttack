using UnityEngine;

namespace DashAttack.Physics{
	
	public interface ICollidable
	{
	    void Collide(GameObject other);
	}
	
}
