using System;

using UnityEngine;
using UnityEngine.AI;

namespace ET
{
	public class OperaComponent: Entity
    {
        public Vector3 ClickPoint;

	    public int mapMask;

	    public NavMeshPath Path;
    }
}
