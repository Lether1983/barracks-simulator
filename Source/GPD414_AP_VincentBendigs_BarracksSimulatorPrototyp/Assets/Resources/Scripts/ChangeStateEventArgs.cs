using UnityEngine;
using System.Collections;

public class ChangeStateEventArgs 
{
    public ChangeStateEventArgs(trusterStates newState,KompanieObject kompanie)
    {
        NewState = newState;
        targetKompanie = kompanie;
    }
    public trusterStates NewState;
    public KompanieObject targetKompanie;
}
