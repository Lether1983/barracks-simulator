using UnityEngine;
using System.Collections;

enum DinstgradGruppen { NormalSolider, TimeSoldier, ProfessionalSoldiers };

enum KompaniezugehoerigkeitsGruppen { AusbildungsKompanien, KampfKompanien, VersorgungsKompanien, LehrKompanien, KampfUnterstuetzungsKompanien, SanitaetsKompanien };

enum KampfkompanienZugehörigkeit { JaegerKompanie,PanzerKompanie,GrenadierKompanie,PanzerGrenadierKompanie};
enum VersorgungsKompanieZugehörigkeit { TransportKompanie,InstansetzungsKompanie,UnterstuetzungsKompanie,MilitaerPolizeiKompanie,VersorgungsKompanie};
enum KampfUnterstuetzungsKomapnieZugehörigkeit { EODKompanie,ABCAbwehrKompanie,PionierKompanie,HeeresfliegerKompanie,FernmeldeKompanie};
enum SanitaetsZugehörigkeit { VersorgungsSanitaeter,KampfSanitaeter,EinsatzSanitaeter,KrankenhausSanitaeter};


public class Soldiers : MonoBehaviour
{
    public KompanieObject ownKompanie;
    public RoomObjects OwnRoom;
    public RoomObjects WorkPlace;


    public float TrainingsLevel;
    public float isDirty;
    public float tired;
    public float diversity;
    public float hasToUseTheToilette;
    public float hungry;
    public float homeIll;
    public float needFitness;

    void Start()
    {
        hungry = 50;
    }
}
