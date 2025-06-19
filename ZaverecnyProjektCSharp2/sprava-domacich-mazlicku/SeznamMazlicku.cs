using System.Collections.Generic;
using System.Xml.Serialization;

namespace sprava_domacich_mazlicku
{
    [XmlRoot("Mazlicci")]
    public class SeznamMazlicku
    {
        [XmlElement("Mazlicek")]
        public List<Mazlicek> Mazlicci { get; set; } = new();
    }
}