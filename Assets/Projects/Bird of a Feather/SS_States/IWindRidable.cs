using UnityEngine;
using System.Collections;

namespace Assets.Projects.StateMachine.SideScroll.SS_States
{
    public interface IWindRidable
    {
        bool IsRidingWind { get; }
        Wind CurrentWind { get; }
        void SetWind(Wind wind);
        void EnterWind(Wind wind);
        void ExitWind();
    }
}