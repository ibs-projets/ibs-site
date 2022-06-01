using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteibs.Services
{
    interface IAchat
    {
        void AchatSimple(string produit, int montant);
        void AchatComplexe(string produit, int montant);
    }
}
