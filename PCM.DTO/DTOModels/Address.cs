using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class Address
    {
        [Display(Name = "Direccion 1")]
        public string Address1 { get; set; }
        [Display(Name = "Direccion 2")]
        public string Address2 { get; set; }
        [Display(Name = "Ciudad")]
        public string City { get; set; }
        [Display(Name = "Provincia")]
        public Providence Povidence { get; set; }
    }
   
    public enum Providence
    {
        [Display(Name = "-No definido-")]
        Notdefined,

        [Display(Name = "Azua")]
        Azua,

        [Display(Name = "Bahoruco")]
        Bahoruco,

        [Display(Name = "Barahona")]
        Barahona,

        [Display(Name = "Dajabón")]
        Dajabón,

        [Display(Name = "Distrito Nacional")]
        DistritoNacional,

        [Display(Name = "Duarte")]
        Duarte,

        [Display(Name = "Elías Piña")]
        ElíasPiña,

        [Display(Name = "El Seibo")]
        ElSeibo,

        [Display(Name = "Espaillat")]
        Espaillat,

        [Display(Name = "Hato Mayor")]
        HatoMayor,

        [Display(Name = "Hermanas Mirabal")]
        HermanasMirabal,

        [Display(Name = "Independencia")]
        Independencia,

        [Display(Name = "La Altagracia")]
        LaAltagracia,

        [Display(Name = "La Romana")]
        LaRomana,

        [Display(Name = "La Vega")]
        LaVega,

        [Display(Name = "María Trinidad Sánchez")]
        MaríaTrinidadSánchez,

        [Display(Name = "Monseñor Nouel")]
        MonseñorNouel,

        [Display(Name = "Monte Cristi")]
        MonteCristi,

        [Display(Name = "Monte Plata")]
        MontePlata,

        [Display(Name = "Pedernales")]
        Pedernales,

        [Display(Name = "Peravia")]
        Peravia,

        [Display(Name = "Puerto Plata")]
        PuertoPlata,

        [Display(Name = "Samaná")]
        Samaná,

        [Display(Name = "Sánchez Ramírez")]
        SánchezRamírez,

        [Display(Name = "San Cristóbal")]
        SanCristóbal,

        [Display(Name = "San José de Ocoa")]
        SanJosédeOcoa,

        [Display(Name = "San Juan")]
        SanJuan,

        [Display(Name = "San Pedro de Macorís")]
        SanPedrodeMacorís,

        [Display(Name = "Santiago")]
        Santiago,

        [Display(Name = "Santiago Rodríguez")]
        SantiagoRodríguez,

        [Display(Name = "Santo Domingo")]
        SantoDomingo,

        [Display(Name = "Valverde")]
        Valverde

    }
}
