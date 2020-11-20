using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamoLib.Entities
{
    public class ArvosteluViewModels
    {
        private List<ArvosteluViewModel> arvostelut;

        public ArvosteluViewModels(List<ArvosteluViewModel> avm)
        {
            arvostelut = avm;
        }

        public List<ArvosteluViewModel> getArvostelut()
        {
            return arvostelut;
        }

    }
}
