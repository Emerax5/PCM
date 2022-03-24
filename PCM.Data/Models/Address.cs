using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public Providence Povidence { get; set; }
    }

    public enum Providence
    {
        Notdefined,
        Azua,
        Bahoruco,
        Barahona,
        Dajabón,
        DistritoNacional,
        Duarte,
        ElíasPiña,
        ElSeibo,
        Espaillat,
        HatoMayor,
        HermanasMirabal,
        Independencia,
        LaAltagracia,
        LaRomana,
        LaVega,
        MaríaTrinidadSánchez,
        MonseñorNouel,
        MonteCristi,
        MontePlata,
        Pedernales,
        Peravia,
        PuertoPlata,
        Samaná,
        SánchezRamírez,
        SanCristóbal,
        SanJosédeOcoa,
        SanJuan,
        SanPedrodeMacorís,
        Santiago,
        SantiagoRodríguez,
        SantoDomingo,
        Valverde

    }
}
