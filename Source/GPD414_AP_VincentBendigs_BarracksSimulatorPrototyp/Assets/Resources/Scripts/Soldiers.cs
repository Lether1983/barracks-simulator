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
    KompanieObject ownKompanie;
    RoomObjects OwnRoom;
    RoomObjects WorkPlace;


    float TrainingsLevel;
    float isDirty;
    float tired;
    float diversity;
    float hasToUseTheToilette;
    float hungry;
    float homeIll;
    float needFitness;
}
